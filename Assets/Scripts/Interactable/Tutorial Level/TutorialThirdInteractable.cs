using UnityEngine;
using System.Collections;

public class TutorialThirdInteractable : InteractiveObject {
	
	public override void TakeAction(int input){

		// Check if activated
		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}

		// Check if correct part of the story
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<TutorialThirdObjective>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) { // Success
			//MController.RemoveInteractable(this.GetTile());
			InteractablSync();
			PrimaryO.OnComplete ();
			Debug.Log ("Opened");
			CloseEvent();
		} else { // Fail
			Debug.Log("Failed");
		}

		
	}

	/*
	 * Handling game syncronization
	 */
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	/*
	 * RPC call for syncronizing game state.
	 */ 
	[PunRPC]
	void Sync() {
		IsInactivated = true;
		MController.RemoveInteractable(GetTile());
	}
	
}