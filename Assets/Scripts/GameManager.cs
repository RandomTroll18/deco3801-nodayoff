using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : Photon.PunBehaviour {

	public Vector3 BoardingSpawn;
	public int RoundsLeftUntilLose;
	public int InitialNumberOfTurns; // The initial number of turns
	public GameObject[] OpenedDoors;
	public Text RemainingTurnsText; // The text counter for the remaining turns

	/*
	 * Remember whether the current turn is a valid turn. 
	 * 
	 * A turn is invalid only if every player has 'ended their turn'.
	 * Invalid turns are where the game manager can do all required 
	 * calculations.
	 * 
	 * A valid turn is when the game manager is done with calculations
	 * and any actions it needs to do and allows players to move
	 */
	bool validTurn;
	bool loaded = false; // Record if all players have been loaded

	int numPlayers;
	int roundsLost = 1;
	int playersLeft; // The number of players still active
	Dictionary<Tile, GameObject> doors = new Dictionary<Tile, GameObject>(); // The doors
	MovementController movController; // Movement controller script

	GameObject waitingCanvas; // The canvas to be displayed when waiting for other players to load
	GameObject instantiatedCanvas; // The instantiated canvas
	NetworkingManager networkingManager; // The networking manager
	HashSet<PhotonPlayer> readyPlayers = new HashSet<PhotonPlayer>(); // Players that have already loaded
	HashSet<PhotonPlayer> pendingInstantiation = new HashSet<PhotonPlayer>(); // Players that are loading at the moment


	/**
	 * Wait for other players to join before starting the game
	 */
	void waitForPlayers() {
		// Player models that have been instantiated
		HashSet<GameObject> playerModels = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Player")); 

		instantiatedCanvas.GetComponentInChildren<Text>().text = 
				"Waiting for " + (PhotonNetwork.playerList.Length - readyPlayers.Count) + " players to finish loading";
		/* Get player models */
		playerModels = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

		Debug.Log("Number of ready players: " + readyPlayers.Count);
		Debug.Log("Number of instantiated player models: " + playerModels.Count);
		Debug.Log("Number of photon players: " + PhotonNetwork.playerList.Length);

		foreach (PhotonPlayer networkPlayer in PhotonNetwork.playerList) { // Check every player
			if (networkPlayer == PhotonNetwork.player) {
				Debug.Log("Looking at current player. Skip");
				continue; // We know for sure that we've instantiated our player model
			} else if (readyPlayers.Contains(networkPlayer)) {
				Debug.Log(networkPlayer.name + " is ready. Skip");
				continue; // This player has already instantiated their player model
			} else if (pendingInstantiation.Contains(networkPlayer))
				Debug.Log(networkPlayer.name + " is processing the RPC request. Skip");

			foreach (GameObject playerModel in playerModels) { // Look at all the player models
				if (playerModel.GetComponent<PhotonView>().ownerId == networkPlayer.ID) { 
					Debug.Log(networkPlayer.name + " is ready");
					readyPlayers.Add(networkPlayer); // We can see the player's model
					if (pendingInstantiation.Contains(networkPlayer)) {
						Debug.Log("Pending player (" + networkPlayer.name + ") has loaded");
						pendingInstantiation.Remove(networkPlayer);
					}
					break;
				}
			}
			if (!readyPlayers.Contains(networkPlayer)) { // Tell player to re-instantiate their model
				if ((bool)(networkPlayer.customProperties["waiting"]) && !pendingInstantiation.Contains(networkPlayer)) {
					// Player has supposedly instantiated their player
					Debug.Log(networkPlayer.name + " is waiting. Ask to re-instantiate their player");
					networkingManager.gameObject.GetComponent<PhotonView>().RPC(
							"RequestInitializingOfPlayer",
							networkPlayer,
							new object[] {PhotonNetwork.player}
					);
					pendingInstantiation.Add(networkPlayer);
				} else
					Debug.Log(networkPlayer.name + " is still selecting their class");
			} else
				Debug.Log(networkPlayer.name + " has already loaded");
		}

		if (readyPlayers.Count >= PhotonNetwork.playerList.Length) {
			Debug.Log("All players have been loaded");
			loaded = true;
			Destroy(instantiatedCanvas);
		}
	}

	/**
	 * Response from a player to re-instantiate a player
	 * 
	 * Arguments
	 * - Vector3 positionToSpawn - The position to spawn
	 * - Quaternion rotationToSpawn - The rotation of object upon being spawned
	 * - PhotonPlayer trueOwner - The true owner of the player object
	 * - int trueOwnerId - The id of the owner
	 * - int trueViewId - The view id of the corresponding player's prefab
	 */
	[PunRPC]
	public void InstantiateResponse(Vector3 positionToSpawn, Quaternion rotationToSpawn, 
			PhotonPlayer trueOwner, int trueOwnerId, int trueViewId) {
		GameObject playerObject = Resources.Load<GameObject>("Prefabs/Resources/Player"); // Player prefab
		GameObject instantiatedPlayer; // The instantiated player
		PhotonView instantiatedView; // The instantiated view

		Debug.LogError("Received RPC response to instantiate a player model for: " + trueOwner.name);
		Debug.LogError("Received RPC response to instantiate a player model with id of: " + trueViewId);
		/* Instantiate the player and change the ownership */
		instantiatedPlayer = Instantiate(playerObject);
		instantiatedView = instantiatedPlayer.GetComponent<PhotonView>();

		Debug.LogError("Is view enabled: " + instantiatedView.enabled);
		instantiatedPlayer.transform.position = positionToSpawn;
		instantiatedPlayer.transform.rotation = rotationToSpawn;
		instantiatedView.TransferOwnership(trueOwner);
		instantiatedView.ownerId = trueOwnerId;
		instantiatedView.viewID = trueViewId;
		instantiatedView.enabled = true;

		Debug.LogError("Instantiated player for: " + trueOwner.name + ", owner is: " + instantiatedView.owner.name);
		Debug.LogError("New owner id: " + instantiatedView.ownerId);
		Debug.LogError("New view id: " + instantiatedView.viewID);
	}

	/*
	 * From now on, it's safer to replace Start with StartMe in all your classes and to 
	 * call StartMe in StartScripts
	 */
	public void StartMe() {
		StartPlaying();
		// Set custom properties for ourselves
		PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
				{"waiting", true}
		});
		InitialNumberOfTurns = RoundsLeftUntilLose;

		/* Load everything necessary for waiting for other players to load */
		waitingCanvas = Resources.Load<GameObject>("Prefabs/UIPrefabs/WaitingCanvas");
		networkingManager = Object.FindObjectOfType<NetworkingManager>(); // Networking manager
		instantiatedCanvas = Instantiate(waitingCanvas); // Create UI to block player from doing anything
		readyPlayers.Add(PhotonNetwork.player); // We have already instantiated our player
	}

	[PunRPC]
	public void AddPlayer() {
		numPlayers++;
		playersLeft++;
		Debug.Log("Player list size: " + playersLeft);
		playersLeft = PhotonNetwork.playerList.Length;
		Debug.Log("Player list size (after checking photon): " + playersLeft);
	}

	void StartScripts() {
		PlayerClass classOfPlayer; // The player's class
		AlienClass alienClass; // The alien's class

		Player.MyPlayer.GetComponent<Player>().StartMe();
		Player.MyPlayer.GetComponent<MovementController>().StartMe();
		Player.MyPlayer.GetComponentInChildren<CameraController>().StartMe();
		Player.MyPlayer.GetComponentInChildren<Camera>().enabled = true;
		Player.MyPlayer.GetComponent<MovementController>().enabled = true;
		Player.MyPlayer.GetComponentInChildren<CameraController>().enabled = true;
		Player.MyPlayer.GetComponent<Player>().enabled = true;
		Player.MyPlayer.GetComponent<Player>().IsSpawned = false; // This isn't spawned
		Player.MyPlayer.GetComponent<BoxCollider>().enabled = true;
		Object.FindObjectOfType<Poll>().StartMe();
		Object.FindObjectOfType<MainCanvasButton>().StartMe();
		Object.FindObjectOfType<ContextAwareBox>().StartMe();
		Object.FindObjectOfType<InventoryUIScript>().StartMe();
		/*
		 * This is how you call StartMe for classes that aren't singletons
		 */
		foreach (InventoryUISlotScript script in Object.FindObjectsOfType<InventoryUISlotScript>()) {
			Debug.Log("Start inventory ui slots");
			script.StartMe();
		}
		
		Object.FindObjectOfType<ChatTest>().StartMe();
		// TODO: Before initializing the class panel, need to check if the player is an alien
		classOfPlayer = Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject();
		if (classOfPlayer.GetClassTypeEnum() == Classes.BETRAYER) { // Alien class
			alienClass = (AlienClass)classOfPlayer;
			alienClass.SetHumanClass();
		}
		Object.FindObjectOfType<ClassPanelScript>().StartMe();
		Object.FindObjectOfType<EffectPanelScript>().StartMe();
		Object.FindObjectOfType<PrimaryObjectiveController>().StartMe();
		Object.FindObjectOfType<EndTurnButtonScript>().enabled = true;
		Object.FindObjectOfType<EndTurnButtonScript>().StartMe();
		Object.FindObjectOfType<APCounterScript>().StartMe();
		Object.FindObjectOfType<APCounterScript>().enabled = true;
	}

	public void StartPlaying() {
		StartScripts();

		/*
		 * This is where the old Start() started
		 */
		movController = Player.MyPlayer.GetComponent<MovementController>();

		//TODO : Still in thinking of a better way to do it. -Ken
		foreach (InteractiveObject script in Object.FindObjectsOfType<InteractiveObject>())
			script.StartMe(this);
		foreach (Trap script in Object.FindObjectsOfType<Trap>())
			script.StartMe(this);

		InitalizeDoors();
		TurnOnLighting();
		validTurn = false;
		//		Debug.Log("Valid turn is: " + validTurn + " by default");
		//		Debug.Log("Number of players left: " + playersLeft);
		RemainingTurnsText.text = "Rounds Remaining: " + RoundsLeftUntilLose;

		enabled = true; // Turns on the Update() function
	}

	void InitalizeDoors() {
		foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door")) {
			doors.Add(Tile.TilePosition(door.transform.position), door);
		}

		foreach (GameObject openDoor in OpenedDoors) {
			if (openDoor != null) {
				OpenDoor(Tile.TilePosition(openDoor.transform.position));
			}
		}
	}

	void TurnOnLighting() {
		Color color;
		Color.TryParseHexString("323232FF", out color);
		RenderSettings.ambientLight = color;
		RenderSettings.fog = true;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Lighting"))
			g.GetComponent<Light>().enabled = true;
	}

	/**
	 * Update function. Needs to be done:
	 * - if it is a valid turn and there are still active players, 
	 * don't do anything
	 * - if it is a valid turn but there are no more active players, 
	 * set the turn to be invalid
	 * - if it is an invalid turn, then do any required actions
	 */
	void Update() {
		if (!loaded) // Wait for other players to join first
			waitForPlayers();
		if (validTurn) {
			if (playersLeft <= 0) { // No more active players
				validTurn = false;
			}
			return;
		}

		//Debug.Log ("Invalid turn. Game Manager doing stuff");
		//playersLeft = numPlayers; Doesn't take into account players disconnected mid-game
		playersLeft = PhotonNetwork.playerList.Length; // Need to check photons list of players, not models

		// Initialize player stats - AP and apply player effects
		try { 
			Player p = Player.MyPlayer.GetComponent<Player>();
			p.InitializeStats();
			p.ApplyTurnEffects();
			p.ReduceItemCoolDowns();
			p.ReduceAbilityTurns();
			p.SetInActivity(false);
		} 
		catch (MissingReferenceException e) { // Handle Security System Kill state
			Application.LoadLevel("GameOver");
		}

		RoundsLeftUntilLose -= roundsLost;
		RemainingTurnsText.text = "Rounds Remaining: " + RoundsLeftUntilLose;
		if (RoundsLeftUntilLose <= 0)
			GetComponent<PhotonView>().RPC("LoseGame", PhotonTargets.All, null);

		validTurn = true;
	}

	[PunRPC]
	void LoseGame() {
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) // Aliens win
			Application.LoadLevel("WinScreen");
		else // Humans lose
			Application.LoadLevel("GameOver");
	}

	/**
	 * Returns whether it is a vaid turn or not
	 * 
	 * Returns
	 * - true if the current turn is still valid. 
	 * - false if otherwise
	 */
	public bool IsValidTurn() {
		return validTurn;
	}

	/**
	 * Record that a player has set themselves to be active again
	 */
	[PunRPC]
	public void SetActivePlayer() {
		playersLeft++;
		Debug.Log("Players left (after setting to active): " + playersLeft);
	}

	/**
	 * Record that a player is no longer active
	 */
	[PunRPC]
	public void SetInactivePlayer() {
		playersLeft--;
		Debug.Log("Players left (after setting to inactive): " + playersLeft);
	}

	/**
	 * Opens the door at the given position. This means the door will start its animation and will
	 * no longer block that tile.
	 */
	public void OpenDoor(Tile position) {
		GameObject door;
		if (!doors.TryGetValue(position, out door)) {
			Debug.LogWarning("You tried to open a door that doesn't exist on that tile. BEN");
			return;
		}

		door.GetComponent<Animator>().enabled = true;
		door.GetComponent<DoorAudio>().PlayOpeningEfx(); // Play opening sound effects
		movController.UnblockTile(position);
	}

	public MovementController GetPlayerControllers(){
		return movController;
	}

	/*
	 * Increases the number of rounds lost after a turn.
	 */
	[PunRPC]
	public void IncreaseRoundsLost(int newRoundsLost) {
		this.roundsLost += newRoundsLost;
	}

	/*
	 * Decreases the number of rounds lost after a turn.
	 */
	[PunRPC]
	public void DecreaseRoundsLost(int newRoundsLost) {
		this.roundsLost -= newRoundsLost;
	}

	public int GetPlayersLeft() {
		return playersLeft;
	}

	[PunRPC]
	public void SpawnBoardingParty(Vector3 spawn) {
		BoardingSpawn = spawn;
		GameObject secondaries 
			= Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;
		secondaries.AddComponent<BoardingParty>();
	}

	[PunRPC]
	public void EventCardMessage(string message) {
		GenericCard gc = gameObject.AddComponent<GenericCard>();
		gc.message = message;
		gc.CreateCard();
	}
}
