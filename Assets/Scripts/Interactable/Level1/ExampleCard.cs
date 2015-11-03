using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/**
 * An example event card
 */
public class ExampleCard : EventCard {
	
	public override void ChangeCard(){

		ListNumber = 1; // Poll vote list ID

		// Intitializing event card characteristics
		ChangeButton(1, "DESTROY");
		ChangeButton(2, "EXPLOSIONS");
		ChangeButton(3, "RAINBOWS");
		ChangeImage("chopper");
		ChangeText("Didn't want to stray from the format I already had. So I put this OP, Not castable, Wrong Contexted Hearthstone card within another card.");
		
		SetCap(); // Set vote cap
	}
	
	public override void CardEffect(int highestVote){
		if (highestVote == 3)
			Debug.Log("D0u8Le R4iNBoow AlI th3 W4ay Acro55 Th3 SkY");
		else if (highestVote == 2) {
			Debug.Log("DID YOU KNOW THAT NINETY-SEVEN PERCENT OF ALL LIVING THINGS ON " +
			          "PANDORA AREN'T EXPLODING RIGHT NOW? THAT'S BULLSH*T, BUY TORGUE!");
		} else if (highestVote == 1)
			Debug.Log("RIP");
		else
			Debug.Log("It's not suppose to get here. Value is: " + highestVote); // Handle error case
	}
	
}