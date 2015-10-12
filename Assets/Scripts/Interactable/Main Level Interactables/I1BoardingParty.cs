using UnityEngine;
using System.Collections;

public class I1BoardingParty : InteractiveObject {

	/* 
	 * So I want this interactable to have a cost and when it's activated it should just call
	 * DecreaseRoundsLeft. Don't worry about deactivating the interactable - I'll destroy it when
	 * it's been activated.
	 * 
	 * Interactable needs to be spawned in
	 */

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			Sync();
			IsInactivated = true;
			Debug.Log("Opened");
			this.CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			this.CloseEvent();	
		}
		
	}

	[PunRPC]
	void Sync() {
		Object.FindObjectOfType<GameManager>().DecreaseRoundsLost(MainLevelObjective1.ROUNDS_LOST);
		IsInactivated = true;
		PhotonNetwork.Destroy(GetComponent<PhotonView>());
		// TODO unblock the tile this interactable was on
	}
}
