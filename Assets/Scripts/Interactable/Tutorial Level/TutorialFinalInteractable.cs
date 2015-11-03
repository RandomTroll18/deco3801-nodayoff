using UnityEngine;
using System.Collections;

/*
 * Fourth Tutorial Objective - Advantageous class multiplier
 */
public class TutorialFinalInteractable : Trap {
	
	
	public override void Activate() {
		// Check if correct part of the story
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<TutorialFinalObjective>().Title)) {
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
