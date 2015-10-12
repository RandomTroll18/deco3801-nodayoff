//using System;
using UnityEngine;

public class TutorialSecondObjective : PrimaryObjective {
	
	public TutorialSecondObjective () {
		Title = "Second Objective";
		Description = "Hmm... I got a bad feeling about this..." +
			" I think there is a Stun gun in the rooms near by.";
		Location = Tile.TilePosition(-12f, -1.65f);
		NextObjective = new TutorialThirdObjective();
		Door = Tile.TilePosition(-10f, -4.3f);
	}
}