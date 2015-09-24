using UnityEngine;
using System.Collections;
public class TestMatchMaker : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	public override void OnConnectedToPhoton()
	{
		Debug.Log("Connected to Photon");
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to my master");
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedRoom()
	{
		GameObject cube = PhotonNetwork.Instantiate("TestPrefabs/CubePrefab", Vector3.zero, Quaternion.identity, 0);
	}
	
	public override void OnJoinedLobby()
	{
		Debug.Log("Lobby joined");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log("Can't join random room");
		PhotonNetwork.CreateRoom(null);
	}
}
