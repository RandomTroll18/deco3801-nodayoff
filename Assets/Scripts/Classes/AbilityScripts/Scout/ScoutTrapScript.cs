using UnityEngine;
using System.Collections;

public class ScoutTrapScript : Trap {

	GameObject owner; // The owner of this trap
	
	/**
	 * Activate function
	 * 
	 * Arguments
	 * - Player player - The player that activated the trap
	 */
	public override void Activated(Player p) {
		if (owner != null && owner.GetComponent<Player>() == p) // Owner activated us 
			Debug.Log("I'm sorry master. I won't activate");
		else if (owner == null) {
			Debug.Log("No master. Don't do anything");
			return;
		} else { // Other players
			Debug.Log("Activated scout trap");
			p.GetComponent<PhotonView>().RPC("Stun", p.GetComponent<PhotonView>().owner, 3);
			p.GetComponent<PhotonView>().RPC("DisplayStunAnim", PhotonTargets.All, null);
			GetComponent<PhotonView>().RPC("Destroy", PhotonTargets.All, null);
		}
	}

	/**
	 * Set the owner of this trap
	 * 
	 * Arguments
	 * - GameObject newOwner - The new owner
	 */
	public void SetOwner(GameObject newOwner) {
		owner = newOwner;
	}

	/**
	 * Get the owner of this trap
	 * 
	 * Returns
	 * - The owner of this trap
	 */
	public GameObject GetOwner() {
		return owner;
	}

	/*
	 * Only the master client or the client that spawned gameObject can call
	 * PhotonNetwork.Destroy(gameObject). This method just tells that client to do so.
	 */
	[PunRPC]
	void Destroy() {
		if (GetComponent<PhotonView>().isMine)
			PhotonNetwork.Destroy(gameObject);
	}
}
