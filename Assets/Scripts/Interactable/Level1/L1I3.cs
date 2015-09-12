using UnityEngine;
using System.Collections;

public class L1I3 : InteractiveObject {

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
		
		if (!PrimaryO.GetObjective().Title.Equals(new FourthObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		//TODO: Class interaction
		
		
		IsInactivated = true;
		PrimaryO.OnComplete ();
	}
}
