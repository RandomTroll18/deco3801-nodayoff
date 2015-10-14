using UnityEngine;
using System.Collections;

public class MainLevelObjective1 : InteractiveObject {
	public const int ROUNDS_LOST = 1;
	public GameObject[] DoorsToUnlock;

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<FirstObjectiveMain>().Title)) {
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
		
		//TODO: Sync others in game... 
		//TODO: Check game state if same??? do we need that?
		
	}
	
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync(){
		GiveSecondaryObjectives();

		foreach (GameObject door in DoorsToUnlock) {
			Object.FindObjectOfType<GameManager>()
				.OpenDoor(Tile.TilePosition(door.transform.position));
		}

		IsInactivated = true;
		EC3 Nasty = gameObject.AddComponent<EC3>();
		GameObject NastyUI = Nasty.CreateCard ();

	}
	
	
	void GiveSecondaryObjectives() {
		Player p = Player.MyPlayer.GetComponent<Player>();
		GameObject secondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;

		if (p.GetPlayerClass().Equals("Scout Class")) {
			Debug.Log("Adding scout secondary");
			secondaries.AddComponent<ScoutSecondaryOne>();
		} else {
			Debug.Log("Not scout");
		}
	}
}
