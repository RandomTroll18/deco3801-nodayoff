using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EngineerPrimaryAbility : Ability {

	GameObject robotPrefab; // The robot prefab
	GameObject robotReference; // Reference to the instantiated robot
	Text primaryAbilityButtonText; // The text for the primary ability button

	Player master; // This robot's master

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player newMaster - The player that spawned this robot
	 */
	public EngineerPrimaryAbility(Player newMaster) {
		AbilityName = "Block-Buster";
		Range = 2.0;
		AbilityRangeType = RangeType.SQUARERANGE;
		AbilityActivationType = ActivationType.SUPPORTIVE;
		robotPrefab = Resources.Load<GameObject>("AbilityPrefabs/EngineerRobot");
		Debug.Log("Robot Prefab: " + robotPrefab.ToString());
		master = newMaster;
	}

	/**
	 * Override activate function
	 * 
	 * Arguments
	 * - Tile tileClicked - The tile that was clicked
	 */
	public override void Activate(Tile targetTile) {
		base.Activate(targetTile);

		// Set up references to stuff
		robotPrefab.GetComponent<Player>().GameManagerObject = master.GameManagerObject;
		robotPrefab.GetComponent<Player>().EffectBoxPanel = master.EffectBoxPanel;
		robotPrefab.GetComponent<Player>().StunGunPrefab = master.StunGunPrefab;
		robotPrefab.GetComponent<Player>().ClassPanel = master.ClassPanel;
		robotPrefab.GetComponent<Player>().ClassToSet = "Engineer Robot";
		robotPrefab.GetComponent<Player>().InventoryUI = null;
		robotPrefab.GetComponent<Player>().StunGunPrefab = null;
		robotPrefab.GetComponent<Player>().MainCanvas = master.MainCanvas;
		robotPrefab.GetComponent<Player>().APCounterPanel = master.SpawnAPCounterPanel;
		robotPrefab.GetComponent<Player>().APCounterText = master.SpawnAPCounterText;
		robotPrefab.GetComponent<Player>().APCounterPanel.SetActive(true);
		robotPrefab.GetComponent<Player>().IsSpawned = true;

		// Instantiate robot at correct position and get its reference
		robotReference = Object.Instantiate(robotPrefab);
		robotReference.GetComponent<Transform>().position = Tile.TileMiddle(targetTile);
		robotReference.GetComponent<Player>().PlayerObject = robotReference;

		// Set clas panel text to appropriate values
		master.ClassPanel.GetComponent<ClassPanelScript>().SetPrimaryAbilityButtonText("Toggle To Robot");
	}

	/**
	 * Override reduce number of turns function
	 */
	public override void ReduceNumberOfTurns() {
		base.ReduceNumberOfTurns();
		// Reset the robot's stats
		if (robotReference != null) // Robot has been generated
			robotReference.GetComponent<Player>().InitializeStats();
		if (RemainingTurns == 0 && robotReference != null) { 
			// Set UI elements of robot to be inactive
			robotReference.GetComponent<Player>().APCounterPanel.SetActive(false);

			// Reset class panel to refer to the master
			master.ClassPanel.GetComponent<ClassPanelScript>().ResetToMaster(master.PlayerObject);

			// Destroy the robot and forget its existence
			Object.Destroy(robotReference);
			robotReference = null;
		}
	}
}
