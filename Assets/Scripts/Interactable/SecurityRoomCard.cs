﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class SecurityRoomCard : EventCard {

	public override void ChangeCard(){

		TeamEvent = true;
		ChangeImage("ui/events/consoleimg");
		ChangeTitle("Security System");
		ChangeText("Someone has requested to activate the ship's security system. If one player" +
			" receives 3 votes, the security system will terminate them. There is only enough power" +
			" to activate the system once.");
		Resolve = new Function(Kill);
		ListNumber = 0;

		/* Assign the name of each player to their button for voting */
		foreach (PhotonPlayer player in PhotonNetwork.playerList) 
			ChangeButton(player.ID, player.name);

		SetCap();
	}

	public override void CardEffect(int highestVote){
		Kill(highestVote);
	}

	void Kill(int toKill){
		SecurityRoomConsole script = gameObject.GetComponent<SecurityRoomConsole>(); // The security console script
		script.DoKill(toKill);
	}
	
}