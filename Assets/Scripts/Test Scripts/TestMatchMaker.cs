using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TestMatchMaker : Photon.PunBehaviour {

	public static string PlayerName; // The name of the player
	public static string RoomName; // The room that the player wants to join
	public Text RoomNameLabel; // The label for the room name
	public Text PlayerCountLabel; // The label for the player count
	public Text[] PlayerNamesLabel; // The labels for each player's name
	public GameObject ClassPickWaitCanvas; // Canvas to display when we are waiting to pick our class
	public GameObject ClassPickWaitText; // The text displaying how many players have picked their class
	int alienIndex; // The index of the alien
	int readyPlayers; // Number of players who have already picked their class
	bool selectingClasses; // Selecting classes
	
	void Start() {
		PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
				{"waiting", false}
		}); // Player is not waiting for other players to pick their class
		PhotonNetwork.ConnectUsingSettings("matchmaker.v1");
		RoomNameLabel.text = "Room Name: " + RoomName; // Display room name
		PhotonNetwork.playerName = PlayerName; // Set player's name
		selectingClasses = false;
		readyPlayers = 0;
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
	 * Check if all players have been assigned a team and if 
	 * they have been assigned correctly
	 * 
	 * Returns
	 * - true if all players have been assigned a team.
	 * - false otherwise
	 */
	bool playersAssigned() {
		bool alienAssigned = false; // Record if the alien was already assigned
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam() == PunTeams.Team.none)
				return false; // A player hasn't been assigned
			else if (player.GetTeam() == PunTeams.Team.red && !alienAssigned)
				alienAssigned = true; // One alien has been already assigned
			else if (player.GetTeam() == PunTeams.Team.red && alienAssigned)
				return false; // Alien assigned twice. In the middle of changing assignments
		}
		return true;
	}

	/**
	 * RPC call for master client to send a player to the class select screen
	 */
	[PunRPC]
	public void MatchMakerSendPlayerToClassSelect() {
		int randomIndex = Random.Range(0, PhotonNetwork.otherPlayers.Length);

		readyPlayers++;
		if (!PhotonNetwork.player.isMasterClient || readyPlayers >= PhotonNetwork.playerList.Length)
			return;

		/* Send a player in match-maker to the class select screen */
		while ((bool)PhotonNetwork.otherPlayers[randomIndex].customProperties["waiting"])
			randomIndex = Random.Range(0, PhotonNetwork.otherPlayers.Length);

		GetComponent<PhotonView>().RPC("LoadClassSelect", PhotonNetwork.otherPlayers[randomIndex], null);
	}

	/**
	 * Queue for selecting classes
	 */
	public void SelectingClassQueueing() {
		int ready = 0; // Number of players who've picked their classes

		/* Display text indicating how many players have picked their classes */
		foreach (PhotonPlayer currentPlayer in PhotonNetwork.otherPlayers) {
			if ((bool)currentPlayer.customProperties["waiting"])
				ready++;
		}
		ClassPickWaitText.GetComponent<Text>().text = 
			"Waiting To Be Able To Pick Class. " + StringMethodsScript.NEWLINE
			+ ready + " players have already picked their class";
		if (!PhotonNetwork.isMasterClient) // Not master client. Don't do anything
			return;
		if (readyPlayers >= PhotonNetwork.playerList.Length) // We're done here
			LoadClassSelect();
	}

	void Update() {
		PhotonPlayer currentPlayer; // The current player being looked at

		if (!PhotonNetwork.connectedAndReady) // Don't do anything. Not yet connected
			return;

		if (selectingClasses) { // We are now selecting classes
			SelectingClassQueueing();
			return;
		}

		if (PhotonNetwork.isMasterClient) // Print out alien index if master client
			Debug.Log("Alien index: " + alienIndex);

		// Update counter
		PlayerCountLabel.text = "Number of Players: " + PhotonNetwork.playerList.Length.ToString();

		if (PhotonNetwork.playerList.Length == 4 && playersAssigned()) { // Enough players and assigned correctly
			PhotonNetwork.room.open = false; // This room is now closed
			if (string.IsNullOrEmpty(MainMenuScript.LevelToLoad)) {
				Debug.Log("There is a level to load");
				MainMenuScript.LevelToLoad = "Main Level";
			}
			selectingClasses = true; // Initiate selecting class queue
			ClassPickWaitCanvas.SetActive(true);
			MatchMakerSendPlayerToClassSelect();
		}

		/* Set text and assign teams for different players */
		for (int i = 0; i < PlayerNamesLabel.Length; ++i) {
			if (i >= PhotonNetwork.playerList.Length) { // No player for this label
				PlayerNamesLabel[i].text = "N/A";
				continue;
			}
			currentPlayer = PhotonNetwork.playerList[i];
			PlayerNamesLabel[i].text = currentPlayer.name;
			if (PhotonNetwork.player == currentPlayer) // We are at our label
				PlayerNamesLabel[i].text += " (You)";

			if (PhotonNetwork.isMasterClient) { // Need to assign teams
				Debug.Log("Master client. Assign these teams");
				assignTeam(currentPlayer, i);
			} else 
				Debug.Log("Not master client");

			if (currentPlayer.GetTeam() == PunTeams.Team.none) { // Unknown team. Not yet assigned
				PlayerNamesLabel[i].text += " (Unknown Team)";
				Debug.Log("Player: " + currentPlayer.name + " is in an unknown team");
			}
				
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
		if (index == alienIndex && currentPlayer.GetTeam() != PunTeams.Team.red) { // Player needs to be an alien
			Debug.Log("Setting " + currentPlayer.name + " to be the alien");
			if (currentPlayer == PhotonNetwork.player) // Just set our own team
				currentPlayer.SetTeam(PunTeams.Team.red);
			else // Need RPC call
				GetComponent<PhotonView>().RPC("SetPlayerTeam", currentPlayer, new object[] {PunTeams.Team.red});
		} else if (index != alienIndex && currentPlayer.GetTeam() != PunTeams.Team.blue) {
			Debug.Log("Setting " + currentPlayer.name + " to be a human");
			if (currentPlayer == PhotonNetwork.player) // Just set our own team
				currentPlayer.SetTeam(PunTeams.Team.blue);
			else // Player needs to be an alien
				GetComponent<PhotonView>().RPC("SetPlayerTeam", currentPlayer, new object[] {PunTeams.Team.blue});
		}
	}

	/* Callbacks for Photon Plugin */
	
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
		RoomNameLabel.text = "Room Name: " + PhotonNetwork.room.name;
		PhotonNetwork.playerName = PlayerName;
		if (PhotonNetwork.isMasterClient) // Calculate alien index
			alienIndex = Random.Range(0, 4);
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

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		base.OnPhotonPlayerDisconnected(otherPlayer);

		if (PhotonNetwork.isMasterClient)
			MatchMakerSendPlayerToClassSelect();
	}
}
