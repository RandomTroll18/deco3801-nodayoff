using UnityEngine;
using System.Collections;

public class SecondObjective : PrimaryObjective {
	public SecondObjective () {
		Title = "Second Objective";
		Description = "Second objective description";
		Location = Tile.TilePosition(-0, -0);
	}
}
