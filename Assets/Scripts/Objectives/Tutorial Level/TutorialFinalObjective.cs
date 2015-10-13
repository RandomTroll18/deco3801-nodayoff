using UnityEngine;
using System.Collections;

public class TutorialFinalObjective : PrimaryObjective {

	void Start() {
		Title = "Final Objective";
		Description = 
			"Find the others";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Trap")) {
			if (objective.name == "Finish Trap") {
				Location = Tile.TilePosition(objective.transform.position);
			}
		}

	}
	
	public override void OnComplete() {
		Application.LoadLevel("WinScreen");
	}
}
