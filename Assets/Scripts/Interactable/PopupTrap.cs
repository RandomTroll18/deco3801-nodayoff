using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupTrap : Trap {

	public bool DestroyTrap;
	public Popup PopupScript;

	public override void TrapToDo(Collision col) {
		if (col.gameObject.name == "Player(Clone)") {
			Activate();
		}
	}

	public override void Activate(){
		// TODO: call card
		if (PopupScript != null) {
			PopupScript.Create();
		} else {
			if (DestroyTrap) TrapSync();
		}

	}

	public virtual void TrapSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		Destroy(this.gameObject);
	}


}
