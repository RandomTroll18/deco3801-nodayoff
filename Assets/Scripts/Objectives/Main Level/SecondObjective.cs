using UnityEngine;
using System.Collections;

public class SecondObjective : PrimaryObjective {
	public SecondObjective () {
		Title = "Second Objective";
		Description = "Second objective description";
		Location = Tile.TilePosition(-12.15f, -8.18f);
		NextObjective = new ThirdObjective();
	}

	public override void OnComplete() {
		base.OnComplete();
		// TODO: trigger an event
	}
}
