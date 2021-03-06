﻿using UnityEngine;
using UnityEngine.UI;

/**
 * Script handling individual ui slots
 */
public class InventoryUISlotScript : MonoBehaviour {
	
	public GameObject UiSlot; // The ui slot. Initially null
	public GameObject ContextAwareBox; // The context aware box
	public GameObject Container; // The parent of this ui slot
	public Sprite DefaultIcon; // The default icon
	bool selected; // Record whether ui element was clicked
	Image uiSlotImage; // The image script of the ui slot
	Item item; // The item contained in this slot
	ContextAwareBox contextBoxScript; // Script of context aware box
	InventoryUIScript containerScript; // Script of parent

	/**
	 * Start function. Need to do the following:
	 * - Initialize variables only if the game objects have been set
	 * - Get the default icon which is "Background"
	 */
	public void StartMe() {
		selected = false;
		if (UiSlot != null)
			uiSlotImage = UiSlot.GetComponent<Image>();
		if (ContextAwareBox != null)
			contextBoxScript = ContextAwareBox.GetComponent<ContextAwareBox>();
		if (Container != null)
			containerScript = Container.GetComponent<InventoryUIScript>();
	}

	/**
	 * Toggle the selected status of this inventory slot
	 * 
	 * Returns
	 * - 1 if the slot became selected
	 * - 0 if otherwise
	 * - -1 if there was a problem initializing the script
	 */
	public int ToggleSelected() {
		Color iconColour = uiSlotImage.color; // Colour of the ui slot

		if (uiSlotImage == null || contextBoxScript == null || containerScript == null) 
			return -1; // Do nothing
		if (!selected) // Make ui slot image opaque
			iconColour.a += 155;
		else // Make ui slot image transluscent
			iconColour.a -= 155;

		UiSlot.GetComponent<Image>().color = iconColour;
		selected = !selected;
		if (selected) 
			return 1;
		else 
			return 0;
	}

	/**
	 * Insert item into this slot
	 * 
	 * Arguments
	 * - Item item - The item to insert
	 */
	public void InsertItem(Item itemToInsert) {
		if (uiSlotImage == null || contextBoxScript == null || containerScript == null) 
			return; // Do nothing
		item = itemToInsert;
		UiSlot.GetComponent<Image>().sprite = item.Image; // Update icon to be the item's icon
		if (selected)  // Update inventory context
			contextBoxScript.SetContextToInventory(item);
	}

	/**
	 * Remove item from this slot
	 */
	public void RemoveItem() {
		if (uiSlotImage == null || contextBoxScript == null || containerScript == null) 
			return; // Do nothing
		item = null;
		UiSlot.GetComponent<Image>().sprite = DefaultIcon;
		if (selected) // Update inventory context
			contextBoxScript.SetContextToInventory(item);
	}

	/**
	 * Return the item in this slot
	 * 
	 * Returns
	 *  - The item attached to this slot
	 */
	public Item GetItem() {
		return item;
	}

	/**
	 * Check if the ui slot was selected
	 * 
	 * Return
	 * - true if the ui slot is selected
	 * - false if otherwise
	 */
	public bool IsSelected() {
		return selected;
	}
}
