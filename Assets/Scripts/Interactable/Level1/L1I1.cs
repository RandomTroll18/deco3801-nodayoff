using UnityEngine;
using System.Collections;

public class L1I1 : InteractiveObject {

	public GameObject Door;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void TakeAction(int input){

		if (IsInactivated) {
			if (DebugOption) Debug.Log("Inactive");
			return;
		}

		if (!PrimaryO.GetObjective().Title.Equals(new FirstObjective().Title)) {
			if (DebugOption) Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			if (DebugOption) Debug.Log("Opened");
			CloseEvent();		
			EC1 Chopper = gameObject.AddComponent<EC1>();
			Chopper.CreateCard ();
		} else {
			if (DebugOption) Debug.Log("Failed with " + input);
			CloseEvent();	
		}

		//TODO: Sync others in game... 
		//TODO: Check game state if same??? do we need that?

	}

	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		//Destroy(Door);
		MController.UnblockTile(Tile.TilePosition(Door.transform.position));
		IsInactivated = true;
	}


}
