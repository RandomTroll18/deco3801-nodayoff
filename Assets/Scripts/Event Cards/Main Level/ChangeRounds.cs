using UnityEngine;
using System.Collections;

/*
 * Use this to reduce the number of rounds left
 */
public class ChangeRounds : EventCard {
	/*
	 * Change this before showing the event card if you want to change the number of rounds lost
	 */
	public int roundsLost = 1;

	public override void ChangeCard(){
		
		ListNumber = 1;
		
		this.ChangeButton (1, "OK");
		this.ChangeImage("chopper");
		this.ChangeText ("Ship sabotages. Time until destruction reduced.");
		
		SetCap();
		return;
	}
	
	public override void CardEffect(int highestVote){
		Object.FindObjectOfType<GameManager>().RoundsLeftUntilLose -= 1;
	}
}
