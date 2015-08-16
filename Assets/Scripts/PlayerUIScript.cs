using UnityEngine;
using System.Collections;

public class PlayerUIScript : MonoBehaviour {

	/**
	 * Make the inventory interface appear
	 * 
	 * Arguments
	 * - GameObject InventoryInterface - The inventory interface
	 */
	public void toggleInventory (GameObject InventoryInterface) {
		if (InventoryInterface.activeInHierarchy) {
			InventoryInterface.SetActive(false);
		} else {
			InventoryInterface.SetActive(true);
		}
	}

	/**
	 * Enable/Disable panning of the camera
	 * 
	 * Arguments
	 * - GameObject mainCamera - The main camera object
	 */
	public void togglePanning (GameObject mainCamera) {
		if (mainCamera.GetComponent<CameraController>().enabled) {
			mainCamera.GetComponent<CameraController>().enabled = false;
		} else {
			mainCamera.GetComponent<CameraController>().enabled = true;
		}
	}
}
