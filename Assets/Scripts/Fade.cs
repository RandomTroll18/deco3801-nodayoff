using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.Events;

public class FadeBigText : MonoBehaviour {
	
	public float FadeSpeed = 1.5f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Text text = this.gameObject.GetComponent<Text>();
		text.color = Color.Lerp(text.color, Color.clear, FadeSpeed * Time.deltaTime);
		if (text.color.a < 0.1f) {
			Destroy(this.gameObject);
		}
	}


}