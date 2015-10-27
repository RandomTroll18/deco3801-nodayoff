using UnityEngine;
using System.Collections;

public class TutorialFinalObjective : PrimaryObjective {

	void Start() {
		Title = "Final Objective";
		Description = 
			"Uhh ... I didn't mean to Destroy that door. " +
			"Whatever. I better get into the elevator, maybe I can find the others at another levels";

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
