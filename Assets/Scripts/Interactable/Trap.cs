using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Update is called once per frame
	public void Activated (Player p) {

		EventCard test = gameObject.AddComponent<EventCard> ();
		GameObject UI = test.CreateCard ();
		Debug.Log ("Sorry Yugi, but you've triggered my trap card!");
		Destroy(this.gameObject);

	}

}
