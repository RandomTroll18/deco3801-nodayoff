using UnityEngine;
using System.Collections;

/*
 * Use this to destroy anything that has a PhotonView (even if you're not the game master)
 */
public class Destroy : MonoBehaviour {

	/**
	 * RPC call for destroying an object's photon view
	 */
	[PunRPC]
	public void PhotonDestroy() {
		PhotonNetwork.Destroy(GetComponent<PhotonView>());
	}
}
