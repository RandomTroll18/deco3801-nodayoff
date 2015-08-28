using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Script handling individual ui slots
 */
public class InventoryUISlotScript : MonoBehaviour {
	
	public GameObject uiSlot = null; // The ui slot. Initially null
	public GameObject contextAwareBox = null; // The context aware box
	public GameObject container = null; // The parent of this ui slot
	private bool selected = false; // Record whether ui element was clicked
	private Image uiSlotImage = null; // The image script of the ui slot
	private Item item = null; // The item contained in this slot
	private ContextAwareBoxScript contextBoxScript = null; // Script of context aware box
	private InventoryUIScript containerScript = null; // Script of parent

	/**
	 * Start function. Need to do the following:
	 * - Initialize variables only if the game objects have been set
	 */
	void Start () {
		if (this.uiSlot != null) {
			this.uiSlotImage = this.uiSlot.GetComponent<Image>();
		}
		if (this.contextAwareBox != null) {
			this.contextBoxScript = 
					this.contextAwareBox.GetComponent<ContextAwareBoxScript>();
		}
		if (this.container != null) {
			this.containerScript = 
					this.container.GetComponent<InventoryUIScript>();
		}
	}

	/**
	 * Toggle the selected status of this inventory slot
	 * 
	 * Returns
	 * - 1 if the slot became selected
	 * - 0 if otherwise
	 * - -1 if there was a problem initializing the script
	 */
	public int toggleSelected () {
		Color iconColour = this.uiSlotImage.color; // Colour of the ui slot

		if (this.uiSlotImage == null || this.contextBoxScript == null 
				|| this.containerScript == null) return -1; // Do nothing
		if (!selected) { // Make ui slot image opaque
			iconColour.a += 155;
		} else { // Make ui slot image transluscent
			iconColour.a -= 155;
		}
		this.uiSlot.GetComponent<Image>().color = iconColour;
		selected = !selected;
		if (selected) return 1;
		else return 0;
	}

	/**
	 * Insert item into this slot
	 * 
	 * Arguments
	 * - Item item - The item to insert
	 */
	public void insertItem (Item item) {
		if (this.uiSlotImage == null || this.contextBoxScript == null 
		    || this.containerScript == null) return; // Do nothing
		this.item = item;
		Debug.Log ("Item inserted: " + item);
		// If the slot is selected, update inventory context
		if (selected) this.contextBoxScript.setContextToInventory(this.item);
	}

	/**
	 * Remove item from this slot
	 */
	public void removeItem () {
		if (this.uiSlotImage == null || this.contextBoxScript == null 
		    || this.containerScript == null) return; // Do nothing
		this.item = null;
		// If the slot is selected, update inventory context
		if (selected) this.contextBoxScript.setContextToInventory(this.item);
	}

	/**
	 * Return the item in this slot
	 * 
	 * Returns
	 *  - The item attached to this slot
	 */
	public Item getItem () {
		if (this.item == null) return null;
		return this.item;
	}
}
