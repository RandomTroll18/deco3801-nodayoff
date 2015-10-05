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
			Debug.Log("Inactive");
			return;
		}

		if (!PrimaryO.GetObjective().Title.Equals(new FirstObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost, Stat.TECHMULTIPLIER)) {
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
	void Sync() {
		//Destroy(Door);
		MController.UnblockTile(Tile.TilePosition(Door.transform.position));
		IsInactivated = true;
	}


}
