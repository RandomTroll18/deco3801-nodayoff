using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupTrap : Trap {

	public bool DestroyTrap;
	public Popup PopupScript;

	public override void TrapToDo(Collision col) {
		if (col.gameObject.name == "Player(Clone)") {
			Activate();
			// TODO: check for photon view?
			if (DestroyTrap) TrapSync();
		}
	}

	public override void Activate(){
		// TODO: call card
		if (PopupScript != null) {
			PopupScript.Create();
		}
	}

}
