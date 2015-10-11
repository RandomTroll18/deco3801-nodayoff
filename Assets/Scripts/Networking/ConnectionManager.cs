using UnityEngine;
using System.Collections;

public class ConnectionManager : Photon.PunBehaviour {

	public GameObject GameManagerObject; // The game manager object

	/**
	 * Disconnect this client
	 */
	public void DisconnectClient() {
		GameObject eventSystem = GameObject.Find("EventSystem"); // The event system
		MainMenuScript navigationScript; // The navigation script

		if (PhotonNetwork.connected) { // Only disconnect if we were connected
			Debug.Log("Player is connected. Need to disconnect");
			PhotonNetwork.Disconnect();
		}
	}

	/**
	 * Callback for when a player disconnects
	 */
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		base.OnPhotonPlayerDisconnected(otherPlayer);
		/* Don't use RPC call because this is called on all clients */
		GameManagerObject.GetComponent<GameManager>().SetInactivePlayer();
		Debug.Log("Other player's id: " + otherPlayer.ID);
		/* Destroy the model belonging to the disconnected player */
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Debug.Log("Finding disconnected player model");
			Debug.Log("Player object id: " + player.GetComponent<PhotonView>().ownerId);
			if (player.GetComponent<PhotonView>().ownerId == otherPlayer.ID) { // Found the player
				Debug.Log("Found player model");
				PhotonNetwork.Destroy(player);
				Destroy(player);
				return;
			}
		}
	}
}
