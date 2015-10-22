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
		this.ChangeImage("chopper");
		this.ChangeText ("A Versipellis boarding party is destroying our ship from the inside. " +
			"Take them out.");
		
		SetCap();
		return;
	}
	
	public override void CardEffect(int highestVote){
	}
	
}