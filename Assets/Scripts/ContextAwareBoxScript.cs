using UnityEngine;
using UnityEngine.UI;

public class ContextAwareBoxScript : MonoBehaviour {

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
	 */
	void Start() {
		SetContextToIdle();

		activationTileScript = GetComponent<ActivationTileController>();
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
				new Vector3((float)116, (float)-129.5);
			/* Set buttons to be inactive */
			InventoryContextPanel[2].SetActive(false);
			InventoryContextPanel[3].SetActive(false);
		} else { // Item given. Set everything to be active
			InventoryContextPanel[1].GetComponent<Text>().text = attachedObject.ToString();
			InventoryContextPanel[1].GetComponent<RectTransform>().anchoredPosition3D = 
					new Vector3((float)0, (float)0);
			/* Set buttons to be active, but if not activatable, don't activate the "Activate" button */
			if (item.IsActivatable()) { // Generate activate button
				InventoryContextPanel[2].SetActive(true);
			} else { // Don't generate activate button
				InventoryContextPanel[2].SetActive(false);
			}
			InventoryContextPanel[3].SetActive(true);
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
			if (item == null) return;
			if (item.RemainingCoolDownTurns() != 0) return;
			activationTileScript.GeneratorInterface(playerScript, item);
		}
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
