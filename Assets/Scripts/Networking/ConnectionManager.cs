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
			PhotonNetwork.Disconnect();
		}
	}

	/**
	 * Callback for when a player disconnects
	 */
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		GameManagerObject = GameObject.FindGameObjectWithTag("GameController");

		base.OnPhotonPlayerDisconnected(otherPlayer);

		if (!PhotonNetwork.player.isMasterClient) // Not master client, don't do anything
			return;

		/* Destroy the model belonging to the disconnected player */
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Debug.Log("Finding disconnected player model");
			Debug.Log("Player object id: " + player.GetComponent<PhotonView>().ownerId);
			if (player.GetComponent<PhotonView>().ownerId == otherPlayer.ID) { // Found the player
				Debug.Log("Found player model");
				if (Player.MyPlayer != null && !player.GetComponent<Player>().IsPlayerNoLongerActive()
						&& GameManagerObject != null) 
					GameManagerObject.GetComponent<PhotonView>().RPC("SetInactivePlayer", PhotonTargets.All, null);
				PhotonNetwork.Destroy(player);
				return;
			}
		}
	}
}
