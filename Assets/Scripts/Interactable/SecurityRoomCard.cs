using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class SecurityRoomCard : EventCard {
	
	private int ListNumber = 0;
	private int VoteCap = 1;

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
		UnityAction KillFour = (() => Method4());
		
		this.ChangeButton (0, "Player 1", KillOne);
		this.ChangeButton (1, "Player 2", KillTwo);
		this.ChangeButton (2, "Player 3", KillThree);
		this.ChangeButton (3, "Player 4", KillFour);
		//this.ChangeImage("chopper");
		//this.ChangeText ("Didn't want to stray from the format I already had. So I put this OP, Not castable, Wrong Contexted Hearthstone card within another card.");
		return;
	}
	
	void Method1(){
		Vote(1);
		Destroy(card);
	}
	
	void Method2(){
		Vote(2);
		Destroy(card);
	}
	
	void Method3(){
		Vote(3);
		Destroy(card);
	}

	void Method4(){
		Vote(4);
		Destroy(card);
	}

	private void Vote(int playerNumber) {
		Poll Counter = GameObject.FindGameObjectWithTag("Poll").GetComponent<Poll>();
		Counter.AddToPoll(ListNumber, playerNumber);
		CheckKill(Counter, playerNumber);
		Destroy(card);
	}

	private void CheckKill(Poll counter){
		if (counter.CheckPoll(ListNumber, VoteCap)) {
			Debug.Log("Kill player " + counter.ReturnHighestCount(ListNumber));
		}
	}

	private void CheckKill(Poll counter, int player){
		Debug.Log("Count now " + counter.CheckCount(ListNumber, player));
		if (counter.CheckCount(ListNumber, player) == 2) {
			Debug.Log("Kill player " + player);
		}
	}
	
	
}