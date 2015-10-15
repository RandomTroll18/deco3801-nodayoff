using UnityEngine;
using System.Collections;

public class TechnicianSecondaryOne : SecondaryObjective {

	void Start() {
		GameObject objective = GameObject.Find("Technician Secondary One");
		ObjectiveName = "TechnicianSecondaryOne";
		Title = "Find the deployable surveillance cameras";
		Description = "REWARD: Deployable Surveillance Cameras.";
		Location = Tile.TilePosition(objective.transform.position);
		
		TechnicianSecondaryOneInteractable i = 
			GameObject.Find("Technician Secondary One").AddComponent<TechnicianSecondaryOneInteractable>();
		i.InstantInteract = true;
		i.StartMe();
	}
	
	public override void OnComplete() {
		PhotonNetwork.Instantiate(
			"Prefabs/SurveillanceCamera", 
			Player.MyPlayer.transform.position, 
			Quaternion.identity, 
			0
			);
		Destroy(this);
	}
}
