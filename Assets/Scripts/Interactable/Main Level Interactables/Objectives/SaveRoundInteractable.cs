using UnityEngine;
using System.Collections;

public class SaveRoundInteractable : InteractiveObject {

	public override void StartMe() {
		base.StartMe(Object.FindObjectOfType<GameManager>());

		StringInput = "Repair the ship?";
		Cost = 5;
		ClassMultiplier = Stat.NOMULTIPLIER;
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
		SaveRounds tmp = Player.MyPlayer.GetComponentInChildren<SaveRounds>();
		if (tmp != null)
			tmp.OnComplete();
	}
}
