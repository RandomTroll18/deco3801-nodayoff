using UnityEngine;
using System.Collections;

public class EC2 : EventCard {
	
	public override void ChangeCard(){

		ListNumber = 2;

		ChangeButton (1, "OH... KAY..");
		ChangeImage("bicep");
		ChangeText("Oooooooooh~ Look at you and your biceps.");

		SetCap();
	}

	public override void CardEffect(int highestVote){
		if (DebugOption) 
			Debug.Log("JOHN CEEENAAA");
	}
	
}
