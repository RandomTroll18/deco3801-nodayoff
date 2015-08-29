using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	public GameObject[] inventoryUI = new GameObject[10]; // UI Slots

	private Item[] inventory = new Item[10]; // Inventory
	private GameObject[] physicalItems = new GameObject[10]; // Items' Game Objects
	private Transform transformComponent; // The transform component of this player
	private double[] stats; // The stats of this player
	private ArrayList turnEffects; // The turn-based effects attached to this player
	//private Sprite defaultIcon; // Default icon for inventory slots


	private int availableSpot = 0; //earliest available spot in inventory

	/*
	 * Physics objects
	 */
	//private BoxCollider boxCollider; - Might not be needed
	//private Rigidbody rb;

	/**
	 * Do the following to start the player:
	 * - Get rigidbody component for movement purposes - not needed yet
	 * - Set all valuesin inventory to null
	 * - Get the transform component
	 * - Initialize stats
	 * - Initialize array list of turn effects
	 */
	void Start () {
		//this.rb = GetComponent<Rigidbody>();
		initializeInventory();
		//this.defaultIcon = Resources.Load<Sprite>("Background");
		transformComponent = GetComponent<Transform>();

		this.stats = new double[3];
		this.stats[(int)Stat.HP] = 100.0;
		this.stats[(int)Stat.AP] = 10.0;

		this.turnEffects = new ArrayList();
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
			this.inventory[availableSpot] = other.GetComponent<Item>();
			Debug.Log("Item image: " + item.image);
			this.inventoryUI[availableSpot].GetComponent<Image>().sprite = item.image;
			uiSlotScript.insertItem(item);
			other.gameObject.SetActive(false); // Make object disappear

			// Get turn effects if they exist
			if (item.getTurnEffects() != null) {
				this.turnEffects.AddRange(item.getTurnEffects());
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
	void initializeInventory () {
		for (int i = 0; i < 10; ++i) { 
			this.inventory[i] = null;
			this.physicalItems[i] = null;
		}
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
			this.transformComponent.position;

		// Remove effects if the item has some turn effects
		if (item.getTurnEffects() != null) {
			foreach (TurnEffect turnEffect in item.getTurnEffects()) {
				this.turnEffects.Remove(turnEffect);
			}
		}

		// Set inventory references to null
		this.inventory[itemIndex] = null;
		this.physicalItems[itemIndex] = null;
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
			if (this.inventory[i].Equals(item)) return i;
		}
		return -1; // Item not found
	}

	/**
	 * Apply turn effects attached to this object
	 */
	private void applyTurnEffects () {
		int stat; // The stat to effect
		TurnEffect turnEffect; // The current turn effect
		if (this.turnEffects.Count == 0) return; // No effects
		for (int i = 0; i < this.turnEffects.Count; ++i) {
			turnEffect = (TurnEffect)this.turnEffects[i];
			stat = turnEffect.getStatAffected();
			this.stats[stat] += turnEffect.getValue();
		}
	}

}
