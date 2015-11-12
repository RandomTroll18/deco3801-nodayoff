using UnityEngine;
using System.Collections;

public class NetworkingManager : Photon.PunBehaviour {
	
	SpawnPoint[] spawnPoints; // The list of spawn points

	/*
	 * This is like a main() function for our level. It's possible some other classes could use
	 * Start() if they don't require networking or the main player in their Start or Update.
	 */
	void Start () {
		spawnPoints = Object.FindObjectsOfType<SpawnPoint>();
		if (PhotonNetwork.connected)
			SpawnMyPlayer();
		else
			Connect();
	}

	/**
	 * RPC call for requesting initialization of player
	 * 
	 * Arguments
	 * - PhotonPlayer requestingPlayer - The player requesting
	 */
	[PunRPC]
	public void RequestInitializingOfPlayer(PhotonPlayer requestingPlayer) {
		AlienClass alienClassContainer; // The alien class container
		Classes pClass; // Class of this player
		Player playerScript; // Our player's script
		SpawnPoint spawn = spawnPoints[0]; // The spawn point of our player
		GameManager gameManager = Object.FindObjectOfType<GameManager>(); // Our game manager

		Debug.Log("Received RPC call to re-initialize player for " + requestingPlayer.name);

		if (Player.MyPlayer != null) { // Only re-initialize if we haven't initialized our character yet
			playerScript = Player.MyPlayer.GetComponent<Player>();
			switch (playerScript.GetPlayerClassObject().GetClassTypeEnum()) {
			case Classes.BETRAYER: // Alien
				alienClassContainer = (AlienClass)playerScript.GetPlayerClassObject();
				pClass = alienClassContainer.GetHumanClassType();
				break;
			default: // Human class
				pClass = playerScript.GetPlayerClassObject().GetClassTypeEnum();
				if (pClass == Classes.BETRAYER) // Something horrible has gone wrong
					throw new UnityException("How did you get past that case up there!");
				break;
			}
			
			foreach (SpawnPoint thisPoint in spawnPoints) {
				if (thisPoint.Class == pClass) {
					spawn = thisPoint;
					break;
				}
			}

			gameManager.gameObject.GetComponent<PhotonView>().RPC(
					"InstantiateResponse",
					requestingPlayer,
					new object[] {spawn.transform.position, spawn.transform.rotation, 
					Player.MyPlayer.GetComponent<PhotonView>().owner, 
					Player.MyPlayer.GetComponent<PhotonView>().owner.ID,
					Player.MyPlayer.GetComponent<PhotonView>().viewID}
			);
		}
	}

	/**
	 * Start connecting
	 */
	void Connect() {
		Debug.Log("Connect");
		PhotonNetwork.autoJoinLobby = true;
		PhotonNetwork.ConnectUsingSettings("eeff");
	}

	public override void OnJoinedLobby() {
		Debug.Log("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		RoomOptions roomOptions; // Options for the room we are creating
		if (Application.loadedLevelName.Equals("Tutorial")) { // Tutorial level
			roomOptions = new RoomOptions() { maxPlayers = 1 };
			Debug.LogWarning("tutorial level");
		} else // Main Level
			roomOptions = new RoomOptions() { maxPlayers = 4 };
		Debug.Log("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
	}

	public override void OnCreatedRoom() {
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedRoom() {
		Debug.Log("OnJoinedRoom. Player ID: " + PhotonNetwork.player.ID);
		if (Application.loadedLevelName.Equals("Tutorial")) // Tutorial level
			PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
		else { // Main Level
			if (Random.Range(1, 5) == 4) // Player must be an alien
				PhotonNetwork.player.SetTeam(PunTeams.Team.red);
			else // Player must be a human
				PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
		}
		SpawnMyPlayer();
	}

	/**
	 * Spawn the player and initialize all required objects
	 */
	void SpawnMyPlayer() {
		AlienClass alienClassContainer; // Container for the alien class
		Player playerScript; // The player script
		SpawnPoint spawn = spawnPoints[0]; // The spawn point for this player
		GameObject myPlayer; // The reference to the player's object
		GameObject gm; // The game manager
		Classes pClass; // The class of the player

		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) { // Alien
			myPlayer = PhotonNetwork.Instantiate(
				"AlienPlayer", 
				spawn.transform.position, 
				spawn.transform.rotation, 
				0
			);
		} else { // Human
			myPlayer = PhotonNetwork.Instantiate(
				"Player", 
				spawn.transform.position, 
				spawn.transform.rotation, 
				0
				);
		}
		/* Enable player components */
		Player.MyPlayer = myPlayer;
		myPlayer.GetComponentInChildren<AudioListener>().enabled = true;
		myPlayer.GetComponentInChildren<Animator>().enabled = true;
		myPlayer.GetComponentInChildren<Light>().enabled = true;
		myPlayer.GetComponentInChildren<Camera>().enabled = true;
		myPlayer.GetComponentInChildren<CameraController>().enabled = true;

		/* Initialize Game Manger */
		gm =  Object.FindObjectOfType<GameManager>().gameObject;
		gm.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, null);
		Object.FindObjectOfType<GameManager>().StartMe();

		/* Initialize player script and pick spawn point */
		playerScript = myPlayer.GetComponent<Player>();
		playerScript.enabled = true;
		switch (playerScript.GetPlayerClassObject().GetClassTypeEnum()) {
		case Classes.BETRAYER: // Alien
			alienClassContainer = (AlienClass)playerScript.GetPlayerClassObject();
			pClass = alienClassContainer.GetHumanClassType();
			break;
		default: // Human class
			pClass = playerScript.GetPlayerClassObject().GetClassTypeEnum();
			if (pClass == Classes.BETRAYER) // Something horrible has gone wrong
				throw new UnityException("How did you get past that case up there!");
			break;
		}

		foreach (SpawnPoint thisPoint in spawnPoints) {
			if (thisPoint.Class == pClass) {
				spawn = thisPoint;
				break;
			}
		}
		myPlayer.transform.position = spawn.transform.position;
		myPlayer.GetComponent<Player>().GenerateStunGun();
		myPlayer.GetComponent<Player>().InstantiateStartingItems();
	}

	/**
	 * Handle automatic GUI
	 */
	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}