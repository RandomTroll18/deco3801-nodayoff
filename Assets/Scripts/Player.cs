using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	public float speed = 10f; // Move speed
	private Item[] inventory = new Item[10]; // Inventory
	public Text inventoryText ; // Inventory text

	/*
	 * Variable keeping track of the earliest available 
	 * spot in the inventory
	 */
	private int availableSpot = 0;

	/*
	 * Physics objects
	 */
	//private BoxCollider boxCollider; - Might not be needed
	//private Rigidbody rigidBody;

	/**
	 * Do the following to start the player:
	 * - Get rigidbody component for movement purposes
	 * - Set all valuesin inventory to null
	 */
	void Start () {
		//rigidBody = GetComponent<Rigidbody>();
		initializeInventory();
	}

	/**
	 * Function used to update things per frame
	 */
	void Update () {
		updateInventoryText();
	}
	
	// Update (every frame)
	void FixedUpdate () {
		float MoveForward = Input.GetAxis("Vertical") * 10 * Time.deltaTime;
		float MoveRotate = Input.GetAxis("Horizontal") * 100 * Time.deltaTime;
		
		transform.Translate(Vector3.forward * MoveForward);
		transform.Rotate(Vector3.up * MoveRotate);
	}

	/**
	 * Function handling collision with a trigger item
	 */
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Item")) {
			if (availableSpot == 10) return; // No more room
			// We collided with an item. Pick it up
			Item item = other.GetComponent<Item>();
			Debug.Log("Item just collided with: " + item.itemName);
			Debug.Log("Item toString: " + other.GetComponent<Item>());
			this.inventory[availableSpot] = other.GetComponent<Item>();
			other.gameObject.SetActive(false); // Make object disappear
			availableSpot++; // Increment available spot
		}
	}

	/**
	 * Function used to initialize inventory array
	 */
	void initializeInventory () {
		for (int i = 0; i < 10; ++i) this.inventory[i] = null;
	}

	/**
	 * Function used to update the inventory text
	 */
	public void updateInventoryText () {
		// Newline character
		string newline = System.Environment.NewLine;

		// First, start with "Inventory: " 
		this.inventoryText.text = "Inventory: " + newline + newline;

		if (this.availableSpot == 0) {
			this.inventoryText.text += "No items in your inventory" + newline;
			return;
		}
		// Second, print out the string of all the inventory items
		for (int i = 0; i < this.availableSpot; ++i) {
			this.inventoryText.text += this.inventory[i] + newline;
		}
	}

}
