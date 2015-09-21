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
		
		/*if (!PrimaryO.GetObjective().Title.Equals(new SecondObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}*/

		if (SpendAP(input, MinCost, Stat.TECHMULTIPLIER)) {
			//IsInactivated = true;
			PrimaryO.OnComplete ();
			Debug.Log ("Opened");
			this.CloseEvent();
			SecurityRoomCard SRC = gameObject.AddComponent<SecurityRoomCard>();
			GameObject SRCGO = SRC.CreateCard ();
		} else {
			Debug.Log("Failed");
		}

		
		//TODO: Class interaction

	}
}
