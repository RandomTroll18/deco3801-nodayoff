using UnityEngine;
using System.Collections;

/*
 * Marine activates a console to get their stun turret. Refer to the stun turret prefab for more.
 */
public class MarineSecondaryOne : SecondaryObjective {

	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}

	void Start() {
		Log();
		ObjectiveName = "MarineSecondaryOne";
		Title = "Stun Turret";
		Description = "Find the deployable stun turrets. A Technician could open the door quicker" +
			"for you.\n" +
			"REWARD: Deployable Stun Turrets.";
		GameObject objective = GameObject.FindGameObjectWithTag("Marine Secondary");
		Location = Tile.TilePosition(objective.transform.position);
		
		MarineSecondaryOneInteractable i = 
			GameObject.Find("Marine Secondary One").AddComponent<MarineSecondaryOneInteractable>();
		i.InstantInteract = true;
		i.StartMe();
	}
	
	public override void OnComplete() {
		PhotonNetwork.Instantiate(
				"Prefabs/ItemPrefabs/DeployableStunTurret(1)", 
				Player.MyPlayer.transform.position, 
				Quaternion.identity, 
				0
		);
		Destroy(this);
		base.OnComplete();
	}
}
