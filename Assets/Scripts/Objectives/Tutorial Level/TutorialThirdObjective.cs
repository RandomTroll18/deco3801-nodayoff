using System.Collections;
using UnityEngine;

public class TutorialThirdObjective : PrimaryObjective {
	
	void Start() {
		Title = "Third Objective";
		Description = "Oh god I don't remember a door here..." +
			" But I do remember how to force it open.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Door")) {
			if (objective.name == "Broken Door") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = Object.FindObjectOfType<TutorialFourthObjective>();
	}
}