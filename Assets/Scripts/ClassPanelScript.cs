using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Script for the class panel
 */
public class ClassPanelScript : MonoBehaviour {

	public Text ClassTitle; // The Text Box for the Class Title
	public Text PrimaryAbilityText; // The text for the primary ability button
	public GameObject ContextAwareBox; // The context aware box
	public GameObject MainCamera; // The main camera
	public GameObject GameManager; // The game manager
	public GameObject PrimaryAbilityButton; // The primary ability button
	
	CameraController cameraController; // The camera controller script
	MovementController movementController; // The movement controller script
	ContextAwareBoxScript contextAwareBoxScript; // The context aware box script
	ActivationTileController activationTileController; // Controller for generating activation tiles
	GameObject currentPlayer; // The current player being tracked. For spawning use

	/**
	 * Start function
	 */
	void Start() {
		cameraController = MainCamera.GetComponent<CameraController>();
		movementController = GameManager.GetComponent<MovementController>();
		contextAwareBoxScript = ContextAwareBox.GetComponent<ContextAwareBoxScript>();
		activationTileController = ContextAwareBox.GetComponent<ActivationTileController>();
	}

	/**
	 * Activate primary ability
	 * 
	 * Arguments
	 * - GameObject player - The player whose ability is being activated
	 */
	public void ActivatePrimaryAbility(GameObject player) {
		Player playerScript = player.GetComponent<Player>(); // Player script
		Ability primaryAbility = playerScript.GetPlayerClassObject().GetPrimaryAbility(); // Primary ability

		if (primaryAbility != null && !primaryAbility.AbilityIsActive()) { 
			// Only activate if ability hasn't been activated before
			Debug.Log("Class Panel: ability is not active. Activate it");
			Debug.Log("Primary ability: " + primaryAbility);
			activationTileController.GeneratorInterface(playerScript, primaryAbility);
			currentPlayer = player; // Set the current player of this script to be the calling player
		} else { // Ability is active. Need to decide what ability this is
			switch (primaryAbility.GetAbilityName()) {
			case "Block-Buster": // Toggle players
				handleEngineerPrimaryAbility(player);
				return;
			default: // Unknown action
				return;
			}
		}
	}

	/**
	 * Set the primary ability button text. For public use
	 * 
	 * Arguments
	 * - string primAbilityText - The new text for the primary ability button
	 */
	public void SetPrimaryAbilityButtonText(string primAbilityText) {
		Debug.Log("Publicly setting prim ability button text");
		PrimaryAbilityText.text = primAbilityText;
		Debug.Log("Publicly set prim text: " + PrimaryAbilityText.text);
	}

	/**
	 * Set the class panel title text. For public use
	 * 
	 * String classPanelTitle - The title to set the class panel to
	 */
	public void SetClassPanelTitle(string classPanelTitle) {
		Debug.Log("Publicly setting class panel title");
		ClassTitle.text = classPanelTitle;
		Debug.Log("Publicly set class title: " + ClassTitle.text);
	}

	/**
	 * Default handler when primary ability is unknown
	 */
	void defaultPrimaryAbilityHandle() {
		Debug.Log("I dunno what to do about this active ability");
	}

	/**
	 * For the classes that spawn controllable characters, reset 
	 * the context of this class back to the master and destroy the primary ability button
	 * 
	 * Arguments
	 * - GameObject master - The original player
	 */
	public void ResetToMaster(GameObject master) {
		/* Reset camera controller and movement controller */
		cameraController.Player = master;
		movementController.Player = master;
		movementController.ChangePlayerScript();
		cameraController.ResetCamera();

		currentPlayer = master; // Reset the current player to be the master
		ClassTitle.text = master.GetComponent<Player>().GetPlayerClassObject().GetPlayerClassType(); // Reset class text
		Destroy(PrimaryAbilityButton); // Destroy the primary ability button
	}

	/**
	 * Handle active engineer ability
	 * 
	 * Arguments
	 * - GameObject player - The player that clicked the primary ability button
	 */
	void handleEngineerPrimaryAbility(GameObject player) {
		GameObject robot = GameObject.FindGameObjectWithTag("EngineerPrimAbilitySpawn"); // The robot
		Player robotScript = robot.GetComponent<Player>(); // The robot's player script
		Player callingPlayer = player.GetComponent<Player>(); // The calling player's script
		bool whoToToggle; // Record who to toggle to

		// First, make sure robot still exists
		if (robot == null) return; // Don't do anything

		// Check what to toggle to
		if (currentPlayer == player) {
			Debug.Log("Toggle to robot");
			whoToToggle = true; // Toggle to robot
		} else {
			Debug.Log("Toggle to player");
			whoToToggle = false; // Toggle to player
		}

		// Toggle
		if (whoToToggle) { // Toggle to robot
			cameraController.Player = robot;
			movementController.Player = robot;
			currentPlayer = robot;
			ClassTitle.text = robotScript.GetPlayerClassObject().GetPlayerClassType();
			PrimaryAbilityText.text = "Toggle To Engineer";
		} else { // Toggle to player
			cameraController.Player = player;
			movementController.Player = player;
			currentPlayer = player;
			ClassTitle.text = callingPlayer.GetPlayerClassObject().GetPlayerClassType();
			PrimaryAbilityText.text = "Toggle To Robot";
		}

		movementController.ChangePlayerScript(); // Change player script of movement controller
		cameraController.ResetCamera(); // Reset the camera to point at player object
	}

	/**
	 * Initialize the class panel
	 * 
	 * Arguments
	 * - string className - The class name
	 * - string primaryAbilityName-  The class' primary ability
	 */
	public void InitializeClassPanel(string className, string primaryAbilityName) {
		ClassTitle.text = className;
		PrimaryAbilityText.text = primaryAbilityName;
	}
}
