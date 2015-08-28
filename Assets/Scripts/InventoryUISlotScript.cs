using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Script handling individual ui slots
 */
public class InventoryUISlotScript : MonoBehaviour {

	private bool selected = false; // Record whether ui element was clicked
	public GameObject uiSlot = null; // The ui slot. Initially null
	private Image uiSlotImage = null; // The image script of the ui slot

	/**
	 * Start function. Need to do the following:
	 * - Initialize uiSlotImage variable only if a 
	 * uiSlot is given
	 */
	void Start () {
		if (this.uiSlot != null) {
			this.uiSlotImage = this.uiSlot.GetComponent<Image>();
		}
	}

	/**
	 * Toggle the selected status of this inventory slot
	 */
	public void toggleSelected () {
		/* The colour class of the ui slot */
		Color iconColour = this.uiSlotImage.color;
		if (this.uiSlotImage == null) return; // Do nothing
		if (!selected) { // Make ui slot image opaque
			iconColour.a += 155;
		} else { // Make ui slot image transluscent
			iconColour.a -= 155;
		}
		this.uiSlot.GetComponent<Image>().color = iconColour;
		selected = !selected;
	}

}
