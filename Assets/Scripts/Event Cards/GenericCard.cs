using UnityEngine;
using System.Collections;

/*
 * Use this as a generic card that does nothing and only has an OK button
 */
public class GenericCard : EventCard {
	string message; // The message on this event card
	string title; // The message's title
	string image; // The image for this event card

	bool updated = false;

	/** 
	 * Change the current event card. Make sure all the public variables are set before using this.
	 */
	public override void ChangeCard(){
		if (!updated)
			Debug.LogError("It seems you didn't call ChangePublics");

		ChangeButton(1, "OK");
		ChangeImage(image);
		ChangeTitle(title);
		ChangeText(message);
		
		SetCap();
	}

	/** 
	 * Change the current event card.
	 * 
	 * - string message - Card Description
	 * - string title - Card Title
	 * - string image - Card Image path
	 */
	public void ChangeContent(string message, string title, string image) {
		this.message = message;
		this.title = title;
		this.image = image;

		updated = true;
	}
}
