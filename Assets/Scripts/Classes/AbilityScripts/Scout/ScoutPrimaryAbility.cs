using UnityEngine;
using System.Collections;

public class ScoutPrimaryAbility : Ability {

	Player master; // The owner of this ability
	GameObject trapPrefab; // Prefab for a trap
	int trapCount; // The number of traps that can be made

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The calling player
	 */
	public ScoutPrimaryAbility(Player player) {
		AbilityName = "Traps";
		Range = 3.0;
		AbilityRangeType = RangeType.SQUARERANGE;
		AbilityActivationType = ActivationType.DEFENSIVE;
		trapPrefab = Resources.Load<GameObject>("AbilityPrefabs/Scout/ScoutTrap");
		master = player;
		trapCount = 3; // Only 3 traps for now
	}

	/**
	 * Activate function
	 * 
	 * Arguments
	 * - Tile targetTile - The tile being targetted
	 */
	public override void Activate(Tile targetTile)
	{
		GameObject generatedTrap; // The generated trap

		if (trapCount == 0) return; // Don't do anything. Out of traps
		trapCount--;
		if (trapCount == 0) IsActive = true; // No more traps

		generatedTrap = Object.Instantiate<GameObject>(trapPrefab);
		generatedTrap.GetComponent<ScoutTrapScript>().SetOwner(master); // Set the owner
		generatedTrap.GetComponent<ScoutTrapScript>().SetReference(generatedTrap);
		generatedTrap.GetComponent<Transform>().position = Tile.TileMiddle(targetTile);
	}

}
