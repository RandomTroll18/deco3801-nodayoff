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

	public override void TakeAction(float input){

		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}

		if (!PrimaryO.GetObjective().Title.Equals(new ThirdObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		//TODO: Class interaction
		

		PrimaryO.OnComplete ();

		MController.RemoveInteractable (this.GetTile());
		//TODO: Fix To not destroy door, and fix to destroy Interactable
		Destroy(Door);
		//TODO: Objective add
	}
}
