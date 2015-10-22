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
			this.CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			this.CloseEvent();	
		}
		
	}
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	[PunRPC]
	void Sync() {
		Object.FindObjectOfType<GameManager>().DecreaseRoundsLost(BoardingParty.ROUNDS_LOST);
		IsInactivated = true;
		PhotonNetwork.Destroy(GetComponent<PhotonView>());
		Player.MyPlayer.GetComponentInChildren<BoardingParty>().OnComplete();
		BoardingPartyDestroyed notification = gameObject.AddComponent<BoardingPartyDestroyed>();
		notification.CreateCard();
	}
}
