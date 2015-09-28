using UnityEngine;
using System.Collections;
public class TestMatchMaker : Photon.PunBehaviour {

	public static string PlayerName; // The name of the player
	public static string RoomName; // The room that the player wants to join

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label("Player Name: " + PlayerName);
		GUILayout.Label("Room Name: " + RoomName);
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
		Debug.Log("Joined room: " + PhotonNetwork.room.name);
		GameObject cube = PhotonNetwork.Instantiate("TestPrefabs/CubePrefab", Vector3.zero, Quaternion.identity, 0);
		GameObject test = Instantiate<GameObject>(GameObject.CreatePrimitive(PrimitiveType.Capsule));
		test.GetComponent<Transform>().position = Vector3.zero;
	}
	
	public override void OnJoinedLobby()
	{
		Debug.Log("Lobby joined");
		if (PlayerName.Length == 0 || RoomName.Length == 0) PhotonNetwork.JoinRandomRoom();
		else PhotonNetwork.JoinRoom(RoomName);

	}

	void OnPhotonJoinRoomFailed() {
		Debug.Log("Can't join given room");
		PhotonNetwork.CreateRoom(RoomName);
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log("Can't join random room");
		PhotonNetwork.CreateRoom(null);
	}
}
