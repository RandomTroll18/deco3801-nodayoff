using UnityEngine;
using System.Collections;

public class EC1 : EventCard {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ChangeCard(){
		this.ChangeButton (0, "DESTROY", null);
		this.ChangeButton (1, "EXPLOSIONS", null);
		this.ChangeButton (2, "RAINBOWS", null);
		this.ChangeImage("chopper");
		this.ChangeText ("Didn't want to stray from the format I already had. So I put this OP, Not castable, Wrong Contexted Hearthstone card within another card.");
		return;
	}

}
