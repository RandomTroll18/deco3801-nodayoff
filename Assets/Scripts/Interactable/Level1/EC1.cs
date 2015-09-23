using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class EC1 : EventCard {

	//Dictionary<String, Integer> Poll = new Dictionary<String, Integer>(); 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ChangeCard(){

		ListNumber = 1;

		this.ChangeButton (1, "DESTROY");
		this.ChangeButton (2, "EXPLOSIONS");
		this.ChangeButton (3, "RAINBOWS");
		this.ChangeImage("chopper");
		this.ChangeText ("Didn't want to stray from the format I already had. So I put this OP, Not castable, Wrong Contexted Hearthstone card within another card.");

		SetCap();
		return;
	}
	 
	public override void CardEffect(int highestVote){
		if (highestVote == 3) {
			Debug.Log("D0u8Le R4iNBoow AlI th3 W4ay Acro55 Th3 SkY");
		} else if (highestVote == 2) {
			Debug.Log("DID YOU KNOW THAT NINETY-SEVEN PERCENT OF ALL LIVING THINGS ON " +
			          "PANDORA AREN'T EXPLODING RIGHT NOW? THAT'S BULLSH*T, BUY TORGUE!");
		} else if (highestVote == 1) {
			Debug.Log("RIP");
		} else {
			Debug.Log("It's not suppose to get here. Value is: " + highestVote);
		}
	}


}