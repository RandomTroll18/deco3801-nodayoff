//using System;
using UnityEngine;

public class TutorialThirdObjective : PrimaryObjective {
	
	public TutorialThirdObjective () {
		Title = "Third Objective";
		Description = "Oh god I don't remember a door here..." +
			" But I do remember how to force it open.";
		Location = Tile.TilePosition(-12f, -1.65f);
		NextObjective = new TutorialFourthObjective();
		Door = Tile.TilePosition(-10f, -4.3f);
	}
}