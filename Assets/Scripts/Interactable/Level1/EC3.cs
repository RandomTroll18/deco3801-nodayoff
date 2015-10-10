using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class EC3 : EventCard {

	public override void ChangeCard(){
		
		ListNumber = 1;
		
		this.ChangeButton (1, "OK");
		this.ChangeImage("chopper");
		this.ChangeText ("DESCRIPTION");
		
		SetCap();
		return;
	}
	
	public override void CardEffect(int highestVote){
		Object.FindObjectOfType<GameManager>().IncreaseRoundsLost(MainLevelObjective1.ROUNDS_LOST);
	}
	
}