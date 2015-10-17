using UnityEngine;
using System.Collections;

public class ScoutSecondaryOne : SecondaryObjective {

	Effect visionEffect; // The vision effect

	void Start() {
		ObjectiveName = "ScoutSecondaryOne";
		Title = "Find the night vision goggles";
		Description = "REWARD: extra vision.\n" +
			"An Engineer will be helpful";
		GameObject objective = GameObject.Find("Scout Secondary One");
		Location = Tile.TilePosition(objective.transform.position);

		ScoutSecondaryOneInteractable i = 
			GameObject.Find("Scout Secondary One").AddComponent<ScoutSecondaryOneInteractable>();
		i.InstantInteract = true;
		i.StartMe();

		visionEffect = new StatusTurnEffect(Stat.VISION, 3.0, 1, 
				"Night Vision Goggles", "Icons/Effects/DefaultEffect", -1, true);
	}

	public override void OnComplete() {
		Player.MyPlayer.GetComponent<Player>().AttachTurnEffect(visionEffect);
		Player.MyPlayer.GetComponent<Player>().SetStatValue(Stat.VISION, 3);
		Destroy(this);
	}
}
