﻿using UnityEngine;
using System.Collections;

public class FourthObjective : PrimaryObjective {
	public FourthObjective() {
		Title = "Fourth Objective";
		Description = 
			"Fourth objective description";
		Location = Tile.TilePosition(11.89f, -8.13f);
		NextObjective = new FifthObjective();
	}
}