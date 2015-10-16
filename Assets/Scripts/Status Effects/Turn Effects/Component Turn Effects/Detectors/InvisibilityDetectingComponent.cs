using UnityEngine;
using System.Collections;

public class InvisibilityDetectingComponent : MonoBehaviour {

	public int Range; // The range
	
	// Update is called once per frame
	void Update () {
		int distance; // The distance
		// Just iterate over each trap item
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.Equals(gameObject))
				return; // Skip over ourselves
			distance = MovementController.TileDistance(
				Player.MyPlayer.transform.position, 
				player.transform.position,
				Player.MyPlayer.GetComponent<MovementController>().GetBlockedTiles()
				);
			if (distance <= Range) { // Within range
				if (player.GetComponent<Stealth>() != null && player.GetComponent<Stealth>().enabled) {
					player.GetComponent<Stealth>().enabled = false;
					player.GetComponentInChildren<MeshRenderer>().enabled = true;
				}
			} else { // Not within range
				if (player.GetComponent<Stealth>() != null && !player.GetComponent<Stealth>().enabled)
					player.GetComponent<Stealth>().enabled = true;
			}
		}
	}

	/**
	 * Make players invisible. Used for when this component is to be destroyed
	 */
	public void MakePlayersInvisible() {
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.GetComponent<Stealth>() != null && !player.GetComponent<Stealth>().enabled)
				player.GetComponent<Stealth>().enabled = true;
		}
	}
}
