using UnityEngine;
using System.Collections;

public class TutorialFinalInteractable : Trap {
	
	
	public override void Activate() {
		
		if (!PrimaryO.GetObjective().Title.Equals(new TutorialFinalObjective().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		PrimaryO.OnComplete ();
		Debug.Log("Sorry Yugi, but you've triggered my trap card!");
		
	}
	
	[PunRPC]
	void Sync() {
		Destroy(this.gameObject);
	}
	
}
