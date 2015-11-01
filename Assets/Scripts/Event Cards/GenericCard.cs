using UnityEngine;
using System.Collections;

/*
 * Use this as a generic card that does nothing and only has an OK button/
 */
public class GenericCard : EventCard {
	public string Message; // The message on this event card
	public string Title; // The message's title
	public string Image; // The image for this event card

	/** 
	 * Change the current event card
	 */
	public override void ChangeCard(){

		ChangeButton(1, "OK");
		ChangeImage(Image);
		ChangeTitle(Title);
		ChangeText(Message);
		
		SetCap();
	}
}
