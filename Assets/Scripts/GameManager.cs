using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : Photon.PunBehaviour {

	public Vector3 BoardingSpawn; // The spawn point of the boarding party
	public int RoundsLeftUntilLose; // The rounds left before humans lose
	public int InitialNumberOfTurns; // The initial number of turns
	public GameObject[] OpenedDoors; // The opened doors
	public Text RemainingTurnsText; // The text counter for the remaining turns
	public List<Tile> UnlockedTiles = new List<Tile>(); // The tiles we have unlocked
	int playersEscaped = 0; // Record the number of players that escaped
	bool playersKilled = false; // Record if a human player has been killed

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

	int numPlayers; // The number of players in the game
	int roundsLost = 1; // The number of rounds we lose every ending of a turn
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
		HashSet<GameObject> playerModels; // Player models that have been instantiated
		if (Application.loadedLevelName != "Tutorial")
		instantiatedCanvas.GetComponentInChildren<Text>().text = 
				"Waiting for " + (PhotonNetwork.playerList.Length - readyPlayers.Count) + " players to finish loading";

		// Get player models
		playerModels = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

		foreach (PhotonPlayer networkPlayer in PhotonNetwork.playerList) { // Check every player
			if (networkPlayer == PhotonNetwork.player) // We aren't looking at ourselves
				continue; // We know for sure that we've instantiated our player model
			else if (readyPlayers.Contains(networkPlayer)) // This player is already ready
				continue; // This player has already instantiated their player model

			foreach (GameObject playerModel in playerModels) { // Look at all the player models
				if (playerModel.GetComponent<PhotonView>().ownerId == networkPlayer.ID) { // This player is ready
					readyPlayers.Add(networkPlayer); // We can see the player's model
					if (pendingInstantiation.Contains(networkPlayer)) // This was a pending player
						pendingInstantiation.Remove(networkPlayer);
					break;
				}
			}
			if (!readyPlayers.Contains(networkPlayer)) { // Tell player to re-instantiate their model
				if ((bool)(networkPlayer.customProperties["waiting"]) 
						&& !pendingInstantiation.Contains(networkPlayer)) {
					// Player has supposedly instantiated their player
					networkingManager.gameObject.GetComponent<PhotonView>().RPC(
							"RequestInitializingOfPlayer",
							networkPlayer,
							new object[] {PhotonNetwork.player}
					);
					pendingInstantiation.Add(networkPlayer); // We are pending this player to spawn their model
				}
			}
		}

		if (readyPlayers.Count >= PhotonNetwork.playerList.Length) { // All players have loaded
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

		/* Instantiate the player and change the ownership */
		instantiatedPlayer = Instantiate(playerObject);
		instantiatedView = instantiatedPlayer.GetComponent<PhotonView>();
		instantiatedPlayer.transform.position = positionToSpawn;
		instantiatedPlayer.transform.rotation = rotationToSpawn;
		instantiatedView.TransferOwnership(trueOwner);
		instantiatedView.ownerId = trueOwnerId;
		instantiatedView.viewID = trueViewId;
		instantiatedView.enabled = true;
	}

	/*
	 * Start this script
	 */
	public void StartMe() {
		StartPlaying();

		/* Set ourselves to be waiting for other players to join */
		PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
				{"waiting", true}
		});
		InitialNumberOfTurns = RoundsLeftUntilLose;

		/* Load everything necessary for waiting for other players to load */
		waitingCanvas = Resources.Load<GameObject>("Prefabs/UIPrefabs/WaitingCanvas");
		networkingManager = Object.FindObjectOfType<NetworkingManager>(); // Networking manager
		if (Application.loadedLevelName != "Tutorial")
		instantiatedCanvas = Instantiate(waitingCanvas); // Create UI to block player from doing anything
		readyPlayers.Add(PhotonNetwork.player); // We have already instantiated our player

		/* Send an RPC to master client to tell a player to select their class */
		if (!(bool)(PhotonNetwork.masterClient.customProperties["waiting"])) // Master client is in match maker
			GetComponent<PhotonView>().RPC("MatchMakerSendPlayerToClassSelect", PhotonNetwork.masterClient, null);
		else // Master client is in the same scene already
			GetComponent<PhotonView>().RPC("GameManagerSendPlayerToClassSelect", PhotonNetwork.masterClient, null);

	}

	/**
	 * Destroy the player model with the given owner id
	 * 
	 * Arguments
	 * - int ownerId - The owner id
	 */
	[PunRPC]
	public void DestroyDisconnectedPlayerModel(int ownerId) {
		Debug.LogError("Destroying Disconnected Player Model");
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.GetComponent<PhotonView>().ownerId == ownerId) { // Found the player
				Debug.LogWarning("Game Manager: Found disconnected player model");
				Destroy(player);
				return;
			}
		}
	}

	/**
	 * Send a player to class select screen - master client only
	 */
	[PunRPC]
	public void GameManagerSendPlayerToClassSelect() {
		Debug.LogWarning("Sending a player to class select from game manager");
	}

	/**
	 * RPC call for adding a player to the current game
	 */
	[PunRPC]
	public void AddPlayer() {
		numPlayers++;
		playersLeft++;
		playersLeft = PhotonNetwork.playerList.Length;
	}

	void StartScripts() {
		PlayerClass classOfPlayer; // The player's class
		AlienClass alienClass; // The alien's class

		/* Start the player */
		Player.MyPlayer.GetComponent<Player>().StartMe();
		Player.MyPlayer.GetComponent<MovementController>().StartMe();
		Player.MyPlayer.GetComponentInChildren<CameraController>().StartMe();
		Player.MyPlayer.GetComponentInChildren<Camera>().enabled = true;
		Player.MyPlayer.GetComponent<MovementController>().enabled = true;
		Player.MyPlayer.GetComponentInChildren<CameraController>().enabled = true;
		Player.MyPlayer.GetComponent<Player>().enabled = true;
		Player.MyPlayer.GetComponent<Player>().IsSpawned = false; // This isn't spawned
		Player.MyPlayer.GetComponent<BoxCollider>().enabled = true;

		/* Start everything else */
		Object.FindObjectOfType<Poll>().StartMe();
		Object.FindObjectOfType<MainCanvasButton>().StartMe();
		Object.FindObjectOfType<ContextAwareBox>().StartMe();
		Object.FindObjectOfType<InventoryUIScript>().StartMe();

		foreach (InventoryUISlotScript script in Object.FindObjectsOfType<InventoryUISlotScript>())
			script.StartMe();
		
		Object.FindObjectOfType<ChatTest>().StartMe();
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
		movController = Player.MyPlayer.GetComponent<MovementController>(); // Get movement controller

		foreach (InteractiveObject script in Object.FindObjectsOfType<InteractiveObject>())
			script.StartMe(this);
		foreach (Trap script in Object.FindObjectsOfType<Trap>())
			script.StartMe(this);

		InitalizeDoors();
		TurnOnLighting();
		validTurn = false;
		RemainingTurnsText.text = "Rounds Remaining: " + RoundsLeftUntilLose;

		enabled = true; // Turns on the Update() function
	}

	/**
	 * Initialize all the doors
	 */
	void InitalizeDoors() {
		foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door")) // Add a door to the list of doors
			doors.Add(Tile.TilePosition(door.transform.position), door);

		foreach (GameObject openDoor in OpenedDoors) {
			if (openDoor != null) // Open the door
				OpenDoor(Tile.TilePosition(openDoor.transform.position));
		}
	}

	/**
	 * Turn on the light objects
	 */
	void TurnOnLighting() {
		Color color;
		Color.TryParseHexString("161616FF", out color);
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
		RemainingTurnsText.text = "Rounds Remaining: " + RoundsLeftUntilLose;
		if (!loaded) // Wait for other players to join first
			if (Application.loadedLevelName != "Tutorial") // This is the main level. Wait
				waitForPlayers();
			else // Tutorial
				loaded = true;
		if (validTurn) {
			if (playersLeft <= 0) // No more active players
				validTurn = false;
			return;
		}

		playersLeft = PhotonNetwork.playerList.Length; // Need to check photons list of players, not models

		/* Initialize player stats - AP and apply player effects */
		try { 
			Player p = Player.MyPlayer.GetComponent<Player>();
			p.InitializeStats();
			p.ApplyTurnEffects();
			p.ReduceItemCoolDowns();
			p.ReduceAbilityTurns();
			p.SetInActivity(false);
		} catch (MissingReferenceException e) { // Handle Security System Kill state
			Debug.LogWarning("Missing ref exception: " + e.Message);
			Application.LoadLevel("GameOver");
		}

		GiveSecondary();
		RoundsLeftUntilLose -= roundsLost;
		RemainingTurnsText.text = "Rounds Remaining: " + RoundsLeftUntilLose;
		if (RoundsLeftUntilLose <= 0) // Lost the game
			GetComponent<PhotonView>().RPC("LoseGame", PhotonTargets.All, null);

		validTurn = true;
	}

	/*
	 * Has a small chance to give a secondary out
	 */
	void GiveSecondary() {
		GameObject secondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject; // Secondaries
		if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() == Classes.BETRAYER) {
			// Alien
			if (secondaries.transform.childCount < 8
			    	&& Random.Range(0, 10) >= 7) // Pick random alien objective
				SecondaryObjective.PickNewAlienObjective();
		} else {
			// Human
			if (secondaries.transform.childCount < 3
			    	&& Random.Range(0, 10) >= 9) // Pick random human objective
				SecondaryObjective.PickNewHumanObjective();
		}
	}

	/**
	 * Return if a player was killed
	 * 
	 * Returns
	 * - true if a player was killed (using security system). false otherwise
	 */
	public bool HasPlayerDied() {
		return playersKilled;
	}

	/**
	 * Return the number of players that escaped
	 * 
	 * Returns
	 * - the number of players that escaped (3 players)
	 */
	public int HasPlayerEscaped() {
		return playersEscaped;
	}

	/**
	 * RPC call for recording if a player has died
	 */
	[PunRPC]
	public void PlayerDied() {
		Debug.LogError("A player has died");
		Debug.LogWarning("Still connected?: " + PhotonNetwork.connected);
		playersKilled = true;
	}

	/**
	 * RPC call for recording if a player has escaped
	 */
	[PunRPC]
	public void PlayerEscaped() {
		Debug.LogError("A player has escaped");
		Debug.LogWarning("Still connected?: " + PhotonNetwork.connected);
		playersEscaped++;
	}

	/**
	 * Increase number of turns remaining
	 * 
	 * Arguments
	 * - int addedTurns - The turns added
	 */
	[PunRPC]
	public void AddTurns(int addedTurns) {
		RoundsLeftUntilLose += addedTurns;
	}

	/**
	 * RPC call for losing the game
	 */
	[PunRPC]
	void LoseGame() {
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) { // Handle alien win
			if (playersEscaped >= 3) // Alien Lost
				Application.LoadLevel("AlienLoseScreen");
			else if (playersEscaped >= 1 && playersEscaped < 3)// Alien Partial Win
				Application.LoadLevel("AlienPartialWinScreen");
			else if (playersEscaped <= 0) // Alien Win
				Application.LoadLevel("AlienWinScreen");
		} else // Humans lose
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
		Debug.LogError("Setting active player. New number of players left: " + playersLeft);
	}

	/**
	 * Record that a player is no longer active
	 */
	[PunRPC]
	public void SetInactivePlayer() {
		playersLeft--;
		Debug.LogError("Setting inactive player. New number of players left: " + playersLeft);
	}

	/**
	 * Opens the door at the given position. This means the door will start its animation and will
	 * no longer block that tile.
	 */
	public void OpenDoor(Tile position) {
		GameObject door; // The door to open
		if (!doors.TryGetValue(position, out door)) {
			Debug.LogWarning("You tried to open a door that doesn't exist on that tile. BEN");
			return;
		}

		try {
			door.GetComponent<Animator>().enabled = true;
		} catch (MissingComponentException) {
			Debug.LogWarning("The door has no animator");
		}
		movController.UnblockTile(position);

		UnlockedTiles.Add(position);
	}

	/**
	 * Get the movement controller of the current player
	 */
	public MovementController GetPlayerControllers(){
		return movController;
	}

	/*
	 * Increases the number of rounds lost after a turn.
	 * 
	 * Arguments
	 * - int newRoundsLost - The additional rounds we've lost
	 */
	[PunRPC]
	public void IncreaseRoundsLost(int newRoundsLost) {
		roundsLost += newRoundsLost;
	}

	/*
	 * Decreases the number of rounds lost after a turn.
	 * 
	 * Arguments
	 * - int newRoundsLost - The number of rounds we gain per ending of a turn
	 */
	[PunRPC]
	public void DecreaseRoundsLost(int newRoundsLost) {
		roundsLost -= newRoundsLost;
	}

	/**
	 * Get the number of players left in the game
	 */
	public int GetPlayersLeft() {
		return playersLeft;
	}

	/**
	 * RPC call for spawning the boarding party
	 * 
	 * Arguments
	 * - Vector3 spawn - The position of the spawn point
	 */
	[PunRPC]
	public void SpawnBoardingParty(Vector3 spawn) {
		// The secondary objectives of the current player
		GameObject secondaries 
			= Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;

		BoardingSpawn = spawn;
		secondaries.AddComponent<BoardingParty>();
	}

	/**
	 * Set event card
	 * 
	 * Arguments
	 * - string message - The message to give
	 * - string title - The title of the event card
	 * - string image - The path to the event card image
	 */
	[PunRPC]
	public void EventCardMessage(string message, string title, string image) {
		GenericCard gc = gameObject.AddComponent<GenericCard>();
		gc.ChangeContent(image, title, message);
		gc.CreateCard();
	}

	/**
	 * Increase the number of rounds left in the game
	 * 
	 * Arguments
	 * - int extraRounds - The additional rounds
	 */
	[PunRPC]
	public void IncreaseRounds(int extraRounds) {
		RoundsLeftUntilLose += extraRounds;
	}
}
