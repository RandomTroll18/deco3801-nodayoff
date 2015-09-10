using UnityEngine;
using System.Collections;

public class EngineerPrimaryAbility : Ability {

	GameObject robotPrefab; // The robot prefab
	GameObject robotReference; // Reference to the instantiated robot

	/**
	 * Constructor
	 */
	public EngineerPrimaryAbility() {
		AbilityName = "Block-Buster";
		Range = 2.0;
		AbilityRangeType = RangeType.SQUARERANGE;
		AbilityActivationType = ActivationType.SUPPORTIVE;
		robotPrefab = Resources.Load<GameObject>("AbilityPrefabs/EngineerRobot");
		Debug.Log("Robot Prefab: " + robotPrefab.ToString());
	}

	/**
	 * Override activate function
	 * 
	 * Arguments
	 * - Tile tileClicked - The tile that was clicked
	 */
	public override void Activate(Tile targetTile) {
		base.Activate(targetTile);

		// Instantiate robot
		robotReference = Object.Instantiate(robotPrefab);
		robotReference.GetComponent<Transform>().position = Tile.TileMiddle(targetTile);
	}

	/**
	 * Override reduce number of turns function
	 */
	public override void ReduceNumberOfTurns() {
		base.ReduceNumberOfTurns();
		if (RemainingTurns == 0 && robotReference != null) {
			Object.Destroy(robotReference);
			robotReference = null;
		}
	}
}
