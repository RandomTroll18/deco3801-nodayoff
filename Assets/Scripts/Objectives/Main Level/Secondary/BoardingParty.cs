using UnityEngine;
using System.Collections;

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

		Object.FindObjectOfType<GameManager>().IncreaseRoundsLost(ROUNDS_LOST);

		ChangeRoundsPerTurn nasty = gameObject.AddComponent<ChangeRoundsPerTurn>();
		nasty.roundsLost = ROUNDS_LOST;
		GameObject NastyUI = nasty.CreateCard();

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
		Description = "Take out the boarding party.";
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
