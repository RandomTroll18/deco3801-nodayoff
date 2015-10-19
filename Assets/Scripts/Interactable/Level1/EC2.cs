using UnityEngine;
using System.Collections;

public class EC2 : EventCard {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void ChangeCard(){

		ListNumber = 2;

		this.ChangeButton (1, "OH... KAY..");
		this.ChangeImage("bicep");
		this.ChangeText ("Oooooooooh~ Look at you and your biceps.");

		SetCap();
		return;
	}

	public override void CardEffect(int highestVote){
		if (DebugOption) Debug.Log("JOHN CEEENAAA");

	}
	
}
