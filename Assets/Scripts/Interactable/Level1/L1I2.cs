using UnityEngine;
using System.Collections;

public class L1I2 : InteractiveObject {

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
		
		if (!PrimaryO.GetObjective().Title.Equals(new SecondObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) {
			InteractablSync();
			PrimaryO.OnComplete ();
			Debug.Log ("Opened");
			this.CloseEvent();
		} else {
			Debug.Log("Failed");
		}

		//TODO: Class interaction

	}

	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		IsInactivated = true;
		ChangeRoundsPerTurn Nasty = gameObject.AddComponent<ChangeRoundsPerTurn>();
		GameObject NastyUI = Nasty.CreateCard ();
	}

}
