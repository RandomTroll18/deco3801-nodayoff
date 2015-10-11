using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {

	/**
	 * Disconnect this client
	 */
	public void DisconnectClient() {
		GameManager gameManager = Object.FindObjectOfType<GameManager>(); // The game manager

		if (gameManager != null)
			gameManager.gameObject.GetComponent<PhotonView>().RPC("SetInactivePlayer", PhotonTargets.All, null);

		if (Player.MyPlayer != null) // Destroy the player's model
			PhotonNetwork.Destroy(Player.MyPlayer);

		if (PhotonNetwork.connected) // Only disconnect if we were connected
			PhotonNetwork.Disconnect();

	}
}
