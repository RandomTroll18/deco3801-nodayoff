using UnityEngine;
using System.Collections;

public class FirstObjectiveMain : PrimaryObjective {

	void Start() {
		Title = "Activate Auxillary Power";
		Description = "Main power sources have taken heavy damage.\n" +
			"A Technician is needed.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "LockDoor1") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = Object.FindObjectOfType<SecondObjectiveMain>();
//		Door = Tile.TilePosition(-10f, -4.3f);
	}
}
