using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	public GameObject[] InventoryUI = new GameObject[8]; // UI Slots
	public GameObject GameManagerObject; // The game manager object
	public GameObject EffectBoxPanel; // The effect box panel
	public GameObject StunGunPrefab; // Stun Gun Prefab
	public GameObject PlayerObject; // The game object that this script is attached to
	public string ClassToSet; // The class to set
	public static string ChosenClass; // The chosen class
	public bool IsSpawned; // Record if this object is spawned
	public bool IsStunned; // Record if this player is stunned
	public List<AudioClip> ItemPickupEfx; // Sound effects for item pickup
	public List<AudioClip> StunEfx; // Sound effects for stunning
	/*
	 * This is an important variable since we can no longer search for the player in the scene.
	 * Make sure that if you need this variable, you wait for network connecting to finish so the
	 * player has spawned.
	 */
	public static GameObject MyPlayer;

	Dictionary<Stat, double> stats; // Dictionary of stats
	GameObject[] physicalItems = new GameObject[8]; // Items' Game Objects
	Item[] inventory = new Item[8]; // Inventory
	List<GameObject> droppedItems; // Recently dropped items
	List<Effect> turnEffects; // The turn-based effects attached to this player

	GameManager gameManagerScript; // The game manager script
	EffectPanelScript effectPanelScript; // The effect panel script
	PlayerClass playerClass; // The class of this player
	Transform transformComponent; // The transform component of this player
	int availableSpot; // Earliest available spot in inventory
	bool turnEffectsApplied; // Record whether turn effects are applied
	bool noLongerActive; // Record if this player is still active
	int stunTimer = 0; // The stun timer
	bool isImmuneToStun; // Recod if the player is immune to stun
	Light playerLight; // Player's light
	string playerMaterialPath = "PlayerMaterials/Metal"; // Path to player material

	/**
	 * StartMe function for networking purposes
	 */
	public void StartMe() {
		GameObject secondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;

		if (Application.loadedLevelName != "Tutorial") // Add security room objective
			secondaries.AddComponent<SecurityRoomObjective>();

		SetPublicVariables();
		GatherScripts();

		foreach (Transform child in transform) {
			if (child.CompareTag("Lighting"))
				playerLight = child.GetComponent<Light>();
		}

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
	}

	/*
	 * Generate the stun gun for this player
	 */
	public void GenerateStunGun() {
		GameObject stunGunObject; // The stun gun object

		if (StunGunPrefab != null && !IsSpawned) { // Proper player
			stunGunObject = Instantiate(StunGunPrefab);
			stunGunObject.transform.position = new Vector3(
					transformComponent.position.x, 
					0f, 
					transformComponent.position.z);
			stunGunObject.GetComponent<StunGun>().StartAfterInstantiate();
		}
	}

	/**
	 * Setting public variables. For use with networking
	 */
	void SetPublicVariables() {
		GameManagerObject = Object.FindObjectOfType<GameManager>().gameObject;
		InventoryUI = new GameObject[8];
		InventoryUI[0] = GameObject.Find("Slot1");
		InventoryUI[1] = GameObject.Find("Slot2");
		InventoryUI[2] = GameObject.Find("Slot3");
		InventoryUI[3] = GameObject.Find("Slot4");
		InventoryUI[4] = GameObject.Find("Slot5");
		InventoryUI[5] = GameObject.Find("Slot6");
		InventoryUI[6] = GameObject.Find("Slot7");
		InventoryUI[7] = GameObject.Find("Slot8");
		GameManagerObject = Object.FindObjectOfType<GameManager>().gameObject;
		EffectBoxPanel = GameObject.Find("EffectBoxPanel");
		StunGunPrefab = Resources.Load("StunGun") as GameObject;
		PlayerObject = gameObject;
		if (ClassToSet != null) // No class. Assume Technician
			ClassToSet = "Technician";
	}

	/*
	 * Gets the player on the given tile
	 * 
	 * Arguments
	 * - Tile tile - The tile to look at
	 * 
	 * Returns
	 * - The script of the player at tile. Null otherwise
	 */
	public static Player PlayerAtTile(Tile tile) {
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
			if (Tile.TilePosition(p.transform.position).Equals(tile)) // Found a player
				return p.GetComponent<Player>();
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
	 * Update function. Called every frame
	 */
	void Update() {
		UnblockUnlockedTiles();
		UpdateVision();
		if (!gameManagerScript.IsValidTurn()) {
			turnEffectsApplied = false;
			noLongerActive = false;
		} else {
			if (noLongerActive) // Don't do anything
				return;
			else if (IsStunned && stunTimer-- > 0) { // Player is currently stunned. Can't do anything
				noLongerActive = true;
				// Player can't do anything anymore so need to tell Game manager player is done
				GameManagerObject.GetComponent<PhotonView>().RPC("SetInactivePlayer", 
				                                                 PhotonTargets.All, null);
				Debug.Log("Game Manager Turns Remaining: " + gameManagerScript.RoundsLeftUntilLose);
				Debug.Log("Stun timer: " + stunTimer);
				return;
			} else if (IsStunned) {
				Debug.Log("Not stunned");
				IsStunned = false;
				noLongerActive = false;
			}
			// Allow movement
		}
	}

	/**
	 * Unblock tiles that have been unlocked through objectives/skill checks
	 */
	void UnblockUnlockedTiles() {
		foreach (Tile tile in gameManagerScript.UnlockedTiles) {
			if (tile != null && GetComponent<MovementController>() != null)
				GetComponent<MovementController>().UnblockTile(tile);
		}
	}

	/**
	 * Return whether or not this player is stunned
	 * 
	 * Returns
	 * - true if player is stunned. false if otherwise
	 */
	public bool IsPlayerStunned() {
		return IsStunned;
	}

	/**
	 * Instantiate starting items for this player's class
	 */
	public void InstantiateStartingItems() {
		GameObject startingItem; // The starting item
		AlienClass alienClass; // Alien class container

		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue) { // Human
			switch (playerClass.GetClassTypeEnum()) {
			case Classes.BETRAYER: goto default;
			case Classes.ENGINEER: // Create Engineer Class
				Debug.Log("Instantiating Engineer Starting Items");
				return;
			case Classes.MARINE: // Create Marine Class
				Debug.Log("Instantiating Marine Starting Items");
				return;
			case Classes.SCOUT: // Create Scout Class
				Debug.Log("Instantiating Scout Starting Items");
				return;
			case Classes.TECHNICIAN: // Create Technician Class
				Debug.Log("Instantiating Technician Starting Items");
				startingItem = InstantiateOnPlayer("Prefabs/ItemPrefabs/UsableSurveillanceCam(4)");
				if (startingItem == null)
					throw new System.ArgumentException("Prefab does not exist");
				return;
			default: // The default class will be the base
				throw new System.NotSupportedException("Wrong class for human");
			}
		} else { // Alien
			alienClass = (AlienClass)playerClass;
			switch (alienClass.GetHumanClassType()) {
			case Classes.BETRAYER: goto default;
			case Classes.ENGINEER: // Create Engineer Class
				Debug.Log("Instantiating Engineer Starting Items");
				return;
			case Classes.MARINE: // Create Marine Class
				Debug.Log("Instantiating Marine Starting Items");
				return;
			case Classes.SCOUT: // Create Scout Class
				Debug.Log("Instantiating Scout Starting Items");
				return;
			case Classes.TECHNICIAN: // Create Technician Class
				Debug.Log("Instantiating Technician Starting Items");
				startingItem = InstantiateOnPlayer("Prefabs/ItemPrefabs/UsableSurveillanceCam(4)");
				if (startingItem == null)
					throw new System.ArgumentException("Prefab does not exist");
				return;
			default: // The default class will be the base
				throw new System.NotSupportedException("Wrong class for human");
			}
		}
	}

	/**
	 * Instantiate an item for a given class (networked)
	 * 
	 * Arguments
	 * - string path - The path to the prefab to instantiate
	 * 
	 * Returns
	 * - The game object instantiated
	 */
	public GameObject InstantiateOnPlayer(string path) {
		return PhotonNetwork.Instantiate(path, transform.position, transform.rotation, 0);
	}

	/**
	 * Set the game objects (contained in this player) with the given names
	 * to be active or inactive
	 * 
	 * Arguments
	 * - string toBeActive - The game object to set active
	 * - string toBeInactive - The game object to set to inactive
	 */
	[PunRPC]
	public void SetAttachedObjects(string toBeActive, string toBeInactive) {
		foreach (Transform objectTransform in transform) {
			if (objectTransform.gameObject.name.Equals(toBeActive)) // Set to active
				objectTransform.gameObject.SetActive(true);
			else if (objectTransform.gameObject.name.Equals(toBeInactive))  // Set to inactive
				objectTransform.gameObject.SetActive(false);
		}
	}

	/**
	 * RPC call for setting the stealth of this player
	 * 
	 * Arguments
	 * - bool enableFlag - Used to indicate whether stealth is enabled or not
	 * - bool permanentFlag - Used to indicate whether stealth is permanent or not
	 */
	[PunRPC]
	public void SetStealth(bool enableFlag, bool permanentFlag) {
		if (Player.MyPlayer.GetComponent<Stealth>() != null) {
			Player.MyPlayer.GetComponent<Stealth>().enabled = enableFlag;
			Player.MyPlayer.GetComponent<Stealth>().Permanent = permanentFlag;
		}
	}


	/**
	 * Stun the player
	 * 
	 * Arguments
	 * - int timer - The amount of turns to stun the player for
	 */
	[PunRPC]
	public void Stun(int timer) {
		Debug.Log("Player Stunned");
		ChatTest.Instance.Big("STUNNED");
		IsStunned = true;
		if (stunTimer < 0) // Invalid stun timer
			stunTimer = 0;
		stunTimer += timer;
		if (SoundManagerScript.Singleton != null) { // Play sound effect
			SoundManagerScript.Singleton.gameObject.transform.position = gameObject.transform.position;
			SoundManagerScript.Singleton.PlaySingle3D(StunEfx);
		}
	}

	/**
	 * Display stun animation
	 */
	[PunRPC]
	public void DisplayStunAnim() {
		GameObject stunAnim; // The stun animation

		stunAnim = Instantiate<GameObject>(Resources.Load<GameObject>("StunGunAnim"));
		stunAnim.transform.position = transform.position;
		if (stunAnim == null) // No stun animation
			throw new System.ArgumentException("Invalid prefab path");
		Destroy(stunAnim, 3f);
	}

	/**
	 * Sets the class of this player
	 * 
	 * Arguments
	 * - string playerClass - The class to give to this player
	 */
	public void SetPlayerClass(string newPlayerClass) {
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue) { // Human
			switch (newPlayerClass) {
			case "Base": goto default;
			case "Alien": goto default;
			case "Engineer": // Create Engineer Class
				playerClass = new EngineerClass(this);
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
				throw new System.NotSupportedException("Wrong class for human");
			}
		} else { // Alien
			switch (newPlayerClass) {
			case "Base": goto default;
			case "Alien": goto default;
			case "Engineer": // Create Engineer Class
				playerClass = new AlienClass(this, Classes.ENGINEER);
				return;
			case "Marine": // Create Marine Class
				playerClass = new AlienClass(this, Classes.MARINE);
				return;
			case "Scout": // Create Scout Class
				playerClass = new AlienClass(this, Classes.SCOUT);
				return;
			case "Technician": // Create Technician Class
				playerClass = new AlienClass(this, Classes.TECHNICIAN);
				return;
			case "Engineer Robot": // Create Class for an Engineer Robot
				playerClass = new EngineerRobotClass();
				return;
			default: // The default class will be the base
				throw new System.NotSupportedException("Wrong class for an alien");
			}
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
		if (other.gameObject.CompareTag("Item")) // Remove item from list of dropped items
			if (droppedItems.Count > 0 && droppedItems.Contains(other.gameObject))
				droppedItems.Remove(other.gameObject);
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
			GetComponent<PhotonView>().RPC("SetNewMaterial", PhotonTargets.All, 
					new object[] {
						toAdd.GetMaterialPath()
					}
			);
			break;
		case TurnEffectType.ITEMEFFECT: // Apply item effects immediately
			foreach (Item item in inventory) {
				if (item != null) 
					toAdd.ApplyEffectToItem(item);
			}
			break;
		case TurnEffectType.MODELCHANGEEFFECT: goto case TurnEffectType.COMPONENTEFFECT;// Need complex attach routine
		case TurnEffectType.COMPONENTEFFECT: // Need a complex attach routine
			toAdd.ExtraAttachActions();
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
		CameraController cm = GetComponentInChildren<CameraController>(); // The camera controller
		InventoryUISlotScript uiSlotScript; // The ui slot script
		Item item; // The item attached to a game object

		switch (other.gameObject.name) { // Set the location of this player
		case "Cargo":
			cm.Location = Locations.CARGO_BAY;
			break;
		case "Quarters":
			cm.Location = Locations.QUARTERS;
			break;
		case "Left Wing":
			cm.Location = Locations.L_WING;
			break;
		case "Right Wing":
			cm.Location = Locations.R_WING;
			break;
		case "Bridge":
			cm.Location = Locations.BRDIGE;
			break;
		case "Left Gun":
			cm.Location = Locations.L_GUN;
			break;
		case "Right Gun":
			cm.Location = Locations.R_GUN;
			break;
		case "Power":
			cm.Location = Locations.POWER;
			break;
		}

		if (other.gameObject.CompareTag("Item")) { // Collided with an item
			if (availableSpot == 8) 
				return; // No more room
			if (droppedItems.Contains(other.gameObject)) 
				return; // Just recently dropped
			if (InventoryUI == null || InventoryUI.Length == 0) 
				return; // Don't do anything

			// Get the ui slot script
			uiSlotScript = InventoryUI[availableSpot].GetComponent<InventoryUISlotScript>();

			/* We collided with an item. Pick it up */
			physicalItems[availableSpot] = other.gameObject;
			item = other.GetComponent<Item>();
			inventory[availableSpot] = item;
			uiSlotScript.InsertItem(item);

			/* Make objects disappear */
			if (other.gameObject.GetComponent<PhotonView>() == null) // No Photon View
				other.gameObject.SetActive(false);
			else { // Make object disappear
				other.gameObject.GetComponent<PhotonView>().RPC(
					"SetActive", 
					PhotonTargets.All, 
					new object[] {false}
				);
			}

			// Get turn effects if they exist
			if (item.GetTurnEffects() != null) 
				AttachTurnEffects(item.GetTurnEffects());

			// Apply effects to item on pickup
			applyItemEffect(item);

			/* Play item pickup effects */
			if (SoundManagerScript.Singleton != null) {
				// Move sound manager to this door
				SoundManagerScript.Singleton.gameObject.transform.position = gameObject.transform.position;
				SoundManagerScript.Singleton.PlaySingle3D(ItemPickupEfx);
			}

			// Increment to the next available spot
			while (availableSpot != 8 && inventory[availableSpot] != null)
				availableSpot++;
		} else if (other.gameObject.CompareTag("Trap")) { // Collided with a trap
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
			if (!effect.GetTurnEffectType().Equals(TurnEffectType.ITEMEFFECT)) // Not an item effect
				continue;
			if (!itemToAffect.GetType().Equals(effect.GetAffectedItemType())) // Not the effected item
				continue;
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
		case TurnEffectType.STATEFFECT: // Can only reset the vision stat
			if (effect.GetStatAffected() == Stat.VISION)
				stats[Stat.VISION] = playerClass.GetStat(Stat.VISION);
			break;
		case TurnEffectType.STATMULTIPLIEREFFECT: // Can rese the stat multiplier now
			playerClass.RestoreDefaultStat(effect.GetStatAffected());
			break;
		case TurnEffectType.ITEMEFFECT: // Need to reset affected items
			foreach (Item item in inventory) {
				if (item != null && item.GetType().Equals(effect.GetAffectedItemType())) { 
					/* Reset item cool down and turn settings */
					item.ResetCoolDown();
					item.ResetCoolDownSetting();
					item.ResetUsePerTurn();
					if (item.RemainingCoolDownTurns() == 0)  // Reduce the cool down of the item to be sure
						item.ReduceCoolDown();
				}
			}
			break;
		case TurnEffectType.MATERIALEFFECT: 
			GetComponent<PhotonView>().RPC("ResetMaterial", PhotonTargets.All, null);
			break;
		case TurnEffectType.MODELCHANGEEFFECT: goto case TurnEffectType.COMPONENTEFFECT;// Need complex detach routine
		case TurnEffectType.COMPONENTEFFECT: // Need complex detaching actions
			effect.ExtraDetachActions();
			break;
		default: break; // Unknown
		}
		effectPanelScript.RemoveTurnEffect(effect);
		turnEffects.Remove(effect);
	}

	/**
	 * Set all the renderers in the list to the given material
	 * 
	 * Arguments
	 * - Material materialToSet - The material to set
	 * - List<Renderer> renderers - The renderers
	 */
	void setRenderersToMaterial(Material materialToSet, List<Renderer> renderers) {
		foreach (Renderer singleRenderer in renderers)
			singleRenderer.material = materialToSet;
	}

	/**
	 * RPC call for resetting player material
	 */
	[PunRPC]
	public void ResetMaterial() {
		List<Renderer> renderers = new List<Renderer>(); // New list of renderers
		Material defaultMaterial = Resources.Load<Material>(playerMaterialPath); // The default material

		renderers.AddRange(gameObject.GetComponents<Renderer>());
		renderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());

		setRenderersToMaterial(defaultMaterial, renderers);
	}

	/**
	 * RPC call for changing material
	 * 
	 * Arguments
	 * - string materialPath - The path to the material
	 */
	[PunRPC]
	public void SetNewMaterial(string materialPath) {
		Material newMaterial = Resources.Load<Material>(materialPath); // The material
		List<Renderer> renderers = new List<Renderer>(); // New list of renderers

		if (newMaterial == null)
			return;

		renderers.AddRange(gameObject.GetComponents<Renderer>());
		renderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());

		setRenderersToMaterial(newMaterial, renderers);
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
			case TurnEffectType.STATMULTIPLIEREFFECT: // Stat multiplier effect
				stat = effect.GetStatAffected();
				mode = effect.GetMode();
				Debug.LogWarning("Stat affected: " + EnumsToString.ConvertStatEnum(stat));
				Debug.LogWarning("Previous value: " + playerClass.GetStat(stat));
				switch (mode) {
				case 0: // Increment
					playerClass.IncreaseStatMultiplierValue(stat, effect.GetValue());
					break;
				case 1: // Set stat
					playerClass.SetMultiplierStat(stat, effect.GetValue());
					break;
				case 2: // Decrement
					playerClass.DecreaseStatMultiplierValue(stat, effect.GetValue());
					break;
				default:
					throw new System.NotSupportedException("Invalid mode");
				}
				Debug.LogWarning("Current value: " + playerClass.GetStat(stat));
				break;
			case TurnEffectType.STATEFFECT: // Stat effect
				stat = effect.GetStatAffected();
				mode = effect.GetMode();
				Debug.LogWarning("Stat affected: " + EnumsToString.ConvertStatEnum(stat));
				Debug.LogWarning("Previous value: " + stats[stat]);
				switch (mode) {
				case 0: // Increment to stat
					IncreaseStatValue(stat, effect.GetValue());
					break;
				case 1: // Set stat
					SetStatValue(stat, effect.GetValue());
					break;
				case 2: // Multiply stat
					SetStatValue(stat, stats[stat] * effect.GetValue());
					break;
				default: // Invalid mode. Do nothing
					throw new System.NotSupportedException("Invalid mode");
				}
				Debug.LogWarning("Current value: " + stats[stat]);
				break;
			case TurnEffectType.MATERIALEFFECT:
				// Only replace material if not already set
				if (!PlayerObject.GetComponentInChildren<Renderer>().material.Equals(effect.GetMaterial())) {
					GetComponent<PhotonView>().RPC("SetNewMaterial", PhotonTargets.All, 
							new object[] {
								effect.GetMaterialPath()
							}
					);
				}
				break;// Change material
			case TurnEffectType.COMPONENTEFFECT: // Component effect. Handled by component
				break;
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
		for (int i = 0; i < 8; ++i) { 
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
		if (stats[playerStat] <= 0.0) // Set stat to 0. Can't be negative
			stats[playerStat] = 0.0;

		/*
		 * Add any constraint checking to this switch
		 */
		switch(playerStat) {
		case Stat.AP: goto default;
		case Stat.VISION:
			if (value < 1 || value > 3) // Invalid value for vision. Set to default
				stats[Stat.VISION] = 2;
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

	/**
	 * Update the light around this player based on any 
	 * effects/items the player has
	 */
	void UpdateVision() {
		if (playerLight == null) // No light exists
			return;
		if (stats[Stat.VISION] <= 1f && stats[Stat.VISION] >= 1f)
			playerLight.intensity = 1f;
		else if (stats[Stat.VISION] <= 2f && stats[Stat.VISION] >= 2f)
			playerLight.intensity = 2.3f;
		else if (stats[Stat.VISION] <= 3f && stats[Stat.VISION] >= 3f)
			playerLight.intensity = 4f;
	}

	/**
	 * Get the distance (in terms of tiles) that this player can see
	 * 
	 * Returns
	 * - The number of tiles the player can see
	 */
	public int GetVisionDistance() {
		if (stats[Stat.VISION] >= 0.9 && stats[Stat.VISION] <= 1.1)
			return 2;
		else if (stats[Stat.VISION] >= 1.9 && stats[Stat.VISION] <= 2.1)
			return 4;
		else if (stats[Stat.VISION] >= 2.9 && stats[Stat.VISION] <= 3.1) {
			Debug.Log("6");
			return 6;
		} else {
			Debug.LogError("Vision stat is wrong:" + stats[Stat.VISION]);
			return 2;
		}
	}

	/**
	 * RPC call to generate surveillance cameras
	 * 
	 * Arguments
	 * - Vector3 positionToSet - The position to set
	 */
	[PunRPC]
	public void InstantiateSurvCamera(Vector3 positionToSet) {
		GameObject instantiatedCamera = 
				Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/SurveillanceCamera")); // Camera

		instantiatedCamera.transform.position = positionToSet;
		instantiatedCamera.transform.SetParent(
			GameObject.FindGameObjectWithTag("SurveillanceCameraContainer").transform);
		instantiatedCamera.SetActive(false);
	}

	/**
	 * Returns the distance between this player and a tile.
	 * 
	 * Arguments
	 * - Tile tile - The tile to look at
	 * - bool stealth - Indicate if stealth needs to be taken into account. For optimisation
	 * 
	 * Returns
	 * - The number of tiles this player can see from their position
	 */
	public int DistanceToTile(Tile tile, bool stealth = false) {
		return GetComponent<MovementController>().TileDistance(tile, stealth);
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
		/* Re-initialize the class stats */
		playerClass.RestoreDefaultStats();
		SetStatValue(Stat.AP, playerClass.GetStat(Stat.AP));
		SetStatValue(Stat.VISION, playerClass.GetStat(Stat.VISION));
	}

	/**
	 * Remove the given item
	 * 
	 * Arguments
	 * - Item item - The item to be removed/dropped
	 * - bool toSetActive - check if this item was meant to be set active
	 */
	public void RemoveItem(Item item, bool toSetActive) {
		int itemIndex; // The index of the given item
		InventoryUISlotScript uiSlotScript; // The ui slot script

		if (item == null)
			return; // No item given

		itemIndex = getIndex(item);
		if (itemIndex == -1) 
			return; // This player doesn't have the item

		// Set game object to be behind the player and set it to active
		if (toSetActive) {
			if (physicalItems[itemIndex].GetComponent<PhotonView>() == null) { // No Photon View
				physicalItems[itemIndex].SetActive(true);
				physicalItems[itemIndex].transform.position = 
					new Vector3(transformComponent.position.x, (float)0.0, 
					            transformComponent.position.z);
			} else { // Set this player to be active
				physicalItems[itemIndex].GetComponent<PhotonView>().RPC(
					"SetActive", 
					PhotonTargets.All, 
					new object[] {true}
				);
				physicalItems[itemIndex].GetComponent<PhotonView>().RPC(
					"SetPosition",
					PhotonTargets.All,
					new object[] {transformComponent.position.x, transformComponent.position.z}
				);
			}

			// Add item to list of recently dropped items
			droppedItems.Add(physicalItems[itemIndex]);
		}

		// Remove effects if the item has some turn effects
		if (item.GetTurnEffects() != null) 
			foreach (Effect turnEffect in item.GetTurnEffects()) 
				DetachTurnEffect(turnEffect);
		
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
		while (availableSpot != 8 && inventory[availableSpot] != null)
			availableSpot++;
	}

	/**
	 * Function used to drop the given item currently in the context aware box
	 * 
	 * Arguments
	 * - GameObject contextAwareBox - The context aware box
	 */
	public void RemoveItem(GameObject contextAwareBox) {
		Item item = (Item)contextAwareBox.GetComponent<ContextAwareBox>().GetAttachedObject();
		RemoveItem(item, true);
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
		for (int i = 0; i < 8; ++i) { // Check each item
			if (inventory[i] == null) 
				continue; // Nothing here
			else if (inventory[i].Equals(itemToGet)) 
				return i;
		}
		return -1; // Item not found
	}

	/**
	 * Get the list of turn effects
	 * 
	 * Returns
	 * - the list of turn effects in this player
	 */
	public List<Effect> GetTurnEffects() {
		return turnEffects;
	}

	/**
	 * Apply turn effects attached to this object
	 */
	public void ApplyTurnEffects() {
		Effect currentEffect; // The current effect being applied
		if (turnEffectsApplied) 
			return;

		for (int i = 0; i < turnEffects.Count; ++i) { // Check each turn effect
			currentEffect = turnEffects[i];
			applyTurnEffect(turnEffects[i]);
			if (!turnEffects.Contains(currentEffect)) 
				--i; // Turn effect removed
		}

		turnEffectsApplied = true; // We have applied turn effects
	}

	/**
	 * Reduce the number of turns this player's ability is still active
	 */
	public void ReduceAbilityTurns() {
		AlienClass alienClass; // Container for the alien class
		if (playerClass.GetPrimaryAbility() == null) 
			return; // No ability

		if (playerClass.GetClassTypeEnum() == Classes.BETRAYER) { // Alien. Need to handle human ability
			alienClass = (AlienClass)playerClass;
			alienClass.GetHumanClass().GetPrimaryAbility().ReduceNumberOfTurns();
		}
		playerClass.GetPrimaryAbility().ReduceNumberOfTurns();
	}

	/**
	 * Reduce the cool downs for the items attached to this player
	 */
	public void ReduceItemCoolDowns() {
		foreach (Item item in inventory) {
			if (item == null) 
				continue;
			item.ReduceCoolDown();
		}
	}

	/**
	 * Get the tile position of this player
	 * 
	 * Returns
	 * - The position of this player in a tile
	 */
	public Tile PlayerPosition() {
		Tile pos = new Tile();
		pos.X = Tile.TilePosition(transform.position.x);
		pos.Z = Tile.TilePosition(transform.position.z);
		return pos;
	}

}
