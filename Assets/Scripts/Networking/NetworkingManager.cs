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
		GameObject myPlayer = PhotonNetwork.Instantiate(
			"Player", 
			spawn.transform.position, 
			spawn.transform.rotation, 
			0
			);
		Player.MyPlayer = myPlayer;
		// TODO: spawn light with player / turn light on
		GameObject gm =  Object.FindObjectOfType<GameManager>().gameObject;
		gm.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, null);
		Object.FindObjectOfType<GameManager>().StartMe();
		myPlayer.GetComponent<MovementController>().enabled = true;
		myPlayer.GetComponentInChildren<CameraController>().enabled = true;
	}

	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}