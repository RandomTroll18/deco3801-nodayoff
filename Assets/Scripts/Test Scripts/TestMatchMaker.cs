using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TestMatchMaker : Photon.PunBehaviour {

	public static string PlayerName; // The name of the player
	public static string RoomName; // The room that the player wants to join
	public Text RoomNameLabel; // The label for the room name
	public Text PlayerCountLabel; // The label for the player count
	public Text[] PlayerNamesLabel; // The labels for each player's name
	int alienIndex; // The index of the alien

	// Use this for initialization
	void Start() {
		PhotonNetwork.ConnectUsingSettings("0.1");
		RoomNameLabel.text = "Room Name: " + RoomName;
		PhotonNetwork.playerName = PlayerName;
	}

	/**
	 * RPC call for loading the needed level
	 */
	[PunRPC]
	public void LoadClassSelect() {
		Application.LoadLevel("ClassSelect");
	}

	/**
	 * RPC call for setting local player's team
	 */
	[PunRPC]
	public void SetPlayerTeam(byte newTeam) {
		Debug.Log("Setting local player team");
		PhotonNetwork.player.SetTeam((PunTeams.Team)newTeam);
	}

	/**
	 * Check if all players have been assigned a team
	 * 
	 * Returns
	 * - true if all players have been assigned a team.
	 * - false otherwise
	 */
	bool playersAssigned() {
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam() == PunTeams.Team.none)
				return false;
		}
		return true;
	}

	void Update() {
		if (!PhotonNetwork.connectedAndReady)
			return;
		if (PhotonNetwork.isMasterClient)
			Debug.Log("Alien index: " + alienIndex);
		PhotonPlayer currentPlayer; // The current player being looked at
		PlayerCountLabel.text = "Number of Players: " + PhotonNetwork.playerList.Length.ToString();
		if (PhotonNetwork.playerList.Length == 4 && playersAssigned()) { // We have enough players
			Debug.Log("We have enough players!");
			gameObject.SetActive(false);
			if (string.IsNullOrEmpty(MainMenuScript.LevelToLoad)) {
				Debug.Log("There is a level to load");
				MainMenuScript.LevelToLoad = "Main Level";
			}
			LoadClassSelect();
		}
		for (int i = 0; i < PlayerNamesLabel.Length; ++i) {
			if (i >= PhotonNetwork.playerList.Length) {
				PlayerNamesLabel[i].text = "N/A";
				continue;
			}
			currentPlayer = PhotonNetwork.playerList[i];
			PlayerNamesLabel[i].text = currentPlayer.name;
			if (PhotonNetwork.player == currentPlayer)
				PlayerNamesLabel[i].text += " (You)";

			if (PhotonNetwork.isMasterClient) { // Need to assign teams
				Debug.Log("Master client. Assign these teams");
				assignTeam(currentPlayer, i);
			} else 
				Debug.Log("Not master client");
			if (currentPlayer.GetTeam() == PunTeams.Team.blue) {
				PlayerNamesLabel[i].text += " (Human)";
				Debug.Log("Player: " + currentPlayer.name + " is a human");
			}
			else if (currentPlayer.GetTeam() == PunTeams.Team.red) {
				PlayerNamesLabel[i].text += " (Alien)";
				Debug.Log("Player: " + currentPlayer.name + " is an alien");
			}
			else {
				PlayerNamesLabel[i].text += " (Unknown Team)";
				Debug.Log("Player: " + currentPlayer.name + " is in an unknown team");
			}
				
		}
	}

	/**
	 * Assign teams to everyone
	 */
	void assignTeams() {
		PhotonPlayer currentPlayer; // The current player
		for (int i = 0; i < PlayerNamesLabel.Length; ++i) {
			if (i >= PhotonNetwork.playerList.Length)
				break;
			currentPlayer = PhotonNetwork.playerList[i];
			assignTeam(PhotonNetwork.playerList[i], i);
			if (i == alienIndex && currentPlayer.GetTeam() != PunTeams.Team.red) { // Not assigned yet
				Debug.LogError("Alien not assigned properly yet");
				i--;
			} else if (i != alienIndex && currentPlayer.GetTeam() != PunTeams.Team.blue) { // Not assigned yet
				Debug.LogError("Human not assigned properly yet");
				i--;
			} else // Assigned. Tell player to load level
				GetComponent<PhotonView>().RPC("LoadClassSelect", currentPlayer, null);
		}
	}

	/**
	 * Assign team for this single player
	 * 
	 * Arguments
	 * - PhotonPlayer currentPlayer - The current player to set team
	 * - int index - The index of this player
	 */
	void assignTeam(PhotonPlayer currentPlayer, int index) {
		if (index == alienIndex && currentPlayer.GetTeam() != PunTeams.Team.red) {
			Debug.Log("Setting " + currentPlayer.name + " to be the alien");
			if (currentPlayer == PhotonNetwork.player)
				currentPlayer.SetTeam(PunTeams.Team.red);
			else
				GetComponent<PhotonView>().RPC("SetPlayerTeam", currentPlayer, new object[] {PunTeams.Team.red});
		} else if (index != alienIndex && currentPlayer.GetTeam() != PunTeams.Team.blue) {
			Debug.Log("Setting " + currentPlayer.name + " to be a human");
			if (currentPlayer == PhotonNetwork.player)
				currentPlayer.SetTeam(PunTeams.Team.blue);
			else
				GetComponent<PhotonView>().RPC("SetPlayerTeam", currentPlayer, new object[] {PunTeams.Team.blue});
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
		if (PhotonNetwork.isMasterClient) { // Calculate alien index
			alienIndex = Random.Range(0, 4);
			Debug.Log("Alien is at index: " + alienIndex);
		}
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

	public override void OnCreatedRoom()
	{
		// This player is the master client
	}
}
