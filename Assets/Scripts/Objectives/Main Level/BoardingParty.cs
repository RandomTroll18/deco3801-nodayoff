using UnityEngine;
using System.Collections;

public class BoardingParty : SecondaryObjective {

	Effect visionEffect; // The vision effect
	
	void Start() {
		teamObjective = true;

		/*
		 * Only the master needs to spawn in the boarding photon.
		 */
		if (PhotonNetwork.isMasterClient) {
			Vector3 pos = Tile.TileMiddle(
				Tile.TilePosition(GameObject.Find("Boarding Party").transform.position));
			PhotonNetwork.Instantiate("Main Level Interactables/Boarding Party",
			                          pos,
			                          Quaternion.identity,
			                          0);
		}

		ObjectiveName = "Boarding";
		Title = "Boarding Party";
		Description = "Take out the boarding party.";
		GameObject objective = GameObject.Find("Boarding Party");
		Location = Tile.TilePosition(objective.transform.position);
		
		ScoutSecondaryOneInteractable i = 
			GameObject.Find("Scout Secondary One").AddComponent<ScoutSecondaryOneInteractable>();
		i.InstantInteract = true;
		i.StartMe();
		
		visionEffect = new StatusTurnEffect(Stat.VISION, 3.0, 1, 
		                                    "Night Vision Goggles", "Icons/Effects/DefaultEffect", -1, true);
	}
	
	public override void OnComplete() {
		/*
		 * functionality is handled in I1BoardingParty.cs Sync()
		 */
		Destroy(this);
	}
}
