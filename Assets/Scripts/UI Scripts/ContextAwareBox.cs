using UnityEngine;
using UnityEngine.UI;

public class ContextAwareBox : MonoBehaviour {

	/*
	 * Array containing various elements of inventory panel
	 * 0 = Panel
	 * 1 = Text
	 * 2 = Activate Button
	 * 3 = Drop Button
	 */
	public GameObject[] InventoryContextPanel = new GameObject[4];

	ActivationTileController activationTileScript; // Script for Activation Tiles
	Context currentContext; // The current context
	Object attachedObject; // Object currently attached

	/**
	 * Start function. Needs the following: 
	 * - Initialize current context to be idle
	 * - Initialize any other private variables
	 * - Create onclick listeners for buttons
	 */
	public void StartMe() {
		Player playerScript = Player.MyPlayer.GetComponent<Player>();

		SetContextToIdle();
		InventoryContextPanel[2].GetComponent<Button>().onClick.AddListener(
			() => ActivateAttachedItem(Player.MyPlayer));
		InventoryContextPanel[3].GetComponent<Button>().onClick.AddListener(
			() => playerScript.RemoveItem(gameObject));

		activationTileScript = GetComponent<ActivationTileController>();
		activationTileScript.StartMe();
	}

	/**
	 * Set the context to inventory
	 * 
	 * Arguments
	 * - Item item - The item selected
	 */
	public void SetContextToInventory(Item item) {
		if (currentContext != Context.INVENTORY) 
			currentContext = Context.INVENTORY;
		// First, set the inventory context panel to active
		InventoryContextPanel[0].SetActive(true);

		attachedObject = item; // Next attach item

		// Next, set the text
		if (attachedObject == null) { // No item given
			InventoryContextPanel[1].GetComponent<Text>().text = "No Item In Slot" + StringMethodsScript.NEWLINE;
			/* Change text position */
			InventoryContextPanel[1].GetComponent<RectTransform>().anchoredPosition3D = 
				new Vector3(0f, 0f);
			/* Set buttons to be inactive */
			InventoryContextPanel[2].SetActive(false);
			InventoryContextPanel[3].SetActive(false);
		} else { // Item given. Set everything to be active
			InventoryContextPanel[1].GetComponent<Text>().text = attachedObject.ToString();
			InventoryContextPanel[1].GetComponent<RectTransform>().anchoredPosition3D = 
					new Vector3(0f, 0f);
			/* Set buttons to be active, but if not activatable, don't activate the "Activate" button */
			InventoryContextPanel[2].SetActive(item.IsActivatable());
			InventoryContextPanel[3].SetActive(item.IsDroppable());
		}
	}

	/**
	 * Set context to idle
	 */
	public void SetContextToIdle() {
		InventoryContextPanel[0].SetActive(false);
		currentContext = Context.IDLE;
		attachedObject = null; // No object should be attached
	}

	/**
	 * Function used to activate the item. Interface for 
	 * "Activate" Button. The only objects that this 
	 * should be used for are:
	 * - Items
	 * - Interactive Objects
	 * 
	 * Arguments
	 * - GameObject playerObject - The player
	 */
	public void ActivateAttachedItem(GameObject playerObject) {
		Item item; // The item to be activated
		Player playerScript = playerObject.GetComponent<Player>();
	
		if (currentContext == Context.INVENTORY) {
			// We are in the inventory context
			item = (Item)attachedObject;
			if (item == null) 
				return;
			if (item.RemainingCoolDownTurns() != 0) 
				return;
			if (playerObject.GetComponent<MovementController>().IsMoving()) // Don't activate. We are moving
				return;
			if (item.GetRange() > 0) // Need activation tiles
				activationTileScript.GeneratorInterface(playerScript, item);
			else // Just activate item
				item.Activate();
			playerObject.GetComponent<MovementController>().ClearPath();
		}
	}

	/**
	 * Update the text of the attached object
	 */
	public void UpdateAttachedObjectText() {
		if (attachedObject != null)
			InventoryContextPanel[1].GetComponent<Text>().text = attachedObject.ToString();
	}

	/**
	 * Function used to return the currently attached object
	 * 
	 * Returns
	 * - The object attached to the context aware box
	 */
	public Object GetAttachedObject() {
		return attachedObject;
	}
}
