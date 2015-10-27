using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/*
 * Increase the number of rounds lost per turn
 */
public class ChangeRoundsPerTurn : EventCard {
	public int roundsLost = 1;


	public override void ChangeCard(){
		
		ListNumber = 1;
		
		this.ChangeButton (1, "OK");
		this.ChangeImage("UI/Events/alienboard");
		this.ChangeTitle("Boarding Party");
		this.ChangeText ("A Versipellis boarding party is destroying our ship from the inside. Stop" +
			" them from destroying our ship.\n" +
			"\n" +
			"An extra round is lost per turn until the boarding party is removed.");
		
		SetCap();
		return;
	}
	
	public override void CardEffect(int highestVote){
	}
	
}