using UnityEngine;
using System.Collections;

public class SixthObjective : PrimaryObjective {
	public SixthObjective() {
		Title = "Final Objective";
		Description = 
			"Escape the ship";
		Location = Tile.TilePosition(-0.4f, 12.41f);
	}

	public override void OnComplete() {
		// TODO: finish level
	}
}
