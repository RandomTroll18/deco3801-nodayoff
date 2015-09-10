using System;

public class FirstObjective : Objective {
	public FirstObjective () {
		Title = "First Objective";
		Description = "First objective description";
		Location = new Tile(10, 10);
	}

	public override void OnComplete() {
		;
	}
}

