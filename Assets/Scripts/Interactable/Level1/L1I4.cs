using UnityEngine;
using System.Collections;

public class L1I4 : InteractiveObject {

	public GameObject Door;

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

		if (!PrimaryO.GetObjective().Title.Equals(new ThirdObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) {
			InteractablSync();
			PrimaryO.OnComplete();
			Debug.Log ("Opened");
			this.CloseEvent();
			EC2 Biceps = gameObject.AddComponent<EC2>();
			Biceps.CreateCard();
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
		Destroy(Door);
		IsInactivated = true;
		MController.RemoveInteractable(this.GetTile());
	}


}
