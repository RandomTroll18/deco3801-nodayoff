using UnityEngine;
using System.Collections;

public class ScoutSecondaryOne : SecondaryObjective {

	public ScoutSecondaryOne() {
		Title = "Find the night vision goggles.";
		Description = "REWARD: extra vision";
	}

	public void ApplyReward() {
		// not really tested
		Player.MyPlayer.GetComponent<Player>().IncreaseStatValue(Stat.VISION, 3);
	}
}
