using UnityEngine;
using System.Collections;

public class AlienSecondarySevenInteractable : InteractiveObject {

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
	
	// Update is called once per frame
	void Update () {
		
	}
	
	[PunRPC]
	void Sync() {
		// activate objective
		AlienSecondarySeven tmp = Player.MyPlayer.GetComponentInChildren<AlienSecondarySeven>();
		if (tmp != null)
			tmp.OnComplete();
		else
			Debug.Log("Player does not have this objective");
	}
}
