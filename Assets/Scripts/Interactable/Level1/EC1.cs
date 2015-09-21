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

		UnityAction Destroy = (() => Method1());
		UnityAction Rainbows = (() => Method2());
		UnityAction Explosions = (() => Method3());

		this.ChangeButton (2, "DESTROY", Destroy);
		this.ChangeButton (1, "EXPLOSIONS", Explosions);
		this.ChangeButton (0, "RAINBOWS", Rainbows);
		this.ChangeImage("chopper");
		this.ChangeText ("Didn't want to stray from the format I already had. So I put this OP, Not castable, Wrong Contexted Hearthstone card within another card.");
		return;
	}
	 
	void Method1(){
		Debug.Log("I WILL DESTROY YOU");
		Destroy(card);
	}
	
	void Method2(){
		Debug.Log("BOOOOOOOOOOOOOOM EXPLOOOOOOOSSSIIOOOOOOOOOOONS");
		Destroy(card);
	}
	
	void Method3(){
		Debug.Log("PINK FLUFFY UNICORN RIDING ON RAINBOWS");
		Destroy(card);
	}


}