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
		PhotonNetwork.ConnectUsingSettings("e");
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
		SpawnMyPlayer();
	}

	void SpawnMyPlayer() {
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
		switch (myPlayer.GetComponent<Player>().GetPlayerClass()) {
		case "Engineer Class":
			pClass = Classes.ENGINEER;
			break;
		case "Marine Class":
			pClass = Classes.MARINE;
			break;
		case "Technician Class":
			pClass = Classes.TECHNICIAN;
			break;
		case "Scout Class":
			pClass = Classes.SCOUT;
			break;
		default:
			pClass = Classes.MARINE;
			Debug.LogWarning("The player class isn't what it's expected to be: " + 
			    	myPlayer.GetComponent<Player>().GetPlayerClass());
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