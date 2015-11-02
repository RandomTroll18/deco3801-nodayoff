using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupTrap : Trap {

	public bool DestroyTrap;
	public Popup PopupScript;

	public override void TrapToDo(Collision col) {
		if (col.gameObject.name == "Player(Clone)") // Activated by player
			Activate();
	}

	public override void Activate(){
		if (PopupScript != null) // Create popup script
			PopupScript.Create();
		if (DestroyTrap) // Trap is destroyed. Sync
			TrapSync();

	}

	public override void TrapSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		Destroy(gameObject);
	}


}
