using UnityEngine;
using System.Collections;

/*
 * Objectives shown in the panel to the right. These should be mandatory objectives to win.
 */
public abstract class PrimaryObjective : Objective {
	protected Objective NextObjective;
	protected Tile Door; /* Door to open. Doors can be optional */

	public override void OnComplete() {
		GameObject UI = GameObject.FindGameObjectWithTag("Objective UI");
		UI.GetComponent<PrimaryObjectiveController>().ChangeObjective(NextObjective);

		if (Door != null)
			gameManager.OpenDoor(Door);
	}
}
