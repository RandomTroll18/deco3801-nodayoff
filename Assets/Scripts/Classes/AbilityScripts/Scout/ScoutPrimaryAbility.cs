using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoutPrimaryAbility : Ability {

	string trapPrefab; // Prefab for a trap
	int trapCount; // The number of traps that can be made
	const int MAX_TRAPS = 10; // The maximum amount of traps for the scout

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The calling player
	 */
	public ScoutPrimaryAbility() {
		AbilityName = "Traps";
		Range = 3.0; // Range of 3 traps
		AbilityRangeType = RangeType.SQUARERANGE; // Square shaped
		AbilityActivationType = ActivationType.DEFENSIVE; // Defensive ability
		trapPrefab = "Prefabs/AbilityPrefabs/Scout/ScoutTrap";
		AbilityIdentifier = AbilityEnum.SCOABI; // This is the scout's ability
	}

	/**
	 * Activate function
	 * 
	 * Arguments
	 * - Tile targetTile - The tile being targetted
	 */
	public override void Activate(Tile targetTile) {
		GameObject primaryAbilityButton; // The primary ability button

		primaryAbilityButton = ClassPanel.GetComponent<ClassPanelScript>().PrimaryAbilityButton;

		/* Count the number of traps we have */
		trapCount = MAX_TRAPS;
		foreach (Trap t in Object.FindObjectsOfType<Trap>()) {
			PhotonView pv = t.GetComponent<PhotonView>();
			if (pv != null && pv.isMine) // This is our trap
				trapCount--;
		}

		if (trapCount == 0) { // Don't do anything. Out of traps
			primaryAbilityButton.SetActive(false);
			return;
		}

		primaryAbilityButton.SetActive(true); // We still have traps
		SpawnTrap(Tile.TileMiddle(targetTile).x, 0, Tile.TileMiddle(targetTile).z);

		if ((trapCount - 1) == 0) // Out of traps
			primaryAbilityButton.SetActive(false);
	}

	/**
	 * Spawn the trap
	 * 
	 * Arguments
	 * - float x - x coordinate
	 * - float y - y coordinate
	 * - float z - z coordinate
	 */
	void SpawnTrap(float x, float y, float z) {
		GameObject generatedTrap; // The generated trap
		Vector3 pos = new Vector3(x, y, z); // The position of this trap
		generatedTrap = PhotonNetwork.Instantiate(trapPrefab, pos, Quaternion.identity, 0); // Instantiate
		generatedTrap.GetComponent<ScoutTrapScript>().SetOwner(Player.MyPlayer); // Set the owner
	}

}
