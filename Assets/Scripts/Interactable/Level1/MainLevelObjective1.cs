using UnityEngine;
using System.Collections;

public class MainLevelObjective1 : InteractiveObject {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(new FirstObjectiveMain().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			this.CloseEvent();		
			EC1 Chopper = gameObject.AddComponent<EC1>();
			GameObject ChopperUI = Chopper.CreateCard ();
		} else {
			Debug.Log("Failed with " + input);
			this.CloseEvent();	
		}
		
		//TODO: Sync others in game... 
		//TODO: Check game state if same??? do we need that?
		
	}
	
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync(){
		IsInactivated = true;
		EC3 Nasty = gameObject.AddComponent<EC3>();
		GameObject NastyUI = Nasty.CreateCard ();
	}
	
	
}
