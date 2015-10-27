using UnityEngine;
using System.Collections;

public class BoardingPartyDestroyed : EventCard {

	public override void ChangeCard(){
		
		ListNumber = 1;
		
		this.ChangeButton (1, "OK");
		this.ChangeImage("ui/events/alienboard");
		this.ChangeTitle("Boarding Party");
		this.ChangeText ("Boarding party destroyed.");
		
		SetCap();
		return;
	}
	
	public override void CardEffect(int highestVote){
		// nothing
	}
}
