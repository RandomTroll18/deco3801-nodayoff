using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class PopupText : Popup {

	public string text;
	
	public override void ChangeContent() {
		GameObject b = PopupGameObject.transform.GetChild(0).gameObject;
		b.GetComponent<Text>().text = text;
	}
}
