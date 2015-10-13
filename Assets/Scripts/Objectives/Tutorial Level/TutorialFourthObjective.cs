using System.Collections;
using UnityEngine;

public class TutorialFourthObjective : PrimaryObjective {
	
	void Start() {
		Title = "Fourth Objective";
		Description = "Uhhhhh ... This door definately doesn't look like something I can force open." +
			" Maybe if I hit some of these keys something might happen.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 1") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = Object.FindObjectOfType<TutorialFinalObjective>();

	}
}