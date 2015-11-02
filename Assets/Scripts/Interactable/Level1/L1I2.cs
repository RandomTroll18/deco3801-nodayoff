using UnityEngine;
using System.Collections;

public class L1I2 : InteractiveObject {

	public override void TakeAction(int input){
		
		if (IsInactivated) { // Not active. Don't activated
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(new SecondObjective().Title)) { // Wrong objective
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) { // AP roll successful
			InteractablSync();
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			CloseEvent();
		} else // AP roll failed
			Debug.Log("Failed");
	}

	/**
	 * Sync interactable with other players
	 */
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	/**
	 * RPC call for syncing with other players
	 */
	[PunRPC]
	void Sync() {
		ChangeRoundsPerTurn Nasty = gameObject.AddComponent<ChangeRoundsPerTurn>(); // Bad event
		IsInactivated = true;
		Nasty.CreateCard();
	}

}
