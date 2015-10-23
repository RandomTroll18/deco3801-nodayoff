using UnityEngine;
using System.Collections;

public class AutomaticRepairDrone : SupportConsumables {

	/**
	 * On start, do the following:
	 * - Set item description
	 * - Set amount recovered
	 * - The stat affected
	 */
	void Start() {
		ItemDescription = "Repair Drone that can delay your death by a few turns";

		Range = 0; // No range. Instant use
		Activatable = true; // Can be activated
		Droppable = true;
	}

	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override string ToString()
	{
		string toReturn = "Description: " + ItemDescription + StringMethodsScript.NEWLINE; // The string to return

		toReturn += "Amount: " + Amount + StringMethodsScript.NEWLINE;

		return toReturn;
	}

	/* Compiler pacifying */
	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(Tile targetTile)
	{
		throw new System.NotImplementedException();
	}
}
