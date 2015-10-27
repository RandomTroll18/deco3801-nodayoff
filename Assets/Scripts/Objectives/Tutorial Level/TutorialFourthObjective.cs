using System.Collections;
using UnityEngine;

public class TutorialFourthObjective : PrimaryObjective {
	
	void Start() {
		Title = "Fourth Objective";
		Description = "Oh this is definately something I'm good at. This should be easy";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 1") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = Object.FindObjectOfType<TutorialFinalObjective>();

	}
}