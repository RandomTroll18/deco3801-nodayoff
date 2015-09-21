using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	public GameObject[] InventoryUI = new GameObject[9]; // UI Slots
	public GameObject GameManagerObject; // The game manager object
	public GameObject EndTurnButton; // End turn button
	public GameObject EffectBoxPanel; // The effect box panel
	public GameObject StunGunPrefab; // Stun Gun Prefab
	public GameObject ClassPanel; // The Class Panel
	public GameObject MainCanvas; // The main canvas
	public GameObject APCounterPanel; // The panel for counting AP
	public GameObject SpawnAPCounterPanel; // The panel for counting a spawn's AP
	public GameObject PlayerObject; // The game object that this script is attached to
	public Text APCounterText; // The text for counting AP
	public Text SpawnAPCounterText; // The text for counting a spawn's AP
	public string ClassToSet; // The class to set
	public static string ChosenClass; // The chosen class
	public bool IsSpawned; // Record if this object is spawned

	Dictionary<Stat, double> stats; // Dictionary of stats
	GameObject[] physicalItems = new GameObject[9]; // Items' Game Objects
	Item[] inventory = new Item[9]; // Inventory
	List<GameObject> droppedItems; // Recently dropped items
	List<TurnEffect> turnEffects; // The turn-based effects attached to this player

	GameManager gameManagerScript; // The game manager script
	ClassPanelScript classPanelScript; // The class panel script
	EffectPanelScript effectPanelScript; // The effect panel script
	PlayerClass playerClass; // The class of this player
	Transform transformComponent; // The transform component of this player
	int availableSpot; // Earliest available spot in inventory
	bool turnEffectsApplied; // Record whether turn effects are applied
	bool noLongerActive; // Record if this player is still active
	bool isStunned; // Record if this player is stunned
	bool isImmuneToStun; // Recod if the player is immune to stun
	Light playerLight;

	/*
	 * Physics objects
	 */
	//private BoxCollider boxCollider; - Might not be needed
	//private Rigidbody rb;

	/**
	 * Do the following to start the player:
	 * - Get rigidbody component for movement purposes - not needed yet
	 * - Set all values in inventory to null
	 * - Set the player's class to the base class for now
	 * - Get the transform component
	 * - Initialize list of recently dropped items
	 * - Initialize available spots
	 * - Initialize stats
	 * - Initialize array list of turn effects
	 * - Get the Game Manager's script
	 * - Set turn effects applied variable to be false
	 * - Set that this player is not active
	 * - Get the effect panel script
	 * - Instantiate a Stun Gun for the player and have the player pick it up
	 * - Get class panel script and initialize class panel
	 */
	void Awake() {
		foreach (Transform child in transform) {
			if (child.CompareTag("Lighting"))
				playerLight = child.GetComponent<Light>();
		}

		GameObject stunGunObject; // The stun gun object
		//this.rb = GetComponent<Rigidbody>();
		initializeInventory();

		droppedItems = new List<GameObject>();

		if (!IsSpawned && ChosenClass != null) ClassToSet = ChosenClass; // Assign chosen player class
		SetPlayerClass(ClassToSet);
		Debug.Log("Player Class: " + GetPlayerClass());

		transformComponent = GetComponent<Transform>();
		availableSpot = 0;

		stats = new Dictionary<Stat, double>();
		InitializeStats();

		turnEffects = new List<TurnEffect>();
		gameManagerScript = GameManagerObject.GetComponent<GameManager>();
		turnEffectsApplied = false;
		noLongerActive = false;
		effectPanelScript = EffectBoxPanel.GetComponent<EffectPanelScript>();

		if (StunGunPrefab != null) { // Generate stun gun 
			stunGunObject = Instantiate(StunGunPrefab);
			stunGunObject.GetComponent<StunGun>().StartAfterInstantiate();
			stunGunObject.transform.position = 
				new Vector3(transformComponent.position.x, (float)0.0, transformComponent.position.z);
		}

		classPanelScript = ClassPanel.GetComponent<ClassPanelScript>();
		if (playerClass.GetPrimaryAbility() == null) {
			classPanelScript.InitializeClassPanel(playerClass.GetPlayerClassType(), "No name");
		} else {
			classPanelScript.InitializeClassPanel(playerClass.GetPlayerClassType(), 
					playerClass.GetPrimaryAbility().GetAbilityName());
		}

		isImmuneToStun = false;

	}
	
	/**
	 * Set the player's immunity to stun
	 * 
	 * Arguments
	 * - bool immuneFlag - Value to set the stun immunity variable
	 */
	public void SetStunImmunity(bool immuneFlag) {
		isImmuneToStun = immuneFlag;
	}

	/**
	 * Check if player is immune to stun
	 * 
	 * Return 
	 * - true if player is immune
	 * - false if otherwise
	 */
	public bool IsPlayerStunImmune() {
		return isImmuneToStun;
	}

	/**
	 * Update function. Needs to be done:
	 * - if we are in an invalid turn, reset values to default
	 * - if we are in a valid turn and we are active, 
	 * apply turn effects (if not activated yet) and allow movement
	 * - if we are in a valid turn but we are not active, don't 
	 * allow movement
	 */
	void Update() {
		if (!gameManagerScript.IsValidTurn()) {
			Debug.Log("Player is not in a valid turn");
			turnEffectsApplied = false;
			noLongerActive = false;
		}
		else {
			if (isStunned) { // Player is currently stunned. Can't do anything
				noLongerActive = true;
				return;
			}
			if (noLongerActive) {
				// Record that the this player is no longer active
				gameManagerScript.SetInactivePlayer();
			}
			// Allow movement
		}
	}

	/**
	 * Sets the class of this player
	 * 
	 * Arguments
	 * - string playerClass - The class to give to this player
	 */
	public void SetPlayerClass(string newPlayerClass) {
		switch (newPlayerClass) {
		case "Base": goto default;
		case "Engineer": // Create Engineer Class
			playerClass = new EngineerClass(this);
			return;
		case "Alien": // Create Alien Class
			playerClass = new AlienClass();
			return;
		case "Marine": // Create Marine Class
			playerClass = new MarineClass(this);
			return;
		case "Scout": // Create Scout Class
			playerClass = new ScoutClass(this);
			return;
		case "Technician": // Create Technician Class
			playerClass = new TechnicianClass(this);
			return;
		case "Engineer Robot": // Create Class for an Engineer Robot
			playerClass = new EngineerRobotClass();
			return;
		default: // The default class will be the base
			playerClass = new BaseClass();
			return;
		}
	}

	/**
	 * Returns the class of this player in human readable format
	 * 
	 * Returns
	 * - The name of this player's class
	 */
	public string GetPlayerClass() {
		return playerClass.GetPlayerClassType();
	}

	/**
	 * Returns the actual player class object attached to this player
	 * 
	 * Returns
	 * - The actual player class
	 */
	public PlayerClass GetPlayerClassObject() {
		return playerClass;
	}

	/**
	 * Function handling event when we exit a trigger
	 */
	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Item")) { // Remove item from list of dropped items
			droppedItems.Remove(other.gameObject);
		}
	}

	/**
	 * Function handling collision with a trigger item
	 */
	void OnTriggerEnter(Collider other) {
		InventoryUISlotScript uiSlotScript; // The ui slot script
		Item item; // The item attached to a game object
		if (other.gameObject.CompareTag("Item")) {
			if (availableSpot == 9) return; // No more room
			if (droppedItems.Contains(other.gameObject)) return; // Just recently dropped
			if (InventoryUI == null || InventoryUI.Length == 0) return; // Don't do anything
			// Get the ui slot script
			uiSlotScript = InventoryUI[availableSpot].GetComponent<InventoryUISlotScript>();

			// We collided with an item. Pick it up
			physicalItems[availableSpot] = other.gameObject;
			item = other.GetComponent<Item>();
			Debug.Log("Item just collided with: " + item.ItemName);
			Debug.Log("Item toString: " + other.GetComponent<Item>());
			inventory[availableSpot] = item;
			Debug.Log("Item image: " + item.Image);
			uiSlotScript.InsertItem(item);
			other.gameObject.SetActive(false); // Make object disappear

			// Get turn effects if they exist
			if (item.GetTurnEffects() != null) {
				turnEffects.AddRange(item.GetTurnEffects());
				effectPanelScript.AddTurnEffects(item.GetTurnEffects());
				for (int i = 0; i < turnEffects.Count; ++i) {
					Debug.Log("Player effect " + i + ": " + turnEffects[i]);
				}
				Debug.Log("Added turn effects");
			}

			// Increment to the next available spot
			while (availableSpot != 9 && inventory[availableSpot] != null) {
				availableSpot++;
			}
		} else if (other.gameObject.CompareTag("Trap")) {

			Trap TrapObject = other.GetComponent<Trap>();
			TrapObject.Activated(this);

		}

	}

	/**
	 * Apply a given turn effect
	 * 
	 * Arguments
	 * - TurnEffect effect - The turn effect to apply
	 */
	void applyTurnEffect(TurnEffect effect) {
		Stat stat; // The stat to effect
		int mode; // The mode of this turn effect
		stat = effect.GetStatAffected();
		mode = effect.GetMode();
		switch (mode) {
		case 0: // Increment to stat
			stats[stat] += effect.GetValue();
			break;
		case 1: // Set stat
			stats[stat] = effect.GetValue();
			break;
		case 2: // Multiply stat
			stats[stat] *= effect.GetValue();
			break;
		default: // Invalid mode. Do nothing
			break;
		}
		stats[stat] += effect.GetValue();
		if (stat == Stat.AP) { // Update AP Counter
			if (IsSpawned) APCounterText.text = "Spawn AP Count: " + stats[stat];
			else APCounterText.text = "Player AP Count: " + stats[stat];
		}
	}

	/**
	 * End this player's turn. Write everything that the 
	 * player should do here when the player's turn ends
	 */
	public void EndTurn() {
		if (!noLongerActive) {
			noLongerActive = true;
			gameManagerScript.SetInactivePlayer();
			EndTurnButton.SetActive(false);
		}
	}

	/**
	 * Function used to initialize inventory array
	 */
	void initializeInventory() {
		for (int i = 0; i < 9; ++i) { 
			inventory[i] = null;
			physicalItems[i] = null;
		}
	}

	/**
	 * Initialize the following objects:
	 * - End Turn Button
	 */
	public void InitializeAttachedObjects() {
		EndTurnButton.SetActive(true);
	}

	/**
	 * Increase the value for a particular player stat
	 * 
	 * Arguments
	 * - Stat playerStat - The stat to increase
	 * - double value - The value to increase the stat by
	 */
	public void IncreaseStatValue(Stat playerStat, double value) {
		SetStatValue(playerStat, GetStatValue(playerStat) + value);
		// stats can go beyond the default value - Ben
//		if (stats[playerStat] >= playerClass.GetDefaultStat(playerStat)) 
//			stats[playerStat] = playerClass.GetDefaultStat(playerStat);
//		if (playerStat == Stat.AP) // Update AP Counter
//			if (IsSpawned) APCounterText.text = "Spawn AP Count: " + stats[playerStat];
//			else APCounterText.text = "Player AP Count: " + stats[playerStat];
	}

	/**
	 * Reduce the value for a particular player stat
	 * 
	 * Arguments
	 * - Stat playerStat - The stat to reduce
	 * - double value - The value to reduce the stat by
	 */
	public void ReduceStatValue(Stat playerStat, double value) {
		SetStatValue(playerStat, GetStatValue(playerStat) - value);
	}

	/**
	 * Set the value for a particular player stat
	 * 
	 * Arguments
	 * - Stat playerStat - The stat to set
	 * - double value - The value to set the stat by
	 */
	public void SetStatValue(Stat playerStat, double value) {
		stats[playerStat] = value;
		if (stats[playerStat] <= 0.0) {
			stats[playerStat] = 0.0;
		}

		/*
		 * Add any constraint checking to this switch
		 */
		switch(playerStat) {
		case Stat.AP:
			if (IsSpawned) 
				APCounterText.text = "Spawn AP Count: " + stats[playerStat];
			else 
				APCounterText.text = "Player AP Count: " + stats[playerStat];
			break;
		case Stat.VISION:
			if (value < 1 || value > 3) {
				Debug.LogError("You tried to change the vision stat to a value outside it's range" +
			                   "of 1-3. Setting vision value to 2.");
				stats[Stat.VISION] = 2;
			}
			UpdateVision();
			break;
		}
	}

	void UpdateVision() {
		if (stats[Stat.VISION] == 1) {
			playerLight.intensity = 0;
		} else if (stats[Stat.VISION] == 2) {
			playerLight.intensity = 10;
		} else if (stats[Stat.VISION] == 3) {

		}
	}

	/**
	 * Get the value for a particular player stat
	 * 
	 * Arguments
	 * - Stat playerStat - The stat to get
	 * 
	 * Returns
	 * - The value of the stat given. Null if stat doesn't exist or value is not set
	 */
	public double GetStatValue(Stat playerStat) {
		return stats[playerStat];
	}

	/**
	 * Reinitialize player stats
	 */
	public void InitializeStats() {
		SetStatValue(Stat.AP, playerClass.GetDefaultStat(Stat.AP));
		SetStatValue(Stat.VISION, playerClass.GetDefaultStat(Stat.VISION));

		isStunned = false;

	}

	/**
	 * Function used to drop the given item
	 * 
	 * Arguments
	 * - GameObject contextAwareBox - The context aware box
	 */
	public void DropItem(GameObject contextAwareBox) {
		Item item = (Item)contextAwareBox.GetComponent<ContextAwareBoxScript>().GetAttachedObject();
		int itemIndex; // The index of the given item
		InventoryUISlotScript uiSlotScript; // The ui slot script
		Debug.Log("Item to drop: " + item);
		if (item == null) {
			Debug.Log ("No item attached");
			return;
		}
		itemIndex = getIndex(item);
		if (itemIndex == -1) {
			Debug.Log ("Item not found");
			return;
		}
		// Set game object to be behind the player and set it to active
		physicalItems[itemIndex].SetActive(true);
		physicalItems[itemIndex].transform.position = 
				new Vector3(transformComponent.position.x, (float)0.0, 
			    transformComponent.position.z);

		// Add item to list of recently dropped items
		droppedItems.Add(physicalItems[itemIndex]);

		// Remove effects if the item has some turn effects
		if (item.GetTurnEffects() != null) {
			foreach (TurnEffect turnEffect in item.GetTurnEffects()) {
				effectPanelScript.RemoveTurnEffect(turnEffect);
				turnEffects.Remove(turnEffect);
			}
		}

		// Remove the item from the ui slot
		uiSlotScript = InventoryUI[itemIndex].GetComponent<InventoryUISlotScript>();
		uiSlotScript.RemoveItem();

		// Set inventory references to null
		inventory[itemIndex] = null;
		physicalItems[itemIndex] = null;

		// Find next available spot
		availableSpot = 0;
		while (availableSpot != 9 && inventory[availableSpot] != null) {
			availableSpot++;
		}
		Debug.Log ("Item: " + item.ItemName + " Dropped");
	}

	/**
	 * Function that gets the index of the given item
	 * 
	 * Arguments
	 * - Item item - The item being looked for
	 *
	 * Returns
	 * - The index of the item given, if it is inside the array
	 * - -1 otherwise
	 */
	int getIndex(object itemToGet) {
		for (int i = 0; i < 9; ++i) {
			Debug.Log("Getting index: " + inventory[i]);
			if (inventory[i] == null) continue; // Nothing here
			else if (inventory[i].Equals(itemToGet)) return i;
		}
		return -1; // Item not found
	}

	/**
	 * Apply turn effects attached to this object
	 */
	public void ApplyTurnEffects() {
		if (turnEffectsApplied) {
			Debug.Log("Effects already applied");
			return;
		}
		Debug.Log ("Start applying turn effects");

		for (int i = 0; i < turnEffects.Count; ++i) {
			applyTurnEffect(turnEffects[i]);
		}

		turnEffectsApplied = true; // We have applied turn effects
	}

	/**
	 * Reduce the number of turns this player's ability is still active
	 */
	public void ReduceAbilityTurns() {
		if (playerClass.GetPrimaryAbility() == null) return; // No ability
		playerClass.GetPrimaryAbility().ReduceNumberOfTurns();
	}

	/**
	 * Reduce the cool downs for the items attached to this player
	 */
	public void ReduceItemCoolDowns() {
		foreach (Item item in inventory) {
			if (item == null) continue;
			item.ReduceCoolDown();
		}
	}

	public Tile PlayerPosition() {
		Tile pos = new Tile();
		pos.X = Tile.TilePosition(transform.position.x);
		pos.Z = Tile.TilePosition(transform.position.z);
		return pos;
	}

}
