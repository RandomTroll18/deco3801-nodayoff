﻿using UnityEngine;
using System.Collections;

public class ScoutSecondaryOneInteractable : InteractiveObject {

	public void StartMe() {
		base.StartMe(Object.FindObjectOfType<GameManager>());
	}
	
	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			Sync();
			IsInactivated = true;
			this.CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			this.CloseEvent();	
		}
		
	}
	
	[PunRPC]
	void Sync() {
		// activate objective
		ScoutSecondaryOne tmp = Player.MyPlayer.GetComponentInChildren<ScoutSecondaryOne>();
		if (tmp != null)
				tmp.OnComplete();
		else
			Debug.Log("Player does not have this objective");
	}
}