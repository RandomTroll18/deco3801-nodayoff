using UnityEngine;
using System.Collections;

public class NetworkingManager : Photon.PunBehaviour {
	SpawnPoint[] spawnPoints;

	void Start () {
		spawnPoints = Object.FindObjectsOfType<SpawnPoint>();
		Connect();
//		PhotonNetwork.logLevel = PhotonLogLevel.Full;
	}

	void Connect() {
		Debug.Log("Connect");
		PhotonNetwork.autoJoinLobby = true;
		PhotonNetwork.ConnectUsingSettings("1.0");
	}

	public override void OnJoinedLobby() {
		Debug.Log("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom(null);
	}

	public override void OnCreatedRoom() {
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedRoom() {
		Debug.Log("OnJoinedRoom");
		SpawnMyPlayer();
	}

	void SpawnMyPlayer() {
		SpawnPoint spawn = spawnPoints[0]; // TODO: pick spawn point based on class
		PhotonNetwork.Instantiate("Player", spawn.transform.position, spawn.transform.rotation, 0);
		// TODO: spawn light with player
	}

	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}