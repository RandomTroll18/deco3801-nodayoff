using UnityEngine;
using System.Collections;

public class AlienSecondaryFourInteractable : InteractiveObject {

	public override void StartMe() {
		base.StartMe(Object.FindObjectOfType<GameManager>());
		this.InstantInteract = true;
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
			this.CloseEvent();	
		}
		
	}
	
	[PunRPC]
	void Sync() {
		// activate objective
		AlienSecondaryFour tmp = Player.MyPlayer.GetComponentInChildren<AlienSecondaryFour>();
		if (tmp != null)
			tmp.OnComplete();
		else
			Debug.Log("Player does not have this objective");
	}
}
