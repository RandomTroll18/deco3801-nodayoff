using UnityEngine;
using System.Collections;

public class FirstObjectiveMain : PrimaryObjective {

	public FirstObjectiveMain() {
		Title = "Activate Auxillary Power";
		Description = "Main power sources have taken heavy damage.\n" +
			"A Technician is needed.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 1") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

		NextObjective = new SecondObjectiveMain();
//		Door = Tile.TilePosition(-10f, -4.3f);
	}
}
