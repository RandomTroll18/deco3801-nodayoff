﻿using UnityEngine;
using System.Collections;

/*
 * Use this as a generic card that does nothing and only has an OK button/
 */
public class GenericCard : EventCard {
	public string message;

	public override void ChangeCard(){

		this.ChangeButton (1, "OK");
		this.ChangeImage("chopper");
		this.ChangeText(message);
		
		SetCap();
		return;
	}

	public override void CardEffect(int highestVote){
		// nothing
	}
}