using System.Collections;
using UnityEngine;

/*
 * First Tutorial Objective - Interacting with Doors
 */
public class TutorialFirstObjective : PrimaryObjective {
	
	void Start() {
		Title = "First Objective";
		Description = "Ughhh ... What happened? I don't know what is going on but lets find the others. " +
			"I got to get out of this room...";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Door")) {
			if (objective.name == "LockDoor1") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = Object.FindObjectOfType<TutorialSecondObjective>();
	}
}
