using UnityEngine;
using System.Collections;

public class TechnicianSecondaryOneInteractable : InteractiveObject {

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
			CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
		
	}
	
	[PunRPC]
	void Sync() {
		// activate objective
		TechnicianSecondaryOne tmp = Player.MyPlayer.GetComponentInChildren<TechnicianSecondaryOne>();
		if (tmp != null)
			tmp.OnComplete();
		else
			Debug.Log("Player does not have this objective");
	}
}
