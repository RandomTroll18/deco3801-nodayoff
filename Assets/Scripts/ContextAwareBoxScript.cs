using UnityEngine;
using System.Collections;

public class ContextAwareBoxScript : MonoBehaviour {

	// Inventory Context Panel
	public GameObject inventoryContextPanel;

	/**
	 * Set the context to inventory
	 * 
	 * Arguments
	 * - Item item - The item selected
	 */
	public void setContextToInventory (Item item) {
		// First, set the inventory context panel to active
		this.inventoryContextPanel.SetActive(true);
	}

	/**
	 * Set context to idle
	 */
	public void setContextToIdle () {
		this.inventoryContextPanel.SetActive(false);
	}
}
