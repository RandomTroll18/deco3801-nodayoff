using UnityEngine;
using System.Collections;

public class BoardingPartyDestroyed : EventCard {

	/**
	 * Change this card
	 */
	public override void ChangeCard(){
		
		ListNumber = 1;
		
		ChangeButton (1, "OK");
		ChangeImage("ui/events/alienboard");
		ChangeTitle("Boarding Party");
		ChangeText ("Boarding party destroyed.");
		
		SetCap();
	}
}
