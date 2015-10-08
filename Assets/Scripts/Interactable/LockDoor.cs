using UnityEngine;
using System.Collections;

public class LockDoor : InteractiveObject {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			//MController.RemoveInteractable(this.GetTile());
			InteractablSync();
			Debug.Log ("Opened");
			this.CloseEvent();
		} else {
			Debug.Log("Failed");
		}
		
		//TODO: Class 
		//TODO: Fix To not destroy door, and fix to destroy Interactable
		
	}
	
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		transform.GetChild(8).GetComponent<Light>().color = Color.red;
		IsInactivated = true;
		MController.RemoveInteractable(this.GetTile());
	}
	
}
