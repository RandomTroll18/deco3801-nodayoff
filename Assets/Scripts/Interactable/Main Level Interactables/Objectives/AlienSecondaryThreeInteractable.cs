using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienSecondaryThreeInteractable : InteractiveObject {

	public override void StartMe() {
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
		AlienSecondaryThree tmp = Player.MyPlayer.GetComponentInChildren<AlienSecondaryThree>();
		if (tmp != null)
			tmp.OnComplete();
		else
			Debug.Log("Player does not have this objective");
	}
}
