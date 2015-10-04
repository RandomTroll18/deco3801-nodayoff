using UnityEngine;
using System.Collections;

public class ScoutTrapScript : Trap {
	
	/**
	 * Activate function
	 * 
	 * Arguments
	 * - Player player - The player that activated the trap
	 */
	public override void Activated(Player p) {
		if (GetComponent<PhotonView>().isMine) 
			Debug.Log("I'm sorry master. I won't activate");
		else {
			Debug.Log("Activated scout trap");
			p.GetComponent<PhotonView>().RPC("Stun", PhotonTargets.All, 3);
			GetComponent<PhotonView>().RPC("Destroy", PhotonTargets.All, null);
		}
	}

	/*
	 * Only the master client or the client that spawned gameObject can call
	 * PhotonNetwork.Destroy(gameObject). This method just tells that client to do so.
	 */
	[PunRPC]
	void Destroy() {
		if (GetComponent<PhotonView>().isMine) {
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
