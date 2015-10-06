using UnityEngine;
using System.Collections;

public class FirstObjectiveMain : PrimaryObjective {

	public FirstObjectiveMain() {
		Title = "Activate Auxillary Power";
		Description = "First objective description";
		Location = Tile.TilePosition(-12f, 14.50034f);
//		NextObjective = new SecondObjective();
//		Door = Tile.TilePosition(-10f, -4.3f);
	}
}
