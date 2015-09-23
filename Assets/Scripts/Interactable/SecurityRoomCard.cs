using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class SecurityRoomCard : EventCard {
	
	public override void ChangeCard(){

		Resolve = new Function(Kill);
		ListNumber = 0;

		this.ChangeButton (1, "Player 1");
		this.ChangeButton (2, "Player 2");
		this.ChangeButton (3, "Player 3");
		this.ChangeButton (4, "Player 4");

		SetCap();

	}

	public override void CardEffect(int highestVote){
		Kill(highestVote);
	}

	private void Kill(int toKill){
		Debug.Log("kill" + toKill);
	}
	
}