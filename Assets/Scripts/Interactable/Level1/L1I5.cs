using UnityEngine;

/*
 * Level 1 Interactable 5 - Skill Panel Interactable (Completes Objective)
 */
public class L1I5 : InteractiveObject {

	public override void TakeAction(int input){
		
		if (IsInactivated) { // This object is inactive. Don't activate it
			Debug.Log ("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(new FifthObjective().Title)) { // Wrong objective
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) { // Succeeded AP roll
			InteractablSync();
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			CloseEvent();
		} else // AP roll failed
			Debug.Log("Failed");
	}

	/**
	 * Sync this interactive object with other players
	 */
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	/**
	 * The RPC call for syncing this object
	 */
	[PunRPC]
	void Sync() {
		IsInactivated = true;
	}
	

}
