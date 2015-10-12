using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 * This is used by external things that are intersted in a player's secondary objectives.
 * The system should be pretty simple:
 *  - This class has a bunch of secondary objectives
 *  - Anything with access to this class can request a secondary to complete. More complicated
 *    checking for when an objective is complicated can be implemented wherever.
 *  - This class will complete that secondary and apply its effects.
 */
public class SecondaryObjectiveManager : MonoBehaviour {
	List<SecondaryObjective> objectives;

	public void AddObjective(SecondaryObjective objective) {
		objectives.Add(objective);
	}

	/*
	 * Pass in the title of the objective to complete it.
	 * Probably not the best way to do things but it's pretty easy to understand.
	 */
	public void CompleteObjective(string objectiveName) {

	}

	public void CompleteObjective(SecondaryObjective objective) {
		objectives.Remove(objective);
		objective.OnComplete();
	}
}
