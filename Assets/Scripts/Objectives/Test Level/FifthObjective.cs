using UnityEngine;
using System.Collections;

public class FifthObjective : PrimaryObjective {
	public FifthObjective() {
		Title = "Fifth Objective";
		Description = 
			"Fifth objective description";
		Location = Tile.TilePosition(-28.28f, -12.05f);
		NextObjective = new SixthObjective();
	}
}
