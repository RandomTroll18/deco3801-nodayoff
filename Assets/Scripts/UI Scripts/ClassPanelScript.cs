using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/**
 * Script for the class panel
 */
public class ClassPanelScript : MonoBehaviour {

	public Text ClassTitle; // The Text Box for the Class Title
	public Text PrimaryAbilityText; // The text for the primary ability button
	public Text AlienPrimaryAbilityText; // The text for the alien primary ability button
	public GameObject ClassPanel; // The class panel that this script is attached to
	public GameObject ContextAwareBox; // The context aware box
	public GameObject GameManager; // The game manager
	public GameObject PrimaryAbilityButton; // The primary ability button
	public GameObject AlienPrimaryAbilityButton; // The Alien mode ability button
	public GameObject ClassPortrait; // The portrait for the class
	public GameObject SkillPortrait; // The portrait for the skill
	public GameObject SpawnAPCounterPanel; // The spawn ap counter panel
	public static GameObject CurrentPlayer; // The current player being tracked. For spawning use
	public List<GameObject> Interactables; // Interactable objects
	
	CameraController cameraController; // The camera controller script
	MovementController movementController; // The movement controller script
	ContextAwareBox contextAwareBoxScript; // The context aware box script
	ActivationTileController activationTileController; // Controller for generating activation tiles
	Player playerOwnerScript; // Script of the player

	/**
	 * Start function
	 */
	public void StartMe() {
		PlayerClass ownerClass; // The class of the player owner
		AlienClass alienClass; // Container for the alien class, if it is true

		cameraController = Camera.main.GetComponent<CameraController>();
		movementController = Player.MyPlayer.GetComponent<MovementController>();
		contextAwareBoxScript = ContextAwareBox.GetComponent<ContextAwareBox>();
		activationTileController = ContextAwareBox.GetComponent<ActivationTileController>();
		playerOwnerScript = Player.MyPlayer.GetComponent<Player>();
		ownerClass = playerOwnerScript.GetPlayerClassObject();
		ClassTitle.text = ownerClass.GetPlayerClassType();
		if (ownerClass.GetPrimaryAbility() == null) 
			PrimaryAbilityText.text = "No name";
		else { // Set button text and abilities
			if (ownerClass.GetClassTypeEnum() == Classes.BETRAYER) { // Alien. Need to account for alien ui
				alienClass = (AlienClass)ownerClass;
				setClassTitleForAlien(alienClass);
				AlienPrimaryAbilityButton.SetActive(true);
				PrimaryAbilityText.text = alienClass.GetHumanClass().GetPrimaryAbility().GetAbilityName();
				AlienPrimaryAbilityText.text = ownerClass.GetPrimaryAbility().GetAbilityName();

				alienClass.GetHumanClass().GetPrimaryAbility().SetClassPanel(ClassPanel);
				alienClass.GetHumanClass().GetPrimaryAbility().ExtraInitializing();
			} else // Ordinary human class. Just set the appropriate text
				PrimaryAbilityText.text = ownerClass.GetPrimaryAbility().GetAbilityName();

			ownerClass.GetPrimaryAbility().SetClassPanel(ClassPanel);
			ownerClass.GetPrimaryAbility().ExtraInitializing();
		}
		CurrentPlayer = Player.MyPlayer; // Current player variable for engineer class
		/* Set portrait */
		setPortraits(ownerClass);
	}

	/**
	 * Set the portrait for the class image
	 * 
	 * Arguments
	 * - PlayerClass playerClass - The player's class
	 */
	void setPortraits(PlayerClass playerClass) {
		AlienClass alienClass; // Alien class container
		switch (playerClass.GetClassTypeEnum()) {
		case Classes.ENGINEER: // Eng
			ClassPortrait.GetComponent<Image>().sprite = 
					Resources.Load<Sprite>("Icons/Class/Engineer/engportraitclear");
			SkillPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/SkillButtons/engSkill");
			break;
		case Classes.MARINE: // Marine
			ClassPortrait.GetComponent<Image>().sprite = 
				Resources.Load<Sprite>("Icons/Class/Marine/marineportraitclear");
			SkillPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/SkillButtons/marSkill");
			break;
		case Classes.SCOUT: // Scout
			ClassPortrait.GetComponent<Image>().sprite = 
				Resources.Load<Sprite>("Icons/Class/Scout/scoutportraitblueclear");
			SkillPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/SkillButtons/ScoutSkill");
			break;
		case Classes.TECHNICIAN: // Technician
			ClassPortrait.GetComponent<Image>().sprite = 
				Resources.Load<Sprite>("Icons/Class/Technician/techportrairclear");
			SkillPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/SkillButtons/TechSkill");
			break;
		case Classes.BETRAYER: // Alien. No Portrait
			ClassPortrait.SetActive(false);
			alienClass = (AlienClass)playerClass;
			switch (alienClass.GetHumanClassType()) {
			case Classes.ENGINEER: // Eng
				SkillPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/SkillButtons/engSkill");
				break;
			case Classes.MARINE: // Marine
				SkillPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/SkillButtons/marSkill");
				break;
			case Classes.SCOUT: // Scout
				SkillPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/SkillButtons/ScoutSkill");
				break;
			case Classes.TECHNICIAN: // Technician
				SkillPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/SkillButtons/TechSkill");
				break;
			case Classes.BETRAYER: goto default;
			default:
				throw new System.NotSupportedException("Invalid alien class");
			}
			break;
		default:
			throw new System.NotSupportedException("Invalid class");
		}
	}

	/**
	 * Set the text of the class title for the alien
	 * 
	 * Arguments
	 * - AlienClass alienClass - The alien class
	 */
	void setClassTitleForAlien(AlienClass alienClass) {
		ClassTitle.text = "Alien (";
		switch (alienClass.GetHumanClassType()) {
		case Classes.ENGINEER:
			ClassTitle.text += "Engineer)";
			break;
		case Classes.MARINE:
			ClassTitle.text += "Marine)";
			break;
		case Classes.SCOUT:
			ClassTitle.text += "Scout)";
			break;
		case Classes.TECHNICIAN:
			ClassTitle.text += "Technician)";
			break;
		default:
			throw new System.NotSupportedException("Invalid human class");
		}
	}

	/**
	 * Change the player being tracked by interactive objects
	 */
	void changeInteractiveObjectTracker() {
		foreach (GameObject interactable in Interactables) {
			if (interactable == null) // Interactable is null
				continue;
			interactable.GetComponent<InteractiveObject>().ChangeTrackedPlayer(CurrentPlayer);
		}
	}

	/**
	 * Attach spawned object to the spawn ap counter panel
	 * 
	 * Arguments
	 * - GameObject spawned - The game object that was spawned
	 */
	public void AttachSpawnedToCounter(GameObject spawned) {
		SpawnAPCounterPanel.GetComponent<APCounterScript>().Owner = spawned;
		SpawnAPCounterPanel.SetActive(true);
	}

	/**
	 * Activate alien ability
	 */
	public void ActivateAlienAbility() {
		Player alien = Player.MyPlayer.GetComponent<Player>();
		AlienClass alienClass = (AlienClass)alien.GetPlayerClassObject();
		Ability alienAbility = alienClass.GetPrimaryAbility(); // The alien's ability
		if (CurrentPlayer.GetComponent<MovementController>().IsMoving()) // Don't activate while we are moving
			return;
		if (!alienClass.GetPrimaryAbility().AbilityIsActive()) { // We are turning into an alien
			alienAbility.Activate();
			AlienPrimaryAbilityText.text = "Human Mode";
			AlienPrimaryAbilityButton.GetComponent<Button>().interactable = false;
		} else { // We are turning back into human
			alienAbility.Deactivate();
			AlienPrimaryAbilityText.text = alienAbility.GetAbilityName();
		}
	}

	/**
	 * Activate primary ability
	 */
	public void ActivatePrimaryAbility() {
		GameObject player = Player.MyPlayer;
		Player playerScript = player.GetComponent<Player>(); // Player script
		PlayerClass classOfPlayer = playerScript.GetPlayerClassObject(); // The player's class
		Ability primaryAbility; // The primary ability
		AlienClass alienClass; // Container for alien class

		if (CurrentPlayer.GetComponent<MovementController>().IsMoving()) // Don't activate while we are moving
			return;

		if (classOfPlayer.GetClassTypeEnum() == Classes.BETRAYER) { // Alien
			alienClass = (AlienClass)classOfPlayer;
			primaryAbility = alienClass.GetHumanClass().GetPrimaryAbility(); // Human ability
		} else // Human player
			primaryAbility = playerScript.GetPlayerClassObject().GetPrimaryAbility();

		movementController.ClearPath(); // Clear the visual movement tiles

		if (primaryAbility == null) return; // No abilities
		else if (!primaryAbility.AbilityIsActive()) { 
			// Only activate if ability hasn't been activated before
			switch (primaryAbility.GetAbilityId()) { // Handle different kinds of abilities
			case AbilityEnum.ENGABI:  // Engineer
				activationTileController.GeneratorInterface(playerScript, primaryAbility);
				break;
			case AbilityEnum.MARABI: // Marine
				primaryAbility.Activate();
				PrimaryAbilityButton.GetComponent<Button>().interactable = false;
				PrimaryAbilityText.text = "Currently Active";
				break;
			case AbilityEnum.TECHABI: // Technician
				primaryAbility.Activate();
				if (primaryAbility.AbilityIsActive())
					PrimaryAbilityText.text = "Exit Camera";
				break;
			default: // Ordinary targetting abilities
				activationTileController.GeneratorInterface(playerScript, primaryAbility);
				CurrentPlayer = player; // Set the current player of this script to be the calling player
				break;
			}
		} else if (primaryAbility.AbilityIsActive()){ // Ability is active. Need to decide what ability this is
			switch (primaryAbility.GetAbilityId()) {
			case AbilityEnum.ENGABI: // Toggle players
				handleEngineerPrimaryAbility(player);
				return;
			case AbilityEnum.TECHABI: // Toggle Hack Mode
				primaryAbility.Deactivate();
				if (!primaryAbility.AbilityIsActive())
					PrimaryAbilityText.text = "Big Brother";
				return;
			case AbilityEnum.MARABI: goto default; // Ability is already activated
			case AbilityEnum.SCOABI: goto default; // out of scout traps
			default: return; // Unknown action
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
		PrimaryAbilityText.text = primAbilityText;
	}

	/**
	 * Set the class panel title text. For public use
	 * 
	 * String classPanelTitle - The title to set the class panel to
	 */
	public void SetClassPanelTitle(string classPanelTitle) {
		ClassTitle.text = classPanelTitle;
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
		Player masterScript = master.GetComponent<Player>(); // The script of the player

		/* Reset camera controller and movement controller */
		movementController.ChangePlayerScript();
		cameraController.ResetCamera();

		CurrentPlayer = master; // Reset the current player to be the master
		changeInteractiveObjectTracker(); // Change the tracked player for interactive objects
		ClassTitle.text = master.GetComponent<Player>().GetPlayerClassObject().GetPlayerClassType(); // Reset class text

		// Make inventory active
		masterScript.InventoryUI[0].GetComponent<InventoryUISlotScript>().Container.SetActive(true);
		PrimaryAbilityButton.GetComponent<Button>().interactable = false; // Can't use primary ability
		PrimaryAbilityText.text = "Cooling Down"; // Reset text

		// Set the Spawn AP Counter Panel to be inactive
		SpawnAPCounterPanel.SetActive(false);

		// Set the movement controller of the activation tile controller
		activationTileController.SetMovementController(master.GetComponent<MovementController>());

		// Enable the disabled components in the master
		master.GetComponent<MovementController>().enabled = true;
		master.GetComponentInChildren<CameraController>().enabled = true;
		master.GetComponentInChildren<Camera>().enabled = true;

		// Set appropriate portrait
		if (masterScript.GetPlayerClassObject().GetClassTypeEnum() == Classes.BETRAYER) { // Alien
			setClassTitleForAlien((AlienClass)masterScript.GetPlayerClassObject());
			ClassPortrait.GetComponent<Image>().sprite = 
				Resources.Load<Sprite>("Icons/Effects/alienmodeblack");
		} else { // Human
			ClassTitle.text = masterScript.GetPlayerClassObject().GetPlayerClassType();
			ClassPortrait.GetComponent<Image>().sprite = 
				Resources.Load<Sprite>("Icons/Class/Engineer/engportraitclear");
		}
	}

	/**
	 * Handle active engineer ability
	 * 
	 * Arguments
	 * - GameObject player - The player that clicked the primary ability button
	 */
	void handleEngineerPrimaryAbility(GameObject player) {
		PlayerClass playerClass = Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject(); // Actual class
		AlienClass alienClass = null; // Alien class container
		EngineerPrimaryAbility engAbility; // The engineer's ability
		GameObject robot; // The robot
		Player robotScript; // The robot's player script
		Player callingPlayer; // The calling player's script
		bool isSelected; // Store whether or not the ui slot was selected

		/* Need to initialize/set variables */
		if (playerClass.GetClassTypeEnum() == Classes.BETRAYER) { // Alien
			alienClass = (AlienClass)playerClass;
			engAbility = (EngineerPrimaryAbility)alienClass.GetHumanClass().GetPrimaryAbility();
		} else // Human
			engAbility = (EngineerPrimaryAbility)(playerClass.GetPrimaryAbility());
		robot = engAbility.GetRobotReference();
		robotScript = robot.GetComponent<Player>();
		callingPlayer = player.GetComponent<Player>();

		/* Disable movement and camera */
		player.GetComponent<MovementController>().enabled = false;
		player.GetComponentInChildren<CameraController>().enabled = false;

		// First, make sure robot still exists
		if (robot == null) 
			return; // Don't do anything

		// Toggle
		if (CurrentPlayer == player) { // Toggle to robot
			Debug.Log("Toggle to robot");
			CurrentPlayer = robot;
			ClassTitle.text = robotScript.GetPlayerClassObject().GetPlayerClassType();
			ClassPortrait.GetComponent<Image>().sprite = 
				Resources.Load<Sprite>("Icons/Class/Engineer/robot");
			PrimaryAbilityText.text = "Toggle To Engineer";

			// Set the context aware box to be in the idle context
			contextAwareBoxScript.SetContextToIdle();

			// Set the individual ui slots to be unselected and set the inventory panel to be inactive
			callingPlayer.InventoryUI[0].GetComponent<InventoryUISlotScript>().Container.SetActive(false);
			for (int i = 0; i < 8; ++i) {
				isSelected = callingPlayer.InventoryUI[i].GetComponent<InventoryUISlotScript>().IsSelected();
				if (isSelected) callingPlayer.InventoryUI[i].GetComponent<InventoryUISlotScript>().ToggleSelected();
			}
			robot.GetComponent<MovementController>().enabled = true;
			robot.GetComponentInChildren<CameraController>().enabled = true;
			robot.GetComponentInChildren<Camera>().enabled = true;
			player.GetComponent<MovementController>().enabled = false;
			player.GetComponentInChildren<CameraController>().enabled = false;
			player.GetComponentInChildren<Camera>().enabled = false;
		} else { // Toggle to player
			Debug.Log("Toggle to player");
			robot.GetComponent<MovementController>().enabled = false;
			robot.GetComponentInChildren<CameraController>().enabled = false;
			robot.GetComponentInChildren<Camera>().enabled = false;
			player.GetComponent<MovementController>().enabled = true;
			player.GetComponentInChildren<CameraController>().enabled = true;
			player.GetComponentInChildren<Camera>().enabled = true;

			CurrentPlayer = player;
			if (alienClass != null) { // Alien
				setClassTitleForAlien(alienClass);
				ClassPortrait.GetComponent<Image>().sprite = 
					Resources.Load<Sprite>("Icons/Effects/alienmodeblack");
			} else {
				ClassTitle.text = callingPlayer.GetPlayerClassObject().GetPlayerClassType();
				ClassPortrait.GetComponent<Image>().sprite = 
					Resources.Load<Sprite>("Icons/Class/Engineer/engportraitclear");
			}
			PrimaryAbilityText.text = "Toggle To Robot";

			// Set the individual ui slots to be unselected and set the inventory panel to be inactive
			callingPlayer.InventoryUI[0].GetComponent<InventoryUISlotScript>().Container.SetActive(true);
		}

		changeInteractiveObjectTracker(); // Change the tracked player for interactive objects
		movementController.ChangePlayerScript(); // Change player script of movement controller
		cameraController.ResetCamera(); // Reset the camera to point at player object
		activationTileController.DestroyActivationTiles(); // Destroy activation tiles
		// Set the movement controller
		activationTileController.SetMovementController(CurrentPlayer.GetComponent<MovementController>()); 
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
