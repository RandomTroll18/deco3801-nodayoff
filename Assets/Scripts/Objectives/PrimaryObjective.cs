using UnityEngine;
using System.Collections;

public abstract class PrimaryObjective : Objective {
	protected Objective NextObjective;
	protected Tile Door;

	public override void OnComplete() {
		GameObject UI = GameObject.FindGameObjectWithTag("Objective UI");
		UI.GetComponent<PrimaryObjectiveController>().ChangeObjective(NextObjective);
		if (Door != null) {
			gameManager.OpenDoor(Door);
		}
	}
}
