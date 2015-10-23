using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EngineerPrimaryAbility : Ability {

	GameObject robotPrefab; // The robot prefab
	GameObject robotReference; // Reference to the instantiated robot
	int coolDown; // Cool down timer

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
		robotPrefab = Resources.Load<GameObject>("Prefabs/AbilityPrefabs/Engineer/EngineerRobot");
		master = newMaster;
		coolDown = 5; // 5 turn cooldown
	}

	/**
	 * Get robot reference
	 * 
	 * Returns
	 * - The reference to the robot
	 */
	public GameObject GetRobotReference() {
		return robotReference;
	}

	/**
	 * Set cool down
	 * 
	 * Arguments
	 * - int newCoolDown - The amount of turns this ability should cool down
	 */
	public void SetCoolDown(int newCoolDown) {
		coolDown = newCoolDown;
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
		robotPrefab.GetComponent<Player>().EffectBoxPanel = master.EffectBoxPanel;
		robotPrefab.GetComponent<Player>().StunGunPrefab = master.StunGunPrefab;
		robotPrefab.GetComponent<Player>().ClassToSet = "Engineer Robot";
		robotPrefab.GetComponent<Player>().InventoryUI = null;
		robotPrefab.GetComponent<Player>().StunGunPrefab = null;
		robotPrefab.GetComponent<Player>().IsSpawned = true;
		robotPrefab.GetComponent<Player>().GameManagerObject = master.GameManagerObject;
		robotPrefab.GetComponent<Player>().SetPlayerLight(master.GetPlayerLight());

		// Instantiate robot at correct position and get its reference
		robotReference = PhotonNetwork.Instantiate(
				"Prefabs/AbilityPrefabs/Engineer/EngineerRobot", 
				Tile.TileMiddle(targetTile), 
				Quaternion.identity, 
				0
		);
		robotReference.SetActive(true);
		robotReference.GetComponent<Transform>().position = Tile.TileMiddle(targetTile);
		robotReference.GetComponentInChildren<Light>().enabled = true;
		robotReference.GetComponent<Player>().PlayerObject = robotReference;
		robotReference.GetComponent<Player>().SetPlayerLight(master.GetPlayerLight());
		robotReference.AddComponent<MovementController>().StartMe();
		robotReference.GetComponentInChildren<CameraController>().StartMe();
		robotReference.GetComponentInChildren<CameraController>().ResetCamera();
		robotReference.GetComponent<Player>().StartMe(); // Initialize the player script of the robot

		// Set class panel text to appropriate values
		ClassPanel.GetComponent<ClassPanelScript>().SetPrimaryAbilityButtonText("Toggle To Robot");
		ClassPanel.GetComponent<ClassPanelScript>().AttachSpawnedToCounter(robotReference);
		RemainingTurns = 3; // Reset remaining turns
	}

	/**
	 * Override reduce number of turns function
	 */
	public override void ReduceNumberOfTurns() {
		ClassPanelScript classPanelScript = ClassPanel.GetComponent<ClassPanelScript>(); // The class panel script

		if (RemainingTurns != 0)
			RemainingTurns--;
		Debug.Log("Engineer ability remaining turns: " + RemainingTurns);
		Debug.Log("Engineer ability active?: " + IsActive);
		// Reset the robot's stats
		if (robotReference != null) // Robot has been generated
			robotReference.GetComponent<Player>().InitializeStats();
		if (RemainingTurns == 0) { 
			if (IsActive) { // Reset ability to go to cooldown
				if (robotReference != null) { // Only do things if robot exists
					// Reset class panel to refer to the master
					classPanelScript.ResetToMaster(master.PlayerObject);
					
					// Destroy the robot and forget its existence
					PhotonNetwork.Destroy(robotReference);
					Object.Destroy(robotReference);
					robotReference = null;
					RemainingTurns = coolDown;
					IsActive = false;
					classPanelScript.PrimaryAbilityButton.GetComponent<Button>().interactable = false;
					classPanelScript.PrimaryAbilityText.text = "Cooling Down";
				}
			} else {  // No longer in cool down
				classPanelScript.PrimaryAbilityButton.GetComponent<Button>().interactable = true;
				classPanelScript.PrimaryAbilityText.text = "Block-Buster";
			}
		}
	}
}
