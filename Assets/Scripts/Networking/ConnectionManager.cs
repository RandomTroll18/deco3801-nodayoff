using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {

	/**
	 * Disconnect this client
	 */
	public void DisconnectClient() {
		if (PhotonNetwork.connected)
			PhotonNetwork.Disconnect();
	}
}
