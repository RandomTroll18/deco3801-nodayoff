using UnityEngine;
using System.Collections;

/**
 * Script used by the panel containing all the ui slots
 */
public class InventoryUIScript : MonoBehaviour {

	public GameObject contextAwareBox = null; // Context aware box

	// The script of the currently selected ui slot
	private InventoryUISlotScript currentSelected = null;
	// The script of the context aware box
	private ContextAwareBoxScript contextBoxScript;

	/**
	 * Starting function
	 */
	void Start () {
		if (this.contextAwareBox != null) {
			this.contextBoxScript = 
				this.contextAwareBox.GetComponent<ContextAwareBoxScript>();
		}
	}

	/**
	 * Function which sets the currently selected slot. 
	 * Makes sure that only one ui slot is set as selected. 
	 * Also sets the context
	 * 
	 * Arguments
	 * - GameObject newSelected - The newly selected slot
	 */
	public void setSelected (GameObject newSelected) {
		// The script of the newly selected slot
		InventoryUISlotScript newSelectedScript = 
				newSelected.GetComponent<InventoryUISlotScript>();
		Item attachedItem = newSelectedScript.getItem(); // Item attached to the ui slot
		int selected = newSelectedScript.toggleSelected();  // Result of toggling ui slot
		if (selected == -1) return;// Error in initialization of ui slot
		else if (selected == 0) { // Nothing is selected
			this.contextBoxScript.setContextToIdle(); // Set to idle context
			this.currentSelected = null;
		} else { // This slot was selected
			if (isSlotSelected()) { // A slot was selected 
				this.currentSelected.toggleSelected();
			}
			// Set/update context to inventory context
			this.contextBoxScript.setContextToInventory(attachedItem);
			this.currentSelected = newSelectedScript;
		}
	}

	/**
	 * Function which deselects the currently selected slot
	 */
	public void deselectAll () {
		this.currentSelected.toggleSelected();
		this.currentSelected = null; // Set the currently selected to null
		this.contextBoxScript.setContextToIdle(); // Set context to idle
	}

	/**
	 * Function which returns whether or not a slot has been 
	 * currently selected
	 * 
	 * Return
	 * - true if a slot is currently selected
	 * - false if otherwise
	 */
	private bool isSlotSelected () {
		if (this.currentSelected == null) return false;
		else return true;
	}

}
