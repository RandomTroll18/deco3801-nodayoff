﻿using UnityEngine;
using System.Collections;

public class AlienSecondarySixInteractable : InteractiveObject {

	public override void StartMe() {
		base.StartMe(Object.FindObjectOfType<GameManager>());
		InstantInteract = true;
	}
	
	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			Sync();
			IsInactivated = true;
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
		
	}

	[PunRPC]
	void Sync() {
		// The alien's secondary
		AlienSecondarySix tmp = Player.MyPlayer.GetComponentInChildren<AlienSecondarySix>();
		if (tmp != null)
			tmp.OnComplete();
		else
			Debug.Log("Player does not have this objective");
	}
}
