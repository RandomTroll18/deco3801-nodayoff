using UnityEngine;
using System.Collections;

public class MainLevelObjective2 : InteractiveObject {
	public GameObject[] DoorsToUnlock; // The doors to unlock

	/**
	 * Initialize this object as soon as it appears
	 */
	void Start() {
		StartMe();
		DoorsToUnlock = new GameObject[2];
		DoorsToUnlock[0] = GameObject.Find("Obj3Door2");
		DoorsToUnlock[1] = GameObject.Find("Obj3Door1");
	}

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<SecondObjectiveMain>().Title)) {
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
		foreach (GameObject door in DoorsToUnlock) { // Open the doors
			Object.FindObjectOfType<GameManager>()
				.OpenDoor(Tile.TilePosition(door.transform.position));
		}
		PhotonNetwork.Destroy(GetComponent<PhotonView>()); // Remove this objective's syncer
	}
}
