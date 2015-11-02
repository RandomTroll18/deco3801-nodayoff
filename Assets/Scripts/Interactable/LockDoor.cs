using UnityEngine;
using System.Collections;

public class LockDoor : InteractiveObject {
	
	public override void TakeAction(int input){
		
		if (IsInactivated) {
			if (DebugOption) 
				Debug.Log("Inactive");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			InteractablSync();
			if (DebugOption) 
				Debug.Log("Opened");
			CloseEvent();
		} else {
			PlayFailureEfx();
			if (DebugOption) 
				Debug.Log("Failed");
		}

	}
	
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All);
	}
	
	[PunRPC]
	void Sync() {
		PlaySuccessEfx();
		IsInactivated = true;
		if (gameObject.GetComponent<DoorLight>() != null) // No light script
			transform.GetChild(8).GetComponent<Light>().color = Color.red;
		else // Destroy the network syncer
			PhotonNetwork.Destroy(GetComponent<PhotonView>());
		MController.RemoveInteractable(GetTile());

	}
	
}
