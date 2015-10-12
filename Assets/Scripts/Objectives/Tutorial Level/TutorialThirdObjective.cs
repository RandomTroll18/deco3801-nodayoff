using System.Collections;
using UnityEngine;

public class TutorialThirdObjective : PrimaryObjective {
	
	void Start() {
		Title = "Third Objective";
		Description = "Oh god I don't remember a door here..." +
			" But I do remember how to force it open.";
		Location = Tile.TilePosition(-12f, -1.65f);
		NextObjective = Object.FindObjectOfType<TutorialFourthObjective>();
	}
}