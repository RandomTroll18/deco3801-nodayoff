﻿using UnityEngine;
using System.Collections;

public class TutorialFinalObjective : PrimaryObjective {
	public TutorialFinalObjective() {
		Title = "Final Objective";
		Description = 
			"Find the others";
		Location = Tile.TilePosition(-0.4f, 12.41f);
	}
	
	public override void OnComplete() {
		Application.LoadLevel("WinScreen");
	}
}
