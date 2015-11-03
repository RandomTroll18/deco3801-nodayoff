using UnityEngine;
using System.Collections;

/*
 * First Tutorial Objective - Locked Door Interactable
 */
public class TutorialFirstInteractable: InteractiveObject {

	public override void TakeAction(int input){

		// Check if activated
		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}

		// Check if correct part of the story
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<TutorialFirstObjective>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) { // Success
			PrimaryO.OnComplete();
			InteractablSync();
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
		try {
			transform.GetChild(8).GetComponent<Light>().color = Color.red;
		} catch (UnityException){
			Debug.LogWarning("Exception caught when setting light");
		}
		IsInactivated = true;
		MController.RemoveInteractable(GetTile());
	}

}
