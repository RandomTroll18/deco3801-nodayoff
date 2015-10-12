using System.Collections;
using UnityEngine;

public class TutorialFirstObjective : PrimaryObjective {
	
	void Start() {
		Title = "First Objective";
		Description = "Ughhh ... What happened? I don't know what is going on but lets find the others. " +
			"I got to get out of this room...";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 1") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = Object.FindObjectOfType<TutorialSecondObjective>();
	}
}
