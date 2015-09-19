using UnityEngine;
using System.Collections;

public class EC2 : EventCard {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void ChangeCard(){

		this.ChangeButton (1, "OH... KAY..", null);
		this.ChangeImage("bicep");
		this.ChangeText ("Oooooooooh~ Look at you and your biceps.");

		return;
	}
	
}
