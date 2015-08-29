using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	private Item[] inventory = new Item[10]; // Inventory
	private GameObject[] physicalItems = new GameObject[10]; // Items' Game Objects
	public GameObject[] inventoryUI = new GameObject[10]; // UI Slots
	//private Sprite defaultIcon; // Default icon for inventory slots


	private int availableSpot = 0; //earliest available spot in inventory

	/*
	 * Physics objects
	 */
	//private BoxCollider boxCollider; - Might not be needed
	private Rigidbody rb;

	/**
	 * Do the following to start the player:
	 * - Get rigidbody component for movement purposes
	 * - Set all valuesin inventory to null
	 */
	void Start () {
		this.rb = GetComponent<Rigidbody>();
		initializeInventory();
		//this.defaultIcon = Resources.Load<Sprite>("Background");
	}

	/**
	 * Function handling collision with a trigger item
	 */
	void OnTriggerEnter (Collider other) {
		InventoryUISlotScript uiSlotScript; // The ui slot script
		if (other.gameObject.CompareTag("Item")) {
			if (availableSpot == 10) return; // No more room
			// Get the ui slot script
			uiSlotScript = 
					this.inventoryUI[availableSpot].GetComponent<InventoryUISlotScript>();
			// We collided with an item. Pick it up
			this.physicalItems[availableSpot] = other.gameObject;
			Item item = other.GetComponent<Item>();
			Debug.Log("Item just collided with: " + item.itemName);
			Debug.Log("Item toString: " + other.GetComponent<Item>());
			this.inventory[availableSpot] = other.GetComponent<Item>();
			Debug.Log("Item image: " + item.image);
			this.inventoryUI[availableSpot].GetComponent<Image>().sprite = item.image;
			uiSlotScript.insertItem(item);
			other.gameObject.SetActive(false); // Make object disappear
			availableSpot++; // Increment available spot
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

}
