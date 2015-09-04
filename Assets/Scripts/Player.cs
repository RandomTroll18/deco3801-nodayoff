using System.Collections.Generic;
using UnityEngine;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	public GameObject[] InventoryUI = new GameObject[9]; // UI Slots
	public GameObject GameManagerObject; // The game manager object

	Item[] inventory = new Item[9]; // Inventory
	GameObject[] physicalItems = new GameObject[9]; // Items' Game Objects
	Transform transformComponent; // The transform component of this player
	Dictionary<Stat, double> stats;
	List<TurnEffect> turnEffects; // The turn-based effects attached to this player
	int availableSpot; // Earliest available spot in inventory
	const double DEFAULTAP = 10.0; // Default AP
	const double DEFAULTSTUN = 0.0; // Default stun
	const double DEFAULTVISION = 5.0; // Default vision
	bool turnEffectsApplied; // Record whether turn effects are applied
	GameManager gameManagerScript; // The game manager script
	bool noLongerActive; // Record if this player is still active
	bool semaphore; // Semaphore to ensure that a function is only called once

	/*
	 * Physics objects
	 */
	//private BoxCollider boxCollider; - Might not be needed
	//private Rigidbody rb;

	/**
	 * Do the following to start the player:
	 * - Get rigidbody component for movement purposes - not needed yet
	 * - Set all values in inventory to null
	 * - Get the transform component
	 * - Initialize available spots
	 * - Initialize stats
	 * - Initialize array list of turn effects
	 * - Get the Game Manager's script
	 * - Set turn effects applied variable to be false
	 * - Set that this player is not active
	 */
	void Start() {
		//this.rb = GetComponent<Rigidbody>();
		initializeInventory();
		transformComponent = GetComponent<Transform>();
		availableSpot = 0;
		stats = new Dictionary<Stat, double>();
		stats[Stat.AP] = DEFAULTAP;
		stats[Stat.STUN] = DEFAULTSTUN;
		stats[Stat.VISION] = DEFAULTVISION;

		turnEffects = new List<TurnEffect>();
		gameManagerScript = GameManagerObject.GetComponent<GameManager>();
		turnEffectsApplied = false;
		noLongerActive = true;
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
			if (noLongerActive) {
				// Record that the this player is no longer active
				gameManagerScript.SetInactivePlayer();
			}
			// Allow movement
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
				for (int i = 0; i < turnEffects.Count; ++i) {
					Debug.Log("Player effect " + i + ": " + turnEffects[i]);
				}
				Debug.Log("Added turn effects");
			}

			// Increment to the next available spot
			while (inventory[availableSpot] != null) {
				availableSpot++;
			}
		}
	}

	// TODO: Create function that applies/gets list of turn effects
	void getTurnEffects() {

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
	 * Reinitialize player stats
	 */
	public void InitializeStats() {
		stats[Stat.AP] = DEFAULTAP;
		stats[Stat.STUN] = DEFAULTSTUN;
		stats[Stat.VISION] = DEFAULTVISION;
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
				new Vector3((float)(transformComponent.position.x + 1.5), (float)0.0, 
			    transformComponent.position.z);

		// Remove effects if the item has some turn effects
		if (item.GetTurnEffects() != null) {
			foreach (TurnEffect turnEffect in item.GetTurnEffects()) {
				turnEffects.Remove(turnEffect);
			}
		}

		// Remove the item from the ui slot
		uiSlotScript = InventoryUI[itemIndex].GetComponent<InventoryUISlotScript>();
		uiSlotScript.RemoveItem();

		// Set inventory references to null
		inventory[itemIndex] = null;
		physicalItems[itemIndex] = null;

		// Set next available spot to be this slot
		availableSpot = itemIndex;
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
			if (inventory[i].Equals(itemToGet)) return i;
		}
		return -1; // Item not found
	}

	/**
	 * Apply turn effects attached to this object
	 */
	public void ApplyTurnEffects() {
		Stat stat; // The stat to effect
		TurnEffect turnEffect; // The current turn effect
		int mode; // The mode of this turn effect

		if (turnEffectsApplied) {
			Debug.Log("Effects already applied");
			return;
		}
		Debug.Log ("Start applying turn effects");

		for (int i = 0; i < turnEffects.Count; ++i) {
			turnEffect = turnEffects[i];
			stat = turnEffect.GetStatAffected();
			mode = turnEffect.GetMode();
			switch (mode) {
			case 0: // Increment to stat
				stats[stat] += turnEffect.GetValue();
				break;
			case 1: // Set stat
				stats[stat] = turnEffect.GetValue();
				break;
			case 2: // Multiply stat
				stats[stat] *= turnEffect.GetValue();
				break;
			default: // Invalid mode. Do nothing
				break;
			}
			stats[stat] += turnEffect.GetValue();
		}

		turnEffectsApplied = true; // We have applied turn effects
	}

	public Tile PlayerPosition() {
		Tile pos = new Tile();
		pos.X = Tile.TilePosition(transform.position.x);
		pos.Z = Tile.TilePosition(transform.position.z);
		return pos;
	}

}
