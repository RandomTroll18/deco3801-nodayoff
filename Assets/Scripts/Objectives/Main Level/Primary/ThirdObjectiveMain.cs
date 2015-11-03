using UnityEngine;
using System.Collections;

/*
 * The last thing before escape. Location is positioned as far from the escape pods as possible
 */
public class ThirdObjectiveMain : PrimaryObjective {

	public void StartMe() {
		Title = "Direct Power";
		Description = "All power needs to be directed to the escape pods. Get to the bridge and " +
			"use the console." + StringMethodsScript.NEWLINE +
			"An Engineer can activate this console faster than everyone else.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 3")
				Location = Tile.TilePosition(objective.transform.position);
		}
	}

	public override void OnComplete() {
		FourthObjectiveMain obj = gameObject.AddComponent<FourthObjectiveMain>();
		obj.StartMe();
		NextObjective = obj;
		base.OnComplete();
	}
}
