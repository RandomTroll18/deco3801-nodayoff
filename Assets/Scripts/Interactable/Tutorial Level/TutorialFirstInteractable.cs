using UnityEngine;
using System.Collections;

public class TutorialFirstInteractable: InteractiveObject {

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<TutorialFirstObjective>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			PrimaryO.OnComplete();
			InteractablSync();
			Debug.Log ("Opened");
			CloseEvent();
		} else {
			Debug.Log("Failed");
		}
	}

	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

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
