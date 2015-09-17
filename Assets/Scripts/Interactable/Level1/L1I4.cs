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
			IsInactivated = true;
			MController.RemoveInteractable(this.GetTile());
			Destroy(Door);
			PrimaryO.OnComplete ();
			Debug.Log ("Opened");
			this.CloseEvent();
			EC2 Biceps = gameObject.AddComponent<EC2> ();
			GameObject BicepsUI = Biceps.CreateCard ();
		} else {
			Debug.Log("Failed");
		}
		
		
		//TODO: Class 
		//TODO: Fix To not destroy door, and fix to destroy Interactable

	}
}
