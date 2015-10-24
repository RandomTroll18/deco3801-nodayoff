using UnityEngine;
using System.Collections;

public class ScoutSecondaryOne : SecondaryObjective {

	Effect visionEffect; // The vision effect

	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}

	void Start() {
		Log();
		ObjectiveName = "ScoutSecondaryOne";
		Title = "Night Vision Goggles";
		Description = "Find your night vision goggle.An Engineer could open the door quicker" +
			"for you.\n" +
			"REWARD: Double vision distance.";
		GameObject objective = GameObject.FindGameObjectWithTag("Scout Secondary");
		Location = Tile.TilePosition(objective.transform.position);

		ScoutSecondaryOneInteractable i = 
			GameObject.Find("Scout Secondary One").AddComponent<ScoutSecondaryOneInteractable>();
		i.InstantInteract = true;
		i.StartMe();

		visionEffect = new StatusTurnEffect(Stat.VISION, 3.0, 1, 
				"Night Vision Goggles", "Icons/Items/googlesgreen", -1, true);
	}

	public override void OnComplete() {
		Player.MyPlayer.GetComponent<Player>().AttachTurnEffect(visionEffect);
		Player.MyPlayer.GetComponent<Player>().SetStatValue(Stat.VISION, 3);
		Destroy(this);
		base.OnComplete();
		PickNewHumanObjective();
	}
}
