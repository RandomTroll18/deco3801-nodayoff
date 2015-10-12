//using System;
using UnityEngine;

public class TutorialFourthObjective : PrimaryObjective {
	
	public TutorialFourthObjective () {
		Title = "Fourth Objective";
		Description = "Uhhhhh ... This door definately doesn't look like something I can force open." +
			" Maybe if I hit some of these keys something might happen.";
		Location = Tile.TilePosition(-12f, -1.65f);
		NextObjective = new TutorialFinalObjective();
		Door = Tile.TilePosition(-10f, -4.3f);
	}
}