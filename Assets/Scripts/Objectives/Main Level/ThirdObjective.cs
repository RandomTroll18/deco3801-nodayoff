using UnityEngine;
using System.Collections;

public class ThirdObjective : PrimaryObjective {
	public ThirdObjective() {
		Title = "Third Objective";
		Description = 
			"Third objective description\n" +
			"Force open the door";
		Location = Tile.TilePosition(10.416f, -4.507f);
		NextObjective = new FourthObjective();
		Door = Location;
	}
}
