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
			CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
		
	}
	
	[PunRPC]
	void Sync() {
		// Alien's secondary objective
		AlienSecondaryTwo tmp = Player.MyPlayer.GetComponentInChildren<AlienSecondaryTwo>();
		if (tmp != null) // This is the alien
			tmp.OnComplete();
		else // This is not the alien
			Debug.Log("Player does not have this objective");
	}
}
