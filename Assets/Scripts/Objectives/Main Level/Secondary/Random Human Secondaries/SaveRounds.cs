using UnityEngine;
using System.Collections;

/*
 * Player has noticed a problem. Solve to stop rounds from being lost.
 */
public class SaveRounds : SecondaryObjective {
	const int ROUNDS_LOST = 1;

	void Start() {
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("IncreaseRoundsLost", PhotonTargets.All, ROUNDS_LOST);
		GameObject objective = PickHumanObjective();
		ObjectiveName = "TechnicianSecondaryOne";
		Title = "Repair the Ship";
		Description = "Part of the ship is damaged. Repair the ship to prevent the round counter " +
			"from going down an extra round per turn.";
		Location = Tile.TilePosition(objective.transform.position);
		
		SaveRoundInteractable i = objective.AddComponent<SaveRoundInteractable>();
		i.InstantInteract = true;
		i.StartMe();
	}
	
	public override void OnComplete() {
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("DecreaseRoundsLost", PhotonTargets.All, ROUNDS_LOST);
		Destroy(this);
		base.OnComplete();
	}
}
