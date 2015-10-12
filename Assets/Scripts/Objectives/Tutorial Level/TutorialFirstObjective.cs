using System;
using UnityEngine;

public class TutorialFirstObjective : PrimaryObjective {
	
	public TutorialFirstObjective () {
		Title = "First Objective";
		Description = "Ughhh ... What happened? I don't know what is going on but lets find the others. " +
			"I got to get out of this room...";
		Location = Tile.TilePosition(-12f, -1.65f);
		NextObjective = new TutorialSecondObjective();
		Door = Tile.TilePosition(-10f, -4.3f);
	}
}