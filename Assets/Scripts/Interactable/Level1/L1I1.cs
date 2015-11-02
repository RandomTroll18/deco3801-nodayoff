using UnityEngine;
using System.Collections;

public class L1I1 : InteractiveObject {

	public GameObject Door; // The reference to the door object we are attached to


	public override void TakeAction(int input){
		EC1 Chopper; // Random event card for testing purposes

		if (IsInactivated) { // Not active.
			if (DebugOption) 
				Debug.Log("Inactive");
			return;
		}

		if (!PrimaryO.GetObjective().Title.Equals(new FirstObjective().Title)) { // Wrong objective
			if (DebugOption) 
				Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) { // AP roll succeeded
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			if (DebugOption) 
				Debug.Log("Opened");
			CloseEvent();		
			Chopper = gameObject.AddComponent<EC1>();
			Chopper.CreateCard();
		} else { // AP roll failed
			if (DebugOption) 
				Debug.Log("Failed with " + input);
			CloseEvent();	
		}
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
		MController.UnblockTile(Tile.TilePosition(Door.transform.position));
		IsInactivated = true;
	}


}
