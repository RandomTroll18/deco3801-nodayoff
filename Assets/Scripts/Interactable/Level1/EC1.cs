using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/*
 * Level 1 Event Card 1: Example Event - Change Rounds:
 */
public class EC1 : EventCard {
	
	public override void ChangeCard(){

		// Poll List ID
		ListNumber = 1;

		// Initializing Event card characteristics
		ChangeButton(1, "ASDASD");
		ChangeImage("chopper");
		ChangeText("DESCRIPTION");

		SetCap(); // Set Vote cap
	}

	/**
	 * Handle sending of private message
	 * 
	 * Arguments
	 * - int highestVote - Option with the highest vote
	 */
	public override void CardEffect(int highestVote){
		Object.FindObjectOfType<GameManager>().GetComponent<PhotonView>().RPC("IncreaseRoundsLost", 0, 1);
	}

}