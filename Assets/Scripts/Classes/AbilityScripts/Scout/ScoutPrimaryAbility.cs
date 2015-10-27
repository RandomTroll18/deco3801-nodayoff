using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoutPrimaryAbility : Ability {

	string trapPrefab; // Prefab for a trap
	int trapCount; // The number of traps that can be made
	const int MAX_TRAPS = 10;

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The calling player
	 */
	public ScoutPrimaryAbility() {
		AbilityName = "Traps";
		Range = 3.0;
		AbilityRangeType = RangeType.SQUARERANGE;
		AbilityActivationType = ActivationType.DEFENSIVE;
		trapPrefab = "Prefabs/AbilityPrefabs/Scout/ScoutTrap";
		AbilityIdentifier = AbilityEnum.SCOABI;
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
		trapCount = MAX_TRAPS;
		foreach (Trap t in Object.FindObjectsOfType<Trap>()) {
			PhotonView pv = t.GetComponent<PhotonView>();
			if (pv != null && pv.isMine)
				trapCount--;
		}

		if (trapCount == 0) { // Don't do anything. Out of traps
			primaryAbilityButton.SetActive(false);
			return; // Don't do anything. Out of traps
		}

		primaryAbilityButton.SetActive(true); // We still have traps
		SpawnTrap(Tile.TileMiddle(targetTile).x, 0, Tile.TileMiddle(targetTile).z);

		if ((trapCount - 1) == 0) // Out of traps
			primaryAbilityButton.SetActive(false);
	}

	void SpawnTrap(float x, float y, float z) {
		GameObject generatedTrap;
		Vector3 pos = new Vector3(x, y, z);
		generatedTrap = PhotonNetwork.Instantiate(trapPrefab, pos, Quaternion.identity, 0);
		generatedTrap.GetComponent<ScoutTrapScript>().SetOwner(Player.MyPlayer);

	}

}
