using UnityEngine;
using System.Collections;

/*
 * The last thing before escape. As far from the escape pods as possible
 */
public class ThirdObjectiveMain : PrimaryObjective {

	// Use this for initialization
	public void StartMe() {
		Title = "Direct Power";
		Description = "All power needs to be directed to the escape pods. Get to the bridge and " +
			"use the console." + StringMethodsScript.NEWLINE +
			"An Engineer can activate this console faster than everyone else.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 3")
				Location = Tile.TilePosition(objective.transform.position);
		}

//		foreach(GameObject escapePod in GameObject.FindGameObjectsWithTag("Escape Pod")) {
//			MainLevelObjective4 i = escapePod.AddComponent<MainLevelObjective4>();
//			i.StartMe();
//		}
	}

	public override void OnComplete() {
		FourthObjectiveMain obj = gameObject.AddComponent<FourthObjectiveMain>();
		obj.StartMe();
		NextObjective = obj;
		base.OnComplete();
	}
}
