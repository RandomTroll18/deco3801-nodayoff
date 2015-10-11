using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TestMatchMaker : Photon.PunBehaviour {

	public static string PlayerName; // The name of the player
	public static string RoomName; // The room that the player wants to join
	public Text RoomNameLabel; // The label for the room name
	public Text PlayerCountLabel; // The label for the player count
	public Text[] PlayerNamesLabel; // The labels for each player's name
	static int randomRoomID = 0; // The id for the random room

	// Use this for initialization
	void Start() {
		PhotonNetwork.ConnectUsingSettings("0.1");
		RoomNameLabel.text = "Room Name: " + RoomName;
		PhotonNetwork.playerName = PlayerName;
	}

	void Update() {
		PlayerCountLabel.text = "Number of Players: " + PhotonNetwork.playerList.Length.ToString();
		if (PhotonNetwork.playerList.Length == 4) { // We have enough players
			Debug.LogError("We have enough players!");
			gameObject.SetActive(false);
			if (!string.IsNullOrEmpty(MainMenuScript.LevelToLoad))
				MainMenuScript.LevelToLoad = "TestScene2";
			Application.LoadLevel("ClassSelect");
		}
		for (int i = 0; i < PhotonNetwork.playerList.Length; ++i) {
			PlayerNamesLabel[i].text = PhotonNetwork.playerList[i].name;
			if (PhotonNetwork.player == PhotonNetwork.playerList[i])
				PlayerNamesLabel[i].text += " (You)";
		}
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
		RoomNameLabel.text = "Room Name: " + PhotonNetwork.room.name;
		PhotonNetwork.playerName = PlayerName;
	}
	
	public override void OnJoinedLobby()
	{
		Debug.Log("Lobby joined");
		if (string.IsNullOrEmpty(RoomName)) 
			PhotonNetwork.JoinRandomRoom();
		else 
			PhotonNetwork.JoinRoom(RoomName);

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
