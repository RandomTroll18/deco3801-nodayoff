using UnityEngine;
using System.Collections;

public class MainLevelObjective4 : InteractiveObject {

	bool escape = false; // Record if this player escaped or not
	
	public override void TakeAction(int input){
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}

		if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() == Classes.BETRAYER) {
			ChatTest.Instance.AllChat(true, "ALIEN CAN'T ESCAPE");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<FourthObjectiveMain>().Title)) {
			ChatTest.Instance.AllChat(true, "NO POWER");
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
	}

	public void InteractablSync() {
		escape = true;

		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All);
	}

	[PunRPC]
	void Sync(){
		PhotonNetwork.Destroy(GetComponent<PhotonView>());
		ChatTest.Instance.AllChat(false, "ONE ESCAPE POD LOST");

		if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() == Classes.BETRAYER
		    	&& GameObject.FindGameObjectsWithTag("Player").Length <= 1) {
			// No more humans. Alien lost
			Application.LoadLevel("AlienLoseScreen");
		}

		if (escape) // Alien won
			Application.LoadLevel("WinScreen");
	}

}
