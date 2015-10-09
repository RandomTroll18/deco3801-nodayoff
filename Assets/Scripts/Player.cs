using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	public GameObject[] InventoryUI = new GameObject[9]; // UI Slots
	public GameObject GameManagerObject; // The game manager object
	public GameObject EffectBoxPanel; // The effect box panel
	public GameObject StunGunPrefab; // Stun Gun Prefab
	public GameObject PlayerObject; // The game object that this script is attached to
	public string ClassToSet; // The class to set
	public static string ChosenClass; // The chosen class
	public bool IsSpawned; // Record if this object is spawned
	/*
	 * This is an important variable since we can no longer search for the player in the scene.
	 * Make sure that if you need this variable, you wait for network connecting to finish so the
	 * player has spawned.
	 */
	public static GameObject MyPlayer;

	Dictionary<Stat, double> stats; // Dictionary of stats
	GameObject[] physicalItems = new GameObject[9]; // Items' Game Objects
	Item[] inventory = new Item[9]; // Inventory
	List<GameObject> droppedItems; // Recently dropped items
	List<Effect> turnEffects; // The turn-based effects attached to this player

	GameManager gameManagerScript; // The game manager script
	EffectPanelScript effectPanelScript; // The effect panel script
	PlayerClass playerClass; // The class of this player
	Transform transformComponent; // The transform component of this player
	int availableSpot; // Earliest available spot in inventory
	bool turnEffectsApplied; // Record whether turn effects are applied
	bool noLongerActive; // Record if this player is still active
	bool isStunned; // Record if this player is stunned
	int stunTimer;
	bool isImmuneToStun; // Recod if the player is immune to stun
	Light playerLight; // Player's light
	Material playerMaterial; // The material of the player

	/**
	 * StartMe function for networking purposes
	 */
	public void StartMe() {

		SetPublicVariables();
		GatherScripts();

		foreach (Transform child in transform) {
			if (child.CompareTag("Lighting"))
				playerLight = child.GetComponent<Light>();
		}

		//this.rb = GetComponent<Rigidbody>();
		initializeInventory();

		droppedItems = new List<GameObject>();

		if (!IsSpawned && ChosenClass != null) 
			ClassToSet = ChosenClass; // Assign chosen player class
		SetPlayerClass(ClassToSet);

		transformComponent = GetComponent<Transform>();
		availableSpot = 0;

		stats = new Dictionary<Stat, double>();
		InitializeStats();

		turnEffects = new List<Effect>();
		turnEffectsApplied = false;
		noLongerActive = false;

		isImmuneToStun = false;
		playerMaterial = Resources.Load<Material>("Cube Player Skin");
	}

	/*
	 * I put this hear since NetworkManager moves the player after its StartMe() is called.
	 */
	public void GenerateStunGun() {
		GameObject stunGunObject;
		if (StunGunPrefab != null && !IsSpawned) {
			stunGunObject = Instantiate(StunGunPrefab);
			stunGunObject.GetComponent<StunGun>().StartAfterInstantiate();
			stunGunObject.transform.position = new Vector3(transformComponent.position.x, 
					(float) 0.0, transformComponent.position.z);
		}
	}

	/*
	 * This is a really bad thing to do since it's so hard to understand why this needs to be done.
	 * Understand that players need to be instantiated from a script, because of PUN, and so we 
	 * can't assign public variables from the scene within the editor.
	 */
	void SetPublicVariables() {
		GameManagerObject = Object.FindObjectOfType<GameManager>().gameObject;
		InventoryUI = new GameObject[9];
		InventoryUI[0] = GameObject.Find("Slot1");
		InventoryUI[1] = GameObject.Find("Slot2");
		InventoryUI[2] = GameObject.Find("Slot3");
		InventoryUI[3] = GameObject.Find("Slot4");
		InventoryUI[4] = GameObject.Find("Slot5");
		InventoryUI[5] = GameObject.Find("Slot6");
		InventoryUI[6] = GameObject.Find("Slot7");
		InventoryUI[7] = GameObject.Find("Slot8");
		InventoryUI[8] = GameObject.Find("Slot9");
		GameManagerObject = Object.FindObjectOfType<GameManager>().gameObject;
		EffectBoxPanel = GameObject.Find("EffectBoxPanel");
		StunGunPrefab = Resources.Load("StunGun") as GameObject;
		PlayerObject = gameObject;
		if (ClassToSet != null)
			ClassToSet = "Technician";
	}

	/*
	 * Returns the Player at the given tile or null if no player is at that tile.
	 * If multiple players are on a tile, just returns one of those players.
	 */
	public static Player PlayerAtTile(Tile tile) {
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
			if (Tile.TilePosition(p.transform.position).Equals(tile)) {
				return p.GetComponent<Player>();
			}
		}
		
		return null;
	}

	void GatherScripts() {
		gameManagerScript = GameManagerObject.GetComponent<GameManager>();
		effectPanelScript = EffectBoxPanel.GetComponent<EffectPanelScript>();
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
		UpdateVision();
		if (!gameManagerScript.IsValidTurn()) {
			turnEffectsApplied = false;
			noLongerActive = false;
		}
		else {
			if (noLongerActive) // Don't do anything
				return;
			else if (isStunned && stunTimer-- > 0) { // Player is currently stunned. Can't do anything
				noLongerActive = true;
				// Player can't do anything anymore so need to tell Game manager player is done
				GameManagerObject.GetComponent<PhotonView>().RPC("SetInactivePlayer", 
				                                                 PhotonTargets.All, null);
				Debug.Log("Game Manager Turns Remaining: " + gameManagerScript.RoundsLeftUntilLose);
				Debug.Log("Stun timer: " + stunTimer);
				return;
			} else if (isStunned) {
				Debug.Log("Not stunned");
				isStunned = false;
				noLongerActive = false;
			}
			// Allow movement
		}
	}

	[PunRPC]
	public void Stun(int timer) {
		Debug.Log("Player Stunned");
		isStunned = true;
		stunTimer = timer;
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
			playerClass = new AlienClass(this);
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
	 * Attach a single turn effect to this player
	 * 
	 * Arguments
	 * - TurnEffect toAdd - Effect to add
	 */
	public void AttachTurnEffect(Effect toAdd) {
		switch (toAdd.GetTurnEffectType()) { // Apply turn effects immediately if needed
		case TurnEffectType.STATEFFECT: goto default; // Only apply stat effects in the next turn
		case TurnEffectType.MATERIALEFFECT:
			PlayerObject.GetComponent<Renderer>().material = toAdd.GetMaterial();
			break;
		case TurnEffectType.ITEMEFFECT: // Apply item effects immediately
			foreach (Item item in inventory) {
				if (item != null) toAdd.ApplyEffectToItem(item);
			}
			break;
		default: break; // Don't do anything
		}
		turnEffects.Add(toAdd);
		effectPanelScript.AddTurnEffect(toAdd);
	}

	/**
	 * Attach a list of turn effects to this player
	 * 
	 * Arguments
	 * - List<TurnEffect> effects - Effects to attach
	 */
	public void AttachTurnEffects(List<Effect> effects) {
		foreach (Effect effect in effects) AttachTurnEffect(effect);
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

			/* We collided with an item. Pick it up */
			physicalItems[availableSpot] = other.gameObject;
			item = other.GetComponent<Item>();
			inventory[availableSpot] = item;
			uiSlotScript.InsertItem(item);
			other.gameObject.SetActive(false); // Make object disappear

			// Get turn effects if they exist
			if (item.GetTurnEffects() != null) AttachTurnEffects(item.GetTurnEffects());

			// Apply effects to item on pickup
			applyItemEffect(item);

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
	 * Apply item effects to the given item. For use when item is picked up
	 * 
	 * Arguments
	 * Item itemToAffect - The item to affect
	 */
	void applyItemEffect(Item itemToAffect) {
		foreach (Effect effect in turnEffects) {
			if (!effect.GetTurnEffectType().Equals(TurnEffectType.ITEMEFFECT)) continue;
			if (!itemToAffect.GetType().Equals(effect.GetAffectedItemType())) continue;
		}
	}

	/**
	 * Remove a turn effect from this player and revert all its effects on this player
	 * 
	 * Arguments
	 * - TurnEffect effect - The turn effect to remove
	 */
	public void DetachTurnEffect(Effect effect) {
		switch (effect.GetTurnEffectType()) { // Detach the effect on this player depending on type
		case TurnEffectType.STATEFFECT: goto default; // Don't do anything yet
		case TurnEffectType.ITEMEFFECT: // Need to reset affected items
			foreach (Item item in inventory) {
				if (item != null && item.GetType().Equals(effect.GetAffectedItemType())) { 
					// Reset item cool down and turn settings
					item.ResetCoolDown();
					item.ResetCoolDownSetting();
					item.ResetUsePerTurn();
					if (item.RemainingCoolDownTurns() == 0) item.ReduceCoolDown();
				}
			}
			break;
		case TurnEffectType.MATERIALEFFECT: 
			PlayerObject.GetComponent<Renderer>().material = playerMaterial; // Reassign material
			break;
		default: break; // Unknown
		}
		effectPanelScript.RemoveTurnEffect(effect);
		turnEffects.Remove(effect);
	}

	/**
	 * Apply a given turn effect
	 * 
	 * Arguments
	 * - TurnEffect effect - The turn effect to apply
	 */
	void applyTurnEffect(Effect effect) {
		Stat stat; // The stat to effect
		int mode; // The mode of this turn effect

		if (effect.TurnsRemaining() == 0) { // Remove turn effect
			DetachTurnEffect(effect);
			return;
		}

		if (effect.IsAppliedPerTurn()) { // Apply per turn
			switch (effect.GetTurnEffectType()) {
			case TurnEffectType.STATEFFECT: // Stat effect
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
				/*
				if (stat == Stat.AP) { // Update AP Counter
					if (IsSpawned) APCounterText.text = "Spawn AP Count: " + stats[stat];
					else APCounterText.text = "Player AP Count: " + stats[stat];
				}*/
				break;
			case TurnEffectType.MATERIALEFFECT:
				// Only replace material if not already set
				if (!PlayerObject.GetComponent<Renderer>().material.Equals(effect.GetMaterial()))
					PlayerObject.GetComponent<Renderer>().material = effect.GetMaterial();
				break;// Change material
				
			default: break; // Unknown
			}
		}

		effect.ReduceTurnsRemaining();
	}

	/**
	 * Set player's inactivity
	 * 
	 * Arguments
	 * - bool newActivityState - boolean value for whether player is not active or not
	 */
	public void SetInActivity(bool newActivityState) {
		noLongerActive = newActivityState;
	}

	/**
	 * Check if this player is active or not
	 * 
	 * Returns
	 * - true if this player is not active. false if otherwise
	 */
	public bool IsPlayerNoLongerActive() {
		return noLongerActive;
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
		case Stat.AP: goto default;
			/*
			if (IsSpawned) 
				APCounterText.text = "Spawn AP Count: " + stats[playerStat];
			else 
				APCounterText.text = "Player AP Count: " + stats[playerStat];
			break;*/
		case Stat.VISION:
			if (value < 1 || value > 3) {
				Debug.LogError("You tried to change the vision stat to a value outside it's range" +
			                   "of 1-3. Setting vision value to 2.");
				stats[Stat.VISION] = 2;
			}
			UpdateVision();
			break;
		default: break; // Do nothing yet
		}
	}

	/**
	 * Set the light attached to this player
	 * 
	 * Arguments
	 * - Light newPlayerLight - The new light for this player
	 */
	public void SetPlayerLight(Light newPlayerLight) {
		playerLight = newPlayerLight;
	}

	/**
	 * Get the light attached to this player
	 * 
	 * Returns
	 * - The player's light
	 */
	public Light GetPlayerLight() {
		return playerLight;
	}

	void UpdateVision() {
		int visionDistance = 2;
		int distanceToCharacter; // The distance to a character
		if (playerLight == null) // No light exists
			return;
		if (stats[Stat.VISION] <= 1f && stats[Stat.VISION] >= 1f) {
			playerLight.intensity = 0;
		} else if (stats[Stat.VISION] <= 2f && stats[Stat.VISION] >= 2f) {
			playerLight.intensity = 4;
			visionDistance = 4;
		} else if (stats[Stat.VISION] <= 3f && stats[Stat.VISION] >= 3f) {
			playerLight.intensity = 8;
			visionDistance = 6;
		}
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
			if (p.Equals(gameObject))
				continue;
			distanceToCharacter = GetComponent<MovementController>().TileDistance(p.transform.position);

			if (distanceToCharacter == -1) // No path found
				continue;

			if (GetComponent<MovementController>().TileDistance(p.transform.position) 
			                      <= visionDistance) {
				p.GetComponent<MeshRenderer>().enabled = true;
			} else {
				p.GetComponent<MeshRenderer>().enabled = false;
			}
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
	}

	/**
	 * Function used to drop the given item
	 * 
	 * Arguments
	 * - GameObject contextAwareBox - The context aware box
	 */
	public void DropItem(GameObject contextAwareBox) {
		Item item = (Item)contextAwareBox.GetComponent<ContextAwareBox>().GetAttachedObject();
		int itemIndex; // The index of the given item
		InventoryUISlotScript uiSlotScript; // The ui slot script
		if (item == null) return;
		itemIndex = getIndex(item);
		if (itemIndex == -1) return;
		// Set game object to be behind the player and set it to active
		physicalItems[itemIndex].SetActive(true);
		physicalItems[itemIndex].transform.position = 
				new Vector3(transformComponent.position.x, (float)0.0, 
			    transformComponent.position.z);

		// Add item to list of recently dropped items
		droppedItems.Add(physicalItems[itemIndex]);

		// Remove effects if the item has some turn effects
		if (item.GetTurnEffects() != null) 
			foreach (Effect turnEffect in item.GetTurnEffects()) DetachTurnEffect(turnEffect);

		// Remove the item from the ui slot
		uiSlotScript = InventoryUI[itemIndex].GetComponent<InventoryUISlotScript>();
		uiSlotScript.RemoveItem();

		// Set inventory references to null
		inventory[itemIndex] = null;
		physicalItems[itemIndex] = null;

		// Reset item to have default values
		item.ResetCoolDownSetting();
		item.ResetUsePerTurn();
		item.ResetCoolDown();

		// Find next available spot
		availableSpot = 0;
		while (availableSpot != 9 && inventory[availableSpot] != null) {
			availableSpot++;
		}
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
			if (inventory[i] == null) continue; // Nothing here
			else if (inventory[i].Equals(itemToGet)) return i;
		}
		return -1; // Item not found
	}

	/**
	 * Apply turn effects attached to this object
	 */
	public void ApplyTurnEffects() {
		Effect currentEffect; // The current effect being applied
		if (turnEffectsApplied) return;

		for (int i = 0; i < turnEffects.Count; ++i) {
			currentEffect = turnEffects[i];
			applyTurnEffect(turnEffects[i]);
			if (!turnEffects.Contains(currentEffect)) --i; // Turn effect removed
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
