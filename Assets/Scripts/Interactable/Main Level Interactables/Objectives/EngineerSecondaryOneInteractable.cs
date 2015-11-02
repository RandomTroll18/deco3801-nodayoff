using UnityEngine;

public class EngineerSecondaryOneInteractable : InteractiveObject {

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
		// The engineer's secondary objective
		EngineerSecondaryOne tmp = Player.MyPlayer.GetComponentInChildren<EngineerSecondaryOne>();
		if (tmp != null) // The player is the engineer.
			tmp.OnComplete();
		else // The player is not the engineer
			Debug.Log("Player does not have this objective");
	}
}
