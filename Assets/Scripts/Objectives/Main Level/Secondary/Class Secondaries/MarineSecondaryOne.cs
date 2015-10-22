using UnityEngine;
using System.Collections;

public class MarineSecondaryOne : SecondaryObjective {

	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}

	void Start() {
		Log();
		ObjectiveName = "MarineSecondaryOne";
		Title = "Find the deployable stun turrets";
		Description = "REWARD: Deployable Stun Turrets.";
		GameObject objective = GameObject.Find("Marine Secondary One");
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
		PickNewHumanObjective();
	}
}
