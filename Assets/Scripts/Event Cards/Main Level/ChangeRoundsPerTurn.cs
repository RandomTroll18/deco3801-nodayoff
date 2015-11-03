using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/*
 * Increase the number of rounds lost per turn.
 */
public class ChangeRoundsPerTurn : EventCard {
	public int RoundsLost = 1;

	/** 
	 * Change the current event card
	 */
	public override void ChangeCard(){
		
		ListNumber = 1;
		
		ChangeButton (1, "OK");
		ChangeImage("UI/Events/alienboard");
		ChangeTitle("Boarding Party");
		ChangeText ("A Versipellis boarding party is destroying our ship from the inside. Stop" +
			" them from destroying our ship." + StringMethodsScript.NEWLINE + StringMethodsScript.NEWLINE + 
			"An extra round is lost per turn until the boarding party is removed.");
		
		SetCap();
		return;
	}
}