using System;
using UnityEngine;

public class FirstObjective : PrimaryObjective {

	public FirstObjective () {
		Title = "First Objective";
		Description = "First objective description";
		Location = Tile.TilePosition(-12f, -1.65f);
		NextObjective = new SecondObjective();
		Door = Tile.TilePosition(-10f, -4.3f);
	}
}