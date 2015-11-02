using UnityEngine;
using System.Collections;

public class L1I4 : InteractiveObject {

	public GameObject Door; // The door we are attached to

	public override void TakeAction(int input){

		if (IsInactivated) { // This is inactive. Don't activate
			Debug.Log("Inactive");
			return;
		}

		if (!PrimaryO.GetObjective().Title.Equals(new ThirdObjective().Title)) { // Wrong objective
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) { // AP roll success
			InteractablSync();
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			CloseEvent();
			EC2 Biceps = gameObject.AddComponent<EC2>();
			Biceps.CreateCard();
		} else // AP roll failed
			Debug.Log("Failed");
	}

	/**
	 * Sync with other players
	 */
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	/**
	 * RPC call for syncing
	 */
	[PunRPC]
	void Sync() {
		Destroy(Door);
		IsInactivated = true;
		MController.RemoveInteractable(GetTile());
	}


}
