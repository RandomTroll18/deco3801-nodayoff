using UnityEngine;
using System.Collections;

public class NetworkingManager : Photon.PunBehaviour {
	SpawnPoint[] spawnPoints;

	/*
	 * This is like a main() function for our level. It's possible some other classes could use
	 * Start() if they don't require networking or the main player in their Start or Update.
	 */
	void Start () {
		spawnPoints = Object.FindObjectsOfType<SpawnPoint>();
		Connect();
//		PhotonNetwork.logLevel = PhotonLogLevel.Full;
	}

	void Connect() {
		if (PhotonNetwork.connected) // Disconnect if we are already connected
			PhotonNetwork.Disconnect();

		Debug.Log("Connect");
		PhotonNetwork.autoJoinLobby = true;
		PhotonNetwork.ConnectUsingSettings("f");
	}

	public override void OnJoinedLobby() {
		Debug.Log("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		RoomOptions roomOptions = new RoomOptions() { 
			maxPlayers = 4 
		}; // Options for the room we are creating
		Debug.Log("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
	}

	public override void OnCreatedRoom() {
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedRoom() {
		Debug.Log("OnJoinedRoom. Player ID: " + PhotonNetwork.player.ID);
		/* Test. Randomly assign player to be an alien */
		if (Random.Range(1, 5) == 4) // Player must be an alien
			PhotonNetwork.player.SetTeam(PunTeams.Team.red);
		else // Player must be a human
			PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
		SpawnMyPlayer();
	}

	void SpawnMyPlayer() {
		AlienClass alienClassContainer; // Container for the alien class
		Player playerScript; // The player script
		SpawnPoint spawn = spawnPoints[0]; // TODO: pick spawn point based on class
		GameObject myPlayer = PhotonNetwork.Instantiate(
			"Player", 
			spawn.transform.position, 
			spawn.transform.rotation, 
			0
			);
		Player.MyPlayer = myPlayer;
		GameObject gm =  Object.FindObjectOfType<GameManager>().gameObject;
		gm.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, null);
		Object.FindObjectOfType<GameManager>().StartMe();

		Classes pClass;

		playerScript = myPlayer.GetComponent<Player>();
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
			}
		}
		myPlayer.transform.position = spawn.transform.position;

		myPlayer.GetComponent<Player>().GenerateStunGun();
	}

	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}