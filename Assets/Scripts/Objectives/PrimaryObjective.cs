using UnityEngine;
using System.Collections;

public abstract class PrimaryObjective : Objective {
	protected Objective NextObjective;
	/* Doors can be optional which probably means there could be a PrimaryObjective subclass called 
	 * PrimaryObjectiveWithDoor but I think this way is better
	 */
	protected Tile Door;

	public override void OnComplete() {
		GameObject UI = GameObject.FindGameObjectWithTag("Objective UI");
		UI.GetComponent<PrimaryObjectiveController>().ChangeObjective(NextObjective);

		if (Door != null)
			gameManager.OpenDoor(Door);
	}
}
