using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class SecurityRoomCard : EventCard {
	
	//Dictionary<String, Integer> Poll = new Dictionary<String, Integer>(); 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void ChangeCard(){
		
		UnityAction KillOne = (() => Method1());
		UnityAction KillTwo = (() => Method2());
		UnityAction KillThree = (() => Method3());
		
		this.ChangeButton (2, "Player 1", KillOne);
		this.ChangeButton (1, "Player 2", KillTwo);
		this.ChangeButton (0, "Player 3", KillThree);
		this.ChangeImage("chopper");
		this.ChangeText ("Didn't want to stray from the format I already had. So I put this OP, Not castable, Wrong Contexted Hearthstone card within another card.");
		return;
	}
	
	void Method1(){
		Poll Counter = GameObject.FindGameObjectWithTag("Poll").GetComponent<Poll>();
		Counter.AddToPoll(0, 1);
		Debug.Log(Counter.CheckPoll(0, 1));
		Destroy(card);
	}
	
	void Method2(){
		Poll Counter = GameObject.FindGameObjectWithTag("Poll").GetComponent<Poll>();
		Counter.AddToPoll(0, 2);
		Debug.Log(Counter.ReturnHighestCount(0));
		Destroy(card);
	}
	
	void Method3(){
		Destroy(card);
	}

	void Method4(){
		Debug.Log("PINK FLUFFY UNICORN RIDING ON RAINBOWS");
		Destroy(card);
	}
	
	
}