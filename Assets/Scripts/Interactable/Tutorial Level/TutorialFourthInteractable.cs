using UnityEngine;
using System.Collections;

public class TutorialFourthInteractable : InteractiveObject {

	
	public GameObject Door; // The door to unlock

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<TutorialFourthObjective>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			//MController.RemoveInteractable(this.GetTile());
			InteractablSync();
			PrimaryO.OnComplete ();
			Debug.Log ("Opened");
			IsInactivated = true;
			this.CloseEvent();
			PlaySuccessEfx();
		} else {
			PlayFailureEfx();
			Debug.Log("Failed");
		}
	}

	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		Tile x = Tile.TilePosition(Door.transform.position);
		MController.UnblockTile(x);
		Destroy(Door);
		IsInactivated = true;
	}
	
}