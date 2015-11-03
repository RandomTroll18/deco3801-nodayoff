using UnityEngine;
using System.Collections;

public class TutorialSecondInteractable : Trap {


	public override void Activate() {

		// Check if correct part of the story
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<TutorialSecondObjective>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}

		PrimaryO.OnComplete ();
		Debug.Log("Sorry Yugi, but you've triggered my trap card!");
		
	}

	/*
	 * RPC call for syncronizing game state.
	 */ 
	[PunRPC]
	void Sync() {
		Destroy(gameObject);
	}
	
}
