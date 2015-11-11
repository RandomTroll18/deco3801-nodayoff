using UnityEngine;
using System.Collections;

/*
 * Level 1 Event Card 2: Example Event Card 2 - Marine Event Card
 */
public class EC2 : EventCard {
	
	public override void ChangeCard(){

		ListNumber = 2; // Poll vote list ID

		// Intitializing event card characteristics
		ChangeButton (1, "OH... KAY..");
		ChangeImage("bicep");
		ChangeText("Oooooooooh~ Look at you and your biceps.");

		SetCap(); // Set vote cap
	}

	public override void CardEffect(int highestVote){
		if (DebugOption) 
			Debug.Log("JOHN CEEENAAA");
	}
	
}
