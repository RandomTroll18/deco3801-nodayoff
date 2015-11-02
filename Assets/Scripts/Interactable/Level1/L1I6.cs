using UnityEngine;
using System.Collections;

public class L1I6 : InteractiveObject {
	 
	public override void TakeAction(int input){
		
		if (IsInactivated) { // Not active. Don't take action
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(new SixthObjective().Title)) { // Wrong objective
			Debug.Log("Wrong part of the story");
			return;
		}

		InteractablSync();
		PrimaryO.OnComplete(); // Run actions for the completion of the primary objective
	}

	/**
	 * Sync this object with other players
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
