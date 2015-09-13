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

		if (playerScript.GetStatValue (Stat.AP) >= input) {
			playerScript.ReduceStatValue (Stat.AP, input);
			Debug.Log ("Reduced AP");
			double rng = Random.value;
			Debug.Log(rng);
			if (rng < Chance + (input/10)) {
				IsInactivated = true;
				PrimaryO.OnComplete ();
				Debug.Log ("Opened");
				this.CloseEvent();
			} else {
				Debug.Log ("Failed");
			}
		} else {
			Debug.Log ("Insufficient AP");
			return;
		}
		//TODO: Class interaction
		

	}
}
