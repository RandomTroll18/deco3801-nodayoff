using UnityEngine;
using System.Collections;

public class AlienSecondaryTwoInteractable : InteractiveObject {

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
		AlienSecondaryTwo tmp = Player.MyPlayer.GetComponentInChildren<AlienSecondaryTwo>();
		if (tmp != null)
			tmp.OnComplete();
		else
			Debug.Log("Player does not have this objective");
	}
}
