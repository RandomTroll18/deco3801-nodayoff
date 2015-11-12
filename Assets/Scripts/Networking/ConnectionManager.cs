using UnityEngine;
using System.Collections;

public class ConnectionManager : Photon.PunBehaviour {

	public GameObject GameManagerObject; // The game manager object

	/**
	 * Disconnect this client
	 */
	public void DisconnectClient() {
		if (PhotonNetwork.connected) { // Only disconnect if we were connected
			Debug.Log("Player is connected. Need to disconnect");

			/* Reset custom properties */
			PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
				{"classselected", null}
			});
			// Set custom properties for ourselves
			PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
				{"waiting", null}
			});
			PhotonNetwork.Disconnect();
		}
	}

	/**
	 * Callback for when a player disconnects
	 */
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
		GameManagerObject = GameObject.FindGameObjectWithTag("GameController"); // The game manager game object
		int numberOfEscapedPlayers; // Number of escaped players

		base.OnPhotonPlayerDisconnected(otherPlayer); // Execute base functions first

		/* Destroy the model belonging to the disconnected player */
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.GetComponent<PhotonView>().ownerId == otherPlayer.ID) { // Found the player
				if (Player.MyPlayer != null && !player.GetComponent<Player>().IsPlayerNoLongerActive()
						&& GameManagerObject != null)  { // Game manager exists
					Debug.LogError("Update Game Manager");
					GameManagerObject.GetComponent<GameManager>().SetInactivePlayer();
					GameManagerObject.GetComponent<GameManager>().DestroyDisconnectedPlayerModel(
							player.GetComponent<PhotonView>().ownerId);
				}
				if (PhotonNetwork.playerList.Length <= 1) { // All by yourself
					Object.FindObjectOfType<ConnectionManager>().DisconnectClient();
					if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue) { // Human
						if (GameManagerObject.GetComponent<GameManager>().HasPlayerDied()) // A human died
							Application.LoadLevel("PartialWinScreen");
						else // Everyone who played and didn't disconnect is safe
							Application.LoadLevel("WinScreen");
					} else { // Alien
						numberOfEscapedPlayers = GameManagerObject.GetComponent<GameManager>().HasPlayerEscaped();
						if (numberOfEscapedPlayers >= 3) // Alien Lost
							Application.LoadLevel("AlienLoseScreen");
						else if (numberOfEscapedPlayers >= 1 && numberOfEscapedPlayers < 3)// Alien Partial Win
							Application.LoadLevel("AlienPartialWinScreen");
						else if (numberOfEscapedPlayers <= 0) // Alien Win
							Application.LoadLevel("AlienWinScreen");
					}
				}
				return;
			}
		}
	}
}
