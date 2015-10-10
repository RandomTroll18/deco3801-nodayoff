using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class EC1 : EventCard {
	const int ROUNDS_LOST = 1;

	public override void ChangeCard(){

		ListNumber = 1;

		this.ChangeButton (1, "ASDASD");
		this.ChangeImage("chopper");
		this.ChangeText ("DESCRIPTION");

		SetCap();
		return;
	}
	 
	public override void CardEffect(int highestVote){
		Object.FindObjectOfType<GameManager>().GetComponent<PhotonView>()
			.RPC("IncreaseRoundsLost", 0, 1);
	}

}