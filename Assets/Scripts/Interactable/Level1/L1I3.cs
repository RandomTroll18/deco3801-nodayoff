using UnityEngine;
using System.Collections;

public class L1I3 : InteractiveObject {

	public override void TakeAction(int input){
		
		if (IsInactivated) { // Not active
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(new FourthObjective().Title)) { // Wrong objective
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) { // AP roll successful
			InteractablSync();
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			CloseEvent();
		} else
			Debug.Log("Failed");
	}

	/**
	 * Sync interactable with other players
	 */
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	/**
	 * RPC call for syncing
	 */
	[PunRPC]
	void Sync() {
		IsInactivated = true;
	}


}
