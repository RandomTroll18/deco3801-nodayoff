using UnityEngine;
using System.Collections;

/*
 * Use this to reduce the number of rounds left
 */
public class ChangeRounds : EventCard {
	/*
	 * Change this before showing the event card if you want to change the number of rounds lost
	 */
	public int RoundsLost = 1;

	/** 
	 * Change the current event card
	 */
	public override void ChangeCard(){
		
		ListNumber = 1;
		
		ChangeButton (1, "OK");
		ChangeImage("ui/events/explosion2");
		ChangeTitle("Ship Sabotaged");
		ChangeText ("The ship has been sabotaged. Rounds have been lost.");
		
		SetCap();
	}

	/**
	 * Apply the effect attached to this card
	 * 
	 * Arguments
	 * - int highestVote - Ignored
	 */
	public override void CardEffect(int highestVote){
		Object.FindObjectOfType<GameManager>().RoundsLeftUntilLose -= 1;
	}
}
