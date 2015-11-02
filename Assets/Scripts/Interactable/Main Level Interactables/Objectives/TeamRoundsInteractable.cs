using UnityEngine;
using System.Collections;

public class TeamRoundsInteractable : InteractiveObject {

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
			InteractablSync();
			IsInactivated = true;
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
		
	}

	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		// The extra round objective
		TeamExtraRounds tmp = Player.MyPlayer.GetComponentInChildren<TeamExtraRounds>();
		if (tmp != null)
			tmp.Progress();
	}
}
