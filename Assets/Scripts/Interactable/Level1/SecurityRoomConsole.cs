using UnityEngine;
using System.Collections;

public class SecurityRoomConsole : InteractiveObject {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}

		SecurityRoomCard SRC = gameObject.AddComponent<SecurityRoomCard>();
		GameObject SRCGO = SRC.CreateCard ();
		this.CloseEvent();

	}
}
