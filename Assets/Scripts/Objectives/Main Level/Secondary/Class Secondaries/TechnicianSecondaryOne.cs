using UnityEngine;
using System.Collections;

/*
 * Technician activates a console to get more cameras
 */
public class TechnicianSecondaryOne : SecondaryObjective {

	public override void InitializeObjective() {
		base.InitializeObjective();
		Start();
	}

	void Start() {
		Log();
		GameObject objective = GameObject.FindGameObjectWithTag("Tech Secondary");
		ObjectiveName = "TechnicianSecondaryOne";
		Title = "Find Cameras";
		Description = "Find your deployable surveillance cameras. \n" +
			"REWARD: Deployable Surveillance Cameras.";
		Location = Tile.TilePosition(objective.transform.position);
		
		TechnicianSecondaryOneInteractable i = 
			GameObject.Find("Technician Secondary One").AddComponent<TechnicianSecondaryOneInteractable>();
		i.InstantInteract = true;
		i.StartMe();
	}
	
	public override void OnComplete() {
		PhotonNetwork.Instantiate(
			"Prefabs/ItemPrefabs/UsableSurveillanceCam(3)", 
			Player.MyPlayer.transform.position, 
			Quaternion.identity, 
			0
		);
		Destroy(this);
		base.OnComplete();
	}
}
