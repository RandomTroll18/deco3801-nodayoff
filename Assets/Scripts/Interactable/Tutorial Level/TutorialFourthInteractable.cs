using UnityEngine;
using System.Collections;

/*
 * Fourth Tutorial Interactable - Advantageous class multiplier door
 */
public class TutorialFourthInteractable : InteractiveObject {

	
	public GameObject Door; // The door to unlock

	public override void TakeAction(int input){

		// Check if activated
		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}

		// Check if correct part of the story
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<TutorialFourthObjective>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) { // Success
			//MController.RemoveInteractable(this.GetTile());
			InteractablSync();
			PrimaryO.OnComplete ();
			Debug.Log ("Opened");
			IsInactivated = true;
			this.CloseEvent();
			PlaySuccessEfx();
		} else { // Fail
			PlayFailureEfx();
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
		Tile x = Tile.TilePosition(Door.transform.position);
		MController.UnblockTile(x);
		Destroy(Door);
		IsInactivated = true;
	}
	
}