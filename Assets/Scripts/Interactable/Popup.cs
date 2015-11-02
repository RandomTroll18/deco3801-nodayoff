using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Popup : MonoBehaviour {
	
	protected GameObject PopupGameObject; // The object of the popup

	public Popup NextPopup; // The next popup to display

	/**
	 * Create the popup
	 */
	public GameObject Create() {
		GameObject Canvas = GameObject.Find("Main_Canvas"); // The canvas

		PopupGameObject = Instantiate(Resources.Load("PopupUI")) as GameObject;
		PopupGameObject.transform.SetParent(Canvas.transform, false);
		ChangeButton();
		ChangeContent();
		return PopupGameObject;
	}

	/**
	 * Close the popup
	 */
	void ClosePopup() {
		Destroy(PopupGameObject);
		if (NextPopup) // Create the next popup
			NextPopup.Create();
	}

	/**
	 * Change the content of this popup
	 */
	public virtual void ChangeContent() {
	}

	/**
	 * Change the button of this popup
	 */
	public void ChangeButton() {
		GameObject b = PopupGameObject.transform.GetChild(1).gameObject; // The button to change
		Button target = b.GetComponent<Button>(); // The script of the button
		
		target.onClick.RemoveAllListeners();
		target.onClick.AddListener(() => ClosePopup());

	}
	
}
