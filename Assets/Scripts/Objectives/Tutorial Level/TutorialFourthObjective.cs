using System.Collections;
using UnityEngine;

public class TutorialFourthObjective : PrimaryObjective {
	
	void Start() {
		Title = "Fourth Objective";
		Description = "Uhhhhh ... This door definately doesn't look like something I can force open." +
			" Maybe if I hit some of these keys something might happen.";
		Location = Tile.TilePosition(-12f, -1.65f);
		NextObjective = Object.FindObjectOfType<TutorialFinalObjective>();

	}
}