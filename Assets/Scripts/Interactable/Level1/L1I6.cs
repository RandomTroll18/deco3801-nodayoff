using UnityEngine;
using System.Collections;

public class L1I6 : InteractiveObject {

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
		
		if (!PrimaryO.GetObjective().Title.Equals(new SixthObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}


		
		//TODO: Class interaction
		
		
		InteractablSync();
		PrimaryO.OnComplete ();
	}

	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		IsInactivated = true;
	}
}
