using UnityEngine;

/**
 * Script used by the panel containing all the ui slots
 */
public class InventoryUIScript : MonoBehaviour {

	public GameObject ContextAwareBox; // Context aware box

	InventoryUISlotScript currentSelected; // The script of the currently selected ui slot
	ContextAwareBox contextBoxScript; // The script of the context aware box

	/**
	 * Starting function
	 */
	public void StartMe() {
		if (ContextAwareBox != null) {
			contextBoxScript = ContextAwareBox.GetComponent<ContextAwareBox>();
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
	public void SetSelected(GameObject newSelected) {
		InventoryUISlotScript newSelectedScript = 
			newSelected.GetComponent<InventoryUISlotScript>(); // The script of the newly selected slot
		Item attachedItem = newSelectedScript.GetItem(); // Item attached to the ui slot
		int selected = newSelectedScript.ToggleSelected();  // Result of toggling ui slot
		if (selected == -1)
			return;// Error in initialization of ui slot
		else if (selected == 0) { // Nothing is selected
			contextBoxScript.SetContextToIdle(); // Set to idle context
			currentSelected = null;
			return;
		}
		if (IsSlotSelected()) // A slot was selected 
			currentSelected.ToggleSelected();
		/* Set/update context to inventory context */
		contextBoxScript.SetContextToInventory(attachedItem);
		currentSelected = newSelectedScript;
	}

	/**
	 * Function which deselects the currently selected slot
	 */
	public void DeselectAll() {
		currentSelected.ToggleSelected();
		currentSelected = null; // Set the currently selected to null
		contextBoxScript.SetContextToIdle(); // Set context to idle
	}

	/**
	 * Function which returns whether or not a slot has been 
	 * currently selected
	 * 
	 * Return
	 * - true if a slot is currently selected
	 * - false if otherwise
	 */
	bool IsSlotSelected() {
		if (currentSelected == null) return false;
		return true;
	}

}
