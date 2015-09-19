﻿using UnityEngine;
using System.Collections;

public class L1I1 : InteractiveObject {

	public GameObject Door;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void TakeAction(int input){

		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}

		if (!PrimaryO.GetObjective().Title.Equals(new FirstObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}


		if (SpendAP(input, MinCost, Stat.TECHMULTIPLIER)) {
			IsInactivated = true;
			MController.UnblockTile(Tile.TilePosition(Door.transform.position));
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			this.CloseEvent();		
			EC1 Chopper = gameObject.AddComponent<EC1>();
			GameObject ChopperUI = Chopper.CreateCard ();
		} else {
			Debug.Log("Failed with " + input);
			this.CloseEvent();	
		}


		//TODO: Class interaction
		//TODO: Toggle Door?
		//TODO: Door animation?

	}
}
