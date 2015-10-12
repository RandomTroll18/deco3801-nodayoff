using System.Collections;
using UnityEngine;

public class TutorialSecondObjective : PrimaryObjective {
	
	void Start() {
		Title = "Second Objective";
		Description = "Hmm... I got a bad feeling about this..." +
			" I think there is a Stun gun in the rooms near by.";
		Location = Tile.TilePosition(-12f, -1.65f);
		NextObjective = Object.FindObjectOfType<TutorialThirdObjective>();
	}
}