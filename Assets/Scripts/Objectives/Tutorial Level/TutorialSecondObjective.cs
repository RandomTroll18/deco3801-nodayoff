using System.Collections;
using UnityEngine;

public class TutorialSecondObjective : PrimaryObjective {
	
	void Start() {
		Title = "Second Objective";
		Description = "Hmm... I got a bad feeling about this..." +
			" I think there is a Stun gun in the rooms near by.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Trap")) {
			if (objective.name == "Stungun Trap") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = Object.FindObjectOfType<TutorialThirdObjective>();
	}
}