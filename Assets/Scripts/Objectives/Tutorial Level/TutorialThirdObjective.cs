using System.Collections;
using UnityEngine;

public class TutorialThirdObjective : PrimaryObjective {
	
	void Start() {
		Title = "Third Objective";
		Description = "Oh god I don't remember a door here..." +
			" Looks like I can force it open thou.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Door")) {
			if (objective.name == "Broken Door") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = Object.FindObjectOfType<TutorialFourthObjective>();
	}
}