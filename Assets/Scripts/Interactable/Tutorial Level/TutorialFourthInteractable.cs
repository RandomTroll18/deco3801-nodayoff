using UnityEngine;
using System.Collections;

public class TutorialFourthInteractable : InteractiveObject {

	
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
		
		if (!PrimaryO.GetObjective().Title.Equals(new TutorialFourthObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			//MController.RemoveInteractable(this.GetTile());
			InteractablSync();
			PrimaryO.OnComplete ();
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
		IsInactivated = true;
		Tile DT = new Tile(
			Tile.TilePosition(Door.transform.position.x), 
			Tile.TilePosition(Door.transform.position.z)
			);
		MController.RemoveInteractable(DT);
		Debug.Log(DT.ToString());
	}
	
}