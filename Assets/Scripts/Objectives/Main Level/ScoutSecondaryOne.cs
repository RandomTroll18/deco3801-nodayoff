using UnityEngine;
using System.Collections;

public class ScoutSecondaryOne : SecondaryObjective {

	void Start() {
		ObjectiveName = "ScoutSecondaryOne";
		Title = "Find the night vision goggles";
		Description = "REWARD: extra vision.\n" +
			"An Engineer will be helpful";

		GameObject.Find("Scout Secondary One").AddComponent<ScoutSecondaryOneInteractable>().StartMe();
	}

	public override void OnComplete() {
		// not really tested
		// applying a turn effect would be nicer
		Player.MyPlayer.GetComponent<Player>().SetStatValue(Stat.VISION, 3);
		Destroy(this);
	}
}
