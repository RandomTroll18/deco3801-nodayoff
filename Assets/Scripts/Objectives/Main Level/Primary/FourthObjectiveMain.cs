using UnityEngine;
using System.Collections;

/*
 * Getting to one escape pod. There are 4 escape pods and each can only hold one player. So if an
 * escape pod is lost, one human must lose.
 */
public class FourthObjectiveMain : PrimaryObjective {

	// Use this for initialization
	public void StartMe() {
		Title = "Escape";
		Description = "The escape pods are ready. Get to them before our ship is destroyed!\n" +
			"There is no sharing of escape pods - once one escape pod is taken, it's gone forever.";

		Location = Tile.TilePosition(GameObject.Find("EscapePod 1").transform.position);
		Location2 = Tile.TilePosition(GameObject.Find("EscapePod 3").transform.position);
	}

	public override void OnComplete() {
		Application.LoadLevel("Win Screen");
	}

}
