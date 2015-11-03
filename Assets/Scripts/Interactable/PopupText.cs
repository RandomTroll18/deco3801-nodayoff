using UnityEngine;
using UnityEngine.UI;

public class PopupText : Popup {

	public string TextToSet; // The text to set

	/**
	 * Change popup text to TextToSet string
	 */
	public override void ChangeContent() {
		GameObject b = PopupGameObject.transform.GetChild(0).gameObject; // The button to change
		b.GetComponent<Text>().text = TextToSet;
	}
}
