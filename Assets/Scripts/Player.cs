using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	public GameObject[] inventoryUI = new GameObject[10]; // UI Slots
	public GameObject gameManagerObject; // The game manager object

	private Item[] inventory = new Item[10]; // Inventory
	private GameObject[] physicalItems = new GameObject[10]; // Items' Game Objects
	private Transform transformComponent; // The transform component of this player
	private double[] stats; // The stats of this player
	private ArrayList turnEffects; // The turn-based effects attached to this player
	private int availableSpot = 0; // Earliest available spot in inventory
	private const double DEFAULTHP = 100.0; // Default HP
	private const double DEFAULTAP = 10.0; // Default AP
	private bool turnEffectsApplied; // Record whether turn effects are applied
	private GameManager gameManagerScript; // The game manager script
	private bool noLongerActive; // Record if this player is still active
	private bool semaphore; // Semaphore to ensure that a function is only called once

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
	 * - Initialize stats
	 * - Initialize array list of turn effects
	 * - Get the Game Manager's script
	 * - Set turn effects applied variable to be false
	 * - Set that this player is not active
	 */
	void Start () {
		//this.rb = GetComponent<Rigidbody>();
		initializeInventory();
		transformComponent = GetComponent<Transform>();

		this.stats = new double[3];
		this.stats[(int)Stat.HP] = DEFAULTHP;
		this.stats[(int)Stat.AP] = DEFAULTAP;

		this.turnEffects = new ArrayList();
		this.gameManagerScript = 
				this.gameManagerObject.GetComponent<GameManager>();
		this.turnEffectsApplied = false;
		this.noLongerActive = true;
	}

	/**
	 * Update function. Needs to be done:
	 * - if we are in an invalid turn, reset values to default
	 * - if we are in a valid turn and we are active, 
	 * apply turn effects (if not activated yet) and allow movement
	 * - if we are in a valid turn but we are not active, don't 
	 * allow movement
	 */
	void Update () {
		if (!this.gameManagerScript.isValidTurn()) {
			Debug.Log("Player is not in a valid turn");
			this.turnEffectsApplied = false;
			this.noLongerActive = false;
		}
		else {
			if (this.noLongerActive) {
				// Record that the this player is no longer active
				this.gameManagerScript.setInactivePlayer();
			}
			// Allow movement
		}
	}

	/**
	 * Function handling collision with a trigger item
	 */
	void OnTriggerEnter (Collider other) {
		InventoryUISlotScript uiSlotScript; // The ui slot script
		Item item; // The item attached to a game object
		if (other.gameObject.CompareTag("Item")) {
			if (availableSpot == 10) return; // No more room

			// Get the ui slot script
			uiSlotScript = 
					this.inventoryUI[availableSpot].GetComponent<InventoryUISlotScript>();

			// We collided with an item. Pick it up
			this.physicalItems[availableSpot] = other.gameObject;
			item = other.GetComponent<Item>();
			Debug.Log("Item just collided with: " + item.itemName);
			Debug.Log("Item toString: " + other.GetComponent<Item>());
			this.inventory[availableSpot] = item;
			Debug.Log("Item image: " + item.image);
			uiSlotScript.insertItem(item);
			other.gameObject.SetActive(false); // Make object disappear

			// Get turn effects if they exist
			if (item.getTurnEffects() != null) {
				this.turnEffects.AddRange(item.getTurnEffects());
				for (int i = 0; i < this.turnEffects.Count; ++i) {
					Debug.Log("Player effect " + i + ": " + this.turnEffects[i]);
				}
				Debug.Log("Added turn effects");
			}

			// Increment to the next available spot
			while (this.inventory[availableSpot] != null) {
				availableSpot++;
			}
		}
	}

	/**
	 * Function used to initialize inventory array
	 */
	private void initializeInventory () {
		for (int i = 0; i < 10; ++i) { 
			this.inventory[i] = null;
			this.physicalItems[i] = null;
		}
	}

	/**
	 * Reinitialize player stats
	 */
	public void initializeStats () {
		this.stats[(int)Stat.AP] = DEFAULTAP;
	}

	/**
	 * Function used to drop the given item
	 * 
	 * Arguments
	 * - GameObject contextAwareBox - The context aware box
	 */
	public void dropItem (GameObject contextAwareBox) {
		Item item = 
				(Item)contextAwareBox
				.GetComponent<ContextAwareBoxScript>()
				.getAttachedObject();
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
		this.physicalItems[itemIndex].SetActive(true);
		this.physicalItems[itemIndex].transform.position = 
				new Vector3((float)(this.transformComponent.position.x + 1.5), 
			    (float)0.0, 
				(float)this.transformComponent.position.z);

		// Remove effects if the item has some turn effects
		if (item.getTurnEffects() != null) {
			foreach (TurnEffect turnEffect in item.getTurnEffects()) {
				this.turnEffects.Remove(turnEffect);
			}
		}

		// Remove the item from the ui slot
		uiSlotScript = 
				this.inventoryUI[itemIndex].GetComponent<InventoryUISlotScript>();
		uiSlotScript.removeItem();

		// Set inventory references to null
		this.inventory[itemIndex] = null;
		this.physicalItems[itemIndex] = null;

		// Set next available spot to be this slot
		this.availableSpot = itemIndex;
		Debug.Log ("Item: " + item.itemName + " Dropped");
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
	private int getIndex (Item item) {
		for (int i = 0; i < 10; ++i) {
			Debug.Log("Getting index: " + this.inventory[i]);
			if (this.inventory[i].Equals(item)) return i;
		}
		return -1; // Item not found
	}

	/**
	 * Apply turn effects attached to this object
	 */
	public void applyTurnEffects () {
		if (this.turnEffectsApplied) {
			Debug.Log("Effects already applied");
			return;
		}
		Debug.Log ("Start applying turn effects");
		int stat; // The stat to effect
		TurnEffect turnEffect; // The current turn effect
		if (this.turnEffects.Count == 0) {
			Debug.Log("No turn effects");
			this.turnEffectsApplied = true;
			return; // No effects
		}
		for (int i = 0; i < this.turnEffects.Count; ++i) {
			turnEffect = (TurnEffect)this.turnEffects[i];
			stat = turnEffect.getStatAffected();
			this.stats[stat] += turnEffect.getValue();
		}
		Debug.Log("Turn effects applied");
		Debug.Log("New HP: " + this.stats[(int)Stat.HP]);
		Debug.Log("New AP: " + this.stats[(int)Stat.AP]);
		this.turnEffectsApplied = true; // We have applied turn effects
	}

}
