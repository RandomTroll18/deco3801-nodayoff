using UnityEngine;
using System.Collections;

public class MainLevelObjective4 : InteractiveObject {

	void Update() {

		if (Player.MyPlayer == null)
			return;
		/*
		 * Alien doesn't need these interactables
		 */
		if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() == Classes.BETRAYER) {
			Destroy(this);
			return;
		}
	}
	
	public override void TakeAction(int input){
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<FourthObjectiveMain>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			this.CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			this.CloseEvent();	
		}
	}

	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
		Application.LoadLevel("WinScreen");
	}

	[PunRPC]
	void Sync(){
		PhotonNetwork.Destroy(GetComponent<PhotonView>());
		ChatTest.Instance.AllChat(false, "ONE ESCAPE POD LOST");
	}

}
