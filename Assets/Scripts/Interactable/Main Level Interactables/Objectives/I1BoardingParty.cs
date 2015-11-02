using UnityEngine;
using System.Collections;

public class I1BoardingParty : InteractiveObject {

	void Start() {
		StartMe();
	}

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			Debug.Log("Opened");
			CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
		
	}
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	[PunRPC]
	void Sync() {
		BoardingPartyDestroyed notification; // The notification
		Object.FindObjectOfType<GameManager>().DecreaseRoundsLost(BoardingParty.ROUNDS_LOST);
		IsInactivated = true;
		PhotonNetwork.Destroy(GetComponent<PhotonView>()); // Destroy this object's network syncer
		Player.MyPlayer.GetComponentInChildren<BoardingParty>().OnComplete();
		notification = gameObject.AddComponent<BoardingPartyDestroyed>();
		notification.CreateCard();
	}
}
