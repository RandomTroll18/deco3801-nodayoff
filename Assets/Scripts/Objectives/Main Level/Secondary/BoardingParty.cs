using UnityEngine;
using System.Collections;

/*
 * Summons a boarding party in a random location for players to remove
 */
public class BoardingParty : SecondaryObjective {
	public const int ROUNDS_LOST = 1;


	Effect visionEffect; // The vision effect

	public override void InitializeObjective() {
		base.InitializeObjective();
		Start();
	}

	void Start() {
		Log();
		teamObjective = true;
		GetComponent<PhotonView>().RPC("IncreaseRoundsLost", PhotonTargets.All, ROUNDS_LOST);

		/*
		 * This is a variable used to sync the random spawn location
		 */
		Vector3 pos = Object.FindObjectOfType<GameManager>().BoardingSpawn;


		/*
		 * Only the master needs to spawn in the boarding photon.
		 */
		if (PhotonNetwork.isMasterClient) {

			pos = Tile.TileMiddle(
				Tile.TilePosition(pos));
			PhotonNetwork.Instantiate("Main Level Interactables/Boarding Party",
			                          pos,
			                          Quaternion.identity,
			                          0);
		}

		ObjectiveName = "Boarding";
		Title = "Boarding Party";
		Description = "TEAM OBJECTIVE\n" +
			"Destroy the boarding party. Whilst the boarding party is alive, an extra round is lost" +
			" per turn.";
		Location = Tile.TilePosition(pos);
		
//		ScoutSecondaryOneInteractable i = 
//			GameObject.Find("Scout Secondary One").AddComponent<ScoutSecondaryOneInteractable>();
//		i.InstantInteract = true;
//		i.StartMe();
//		
//		visionEffect = new StatusTurnEffect(Stat.VISION, 3.0, 1, 
//		                                    "Night Vision Goggles", "Icons/Effects/DefaultEffect", -1, true);
	}
	
	public override void OnComplete() {
		/*
		 * functionality is handled in I1BoardingParty.cs Sync()
		 */
		Destroy(this);
	}
}
