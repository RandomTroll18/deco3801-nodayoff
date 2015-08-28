using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Class inherited by all players
 */
public class Player : MonoBehaviour {

	public float speed = 10f; // Move speed

	// TODO: make another class
	public GameObject leftCollider;
	public GameObject rightCollider;
	public GameObject upCollider;
	public GameObject downCollider;
	private MapCollisionDetector leftDetector;
	private MapCollisionDetector rightDetector;
	private MapCollisionDetector upDetector;
	private MapCollisionDetector downDetector;


	private Item[] inventory = new Item[10]; // Inventory
	/* Array containing references to the actual physical items that were picked up */
	private GameObject[] physicalItems = new GameObject[10];
	/* Array containing references to inventory UI slots */
	public GameObject[] inventoryUI = new GameObject[10];
	/* The default icon for inventory slots */
	private Sprite defaultIcon;


	private int availableSpot = 0; //earliest available spot in inventory

	/*
	 * Physics objects
	 */
	//private BoxCollider boxCollider; - Might not be needed
	private Rigidbody rigidbody;

	/**
	 * Do the following to start the player:
	 * - Get rigidbody component for movement purposes
	 * - Set all valuesin inventory to null
	 */
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		initializeInventory();
		this.defaultIcon = Resources.Load<Sprite>("Background");

		leftDetector = leftCollider.GetComponent <MapCollisionDetector> ();
		rightDetector = rightCollider.GetComponent <MapCollisionDetector> ();
		upDetector = upCollider.GetComponent <MapCollisionDetector> ();
		downDetector = downCollider.GetComponent <MapCollisionDetector> ();
	}

	void Update () {

	}

	void FixedUpdate() {
		Vector3? newPos =  null;
		Vector3 currentPos = transform.position;
		if (Input.GetKeyDown("w") && !upDetector.isColliding ()) {
			rigidbody.MovePosition(new Vector3(currentPos.x, currentPos.y, currentPos.z + 1));
		} else if (Input.GetKeyDown ("s") && !downDetector.isColliding ()) {
			rigidbody.MovePosition(new Vector3(currentPos.x, currentPos.y, currentPos.z - 1));
		} else if (Input.GetKeyDown ("a") && !leftDetector.isColliding ()) {
			rigidbody.MovePosition(new Vector3(currentPos.x - 1, currentPos.y, currentPos.z));
		} else if (Input.GetKeyDown ("d") && !rightDetector.isColliding ()) {
			rigidbody.MovePosition(new Vector3(currentPos.x + 1, currentPos.y, currentPos.z));
		}
		if (newPos.HasValue)
			transform.position = newPos.Value;
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

}
