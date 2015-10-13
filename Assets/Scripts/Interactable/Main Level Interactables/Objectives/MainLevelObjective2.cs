using UnityEngine;
using System.Collections;

public class MainLevelObjective2 : InteractiveObject {

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<FirstObjectiveMain>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			Debug.Log("Opened");
		} else {
			Debug.Log("Failed with " + input);
			this.CloseEvent();	
		}
	}
	
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync(){
		// TODO
		
	}
}
