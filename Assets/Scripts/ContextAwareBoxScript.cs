using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContextAwareBoxScript : MonoBehaviour {

	/*
	 * Array containing various elements of inventory panel
	 * 0 = Panel
	 * 1 = Text
	 * 2 = Activate Button
	 * 3 = Drop Button
	 */
	public GameObject[] inventoryContextPanel = new GameObject[4];
	private Context currentContext; // The current context
	private Object attachedObject; // Object currently attached

	/**
	 * Start function. Needs the following: 
	 * - Initialize current context to be idle
	 * - Initialize any other private variables
	 */
	void Start () {
		setContextToIdle();
	}

	/**
	 * Set the context to inventory
	 * 
	 * Arguments
	 * - Item item - The item selected
	 */
	public void setContextToInventory (Item item) {
		if (this.currentContext != Context.INVENTORY) 
			this.currentContext = Context.INVENTORY;
		// First, set the inventory context panel to active
		this.inventoryContextPanel[0].SetActive(true);

		this.attachedObject = item; // Next attach item

		// Next, set the text
		if (this.attachedObject == null) { // No item given
			this.inventoryContextPanel[1].GetComponent<Text>().text = 
				"No Item In Slot" + StringMethodsScript.NEWLINE;
			/* Change text position */
			this.inventoryContextPanel[1]
					.GetComponent<RectTransform>().anchoredPosition3D = 
				new Vector3((float)0, (float)-7.629395e-06);
			/* Set buttons to be inactive */
			this.inventoryContextPanel[2].SetActive(false);
			this.inventoryContextPanel[3].SetActive(false);
		} else { // Item given. Set everything to be active
			this.inventoryContextPanel[1].GetComponent<Text>().text = 
				this.attachedObject.ToString();
			this.inventoryContextPanel[1]
					.GetComponent<RectTransform>().anchoredPosition3D = 
					new Vector3((float)80, (float)-15);
			/* Set buttons to be active */
			this.inventoryContextPanel[2].SetActive(true);
			this.inventoryContextPanel[3].SetActive(true);
		}
	}

	/**
	 * Set context to idle
	 */
	public void setContextToIdle () {
		this.inventoryContextPanel[0].SetActive(false);
		this.currentContext = Context.IDLE;
		this.attachedObject = null; // No object should be attached
	}

	/**
	 * Function used to activate the item. Interface for 
	 * "Activate" Button. The only objects that this 
	 * should be used for are:
	 * - Items
	 * - Interactive Objects
	 */
	public void activateAttachedItem () {
		Item item; // The item to be activated
		if (this.currentContext == Context.INVENTORY) {
			// We are in the inventory context
			item = (Item)this.attachedObject;
			if (item == null) {
				Debug.Log ("No Item Given");
				return;
			}
			item.Activate();
		}
	}

	/**
	 * Function used to return the currently attached object
	 * 
	 * Returns
	 * - The object attached to the context aware box
	 */
	public Object getAttachedObject () {
		return this.attachedObject;
	}
}
