using UnityEngine;
using System.Collections;

public class MainLevelObjective4 : InteractiveObject {

	// Use this for initialization
	void Start () {
	
	}
	
	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<FourthObjectiveMain>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			this.CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			this.CloseEvent();	
		}
	}

	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
		Application.LoadLevel("WinScreen");
	}

	[PunRPC]
	void Sync(){
		PhotonNetwork.Destroy(GetComponent<PhotonView>());
		ChatTest.Instance.AllChat(false, "ONE ESCAPE POD LOST");
	}

}
