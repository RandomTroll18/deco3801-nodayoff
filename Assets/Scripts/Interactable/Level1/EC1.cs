using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class EC1 : EventCard {
	
	public override void ChangeCard(){

		ListNumber = 1;

		ChangeButton(1, "ASDASD");
		ChangeImage("chopper");
		ChangeText("DESCRIPTION");

		SetCap();
	}
	
	public override void CardEffect(int highestVote){
		Object.FindObjectOfType<GameManager>().GetComponent<PhotonView>().RPC("IncreaseRoundsLost", 0, 1);
	}

}