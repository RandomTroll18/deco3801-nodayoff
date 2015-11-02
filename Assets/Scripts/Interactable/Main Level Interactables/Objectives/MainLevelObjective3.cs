using UnityEngine;
using System.Collections;

public class MainLevelObjective3 : InteractiveObject {

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<ThirdObjectiveMain>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			Debug.Log("Opened");
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
	}
	
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync(){
		GameObject secondaries; // The alien's secondaries
		if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() == Classes.BETRAYER) {
			// This is the alien
			secondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;
			secondaries.AddComponent<AlienSecondaryFive>().StartMe();
		}
		
	}
}
