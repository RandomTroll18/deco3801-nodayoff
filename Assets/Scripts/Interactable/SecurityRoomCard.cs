using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class SecurityRoomCard : EventCard {

	public override void ChangeCard(){

		this.TeamEvent = true;
		Resolve = new Function(Kill);
		ListNumber = 0;

		/*
		for (int i = 1; i < 5; i++) {
			this.ChangeButton (i, "Player 1");
			Player.
		} 
		*/


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
		SecurityRoomConsole script = this.gameObject.GetComponent<SecurityRoomConsole>();
		script.DoKill(toKill);
	}
	
}