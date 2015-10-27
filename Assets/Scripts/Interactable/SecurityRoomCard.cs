using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class SecurityRoomCard : EventCard {

	public override void ChangeCard(){

		this.TeamEvent = true;
		this.ChangeText("Someone has requested to activate the ship's security system. If one player" +
			" receives 3 votes, the security system will terminate them. There is only enough power" +
			" to activate the system once.");
		Resolve = new Function(Kill);
		ListNumber = 0;

		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			this.ChangeButton (player.ID, player.name);
		}

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