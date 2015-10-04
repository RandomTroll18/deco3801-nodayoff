using UnityEngine;
using System.Collections;

public class ScoutPrimaryAbility : Ability {

	Player master; // The owner of this ability
	string trapPrefab; // Prefab for a trap
	int trapCount; // The number of traps that can be made
	const int MAX_TRAPS = 3;

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
		trapPrefab = "AbilityPrefabs/Scout/ScoutTrap";
		master = player;
	}

	/**
	 * Activate function
	 * 
	 * Arguments
	 * - Tile targetTile - The tile being targetted
	 */
	public override void Activate(Tile targetTile) {
		trapCount = MAX_TRAPS;
		foreach (Trap t in Object.FindObjectsOfType<Trap>()) {
			PhotonView pv = t.GetComponent<PhotonView>();
			if (pv != null && pv.isMine) {
				trapCount--;
			}
		}

		if (trapCount == 0) 
			return; // Don't do anything. Out of traps

		SpawnTrap(Tile.TileMiddle(targetTile).x, 0, Tile.TileMiddle(targetTile).z);
	}

	void SpawnTrap(float x, float y, float z) {
		GameObject generatedTrap;
		Vector3 pos = new Vector3(x, y, z);
		generatedTrap = PhotonNetwork.Instantiate(trapPrefab, pos, Quaternion.identity, 0);
		generatedTrap.GetComponent<MeshRenderer>().enabled = true;
	}

}
