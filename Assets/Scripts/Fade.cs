using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.Events;

public class Fade : MonoBehaviour {
	
	public float FadeSpeed = 1.5f; // The speed of the fade
	
	void Update () {
		Text text = gameObject.GetComponent<Text>(); // The text to fade
		text.color = Color.Lerp(text.color, Color.clear, FadeSpeed * Time.deltaTime);
		if (text.color.a < 0.1f) // Destroy the attached object
			Destroy(gameObject);
	}


}