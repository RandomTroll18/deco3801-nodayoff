using UnityEngine;
using System.Collections;

/**
 * Likely won't be used for the MVP but I'm keeping secondary objectives in mind.
 */
public class SecondaryObjective : Objective {
	public string ObjectiveName; // Used to ID objectives
	protected bool teamObjective;

	public override void OnComplete() {
		if (teamObjective) {
			Debug.Log("Team objective completed");
		}
	}

}
