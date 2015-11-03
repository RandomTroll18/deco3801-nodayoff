using System.Collections;
using UnityEngine;

/*
 * Fourth Tutorial Objective - Advantageous class multiplier
 */
public class TutorialFourthObjective : PrimaryObjective {
	
	void Start() {
		// Initialize Title and Description
		Title = "Fourth Objective";
		Description = "Oh this is definately something I'm good at. This should be easy";


		// Initialize Objective Location
		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 1") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		// Initialize Next Objective
		NextObjective = Object.FindObjectOfType<TutorialFinalObjective>();

	}
}