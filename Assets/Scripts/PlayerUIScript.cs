using UnityEngine;

public class PlayerUIScript : MonoBehaviour {

	/**
	 * Enable/Disable panning of the camera
	 * 
	 * Arguments
	 * - GameObject mainCamera - The main camera object
	 */
	public void TogglePanning(GameObject mainCamera) {
		if (mainCamera.GetComponent<CameraController>().enabled) {
			mainCamera.GetComponent<CameraController>().enabled = false;
		} else {
			mainCamera.GetComponent<CameraController>().enabled = true;
		}
	}
}
