using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvisibilityDetectingComponent : MonoBehaviour {

	public int Range; // The range
	
	// Update is called once per frame
	void Update () {
		List<GameObject> gameObjectsFound = new List<GameObject>(); // List of game objects
		/* Check each player, trap and item */
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.GetComponent<Stealth>() == null)
				continue; // Player has no stealth. Skip
			gameObjectsFound.Add(player);
		}
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Item")) {
			if (item.GetComponent<Stealth>() == null)
				continue; // Item has no stealth
			gameObjectsFound.Add(item);
		}
		foreach (GameObject trap in GameObject.FindGameObjectsWithTag("Trap")) {
			if (trap.GetComponent<Stealth>() == null)
				continue; // Trap has no stealth
			gameObjectsFound.Add(trap);
		}
		toggleVisibility(gameObjectsFound, true);
	}

	/**
	 * Start making objects invisible/visible depending on whether their stealth is permanent or not
	 * 
	 * Arguments
	 * - List<GameObject> gameObjects - objects to set visible
	 * - bool visibleFlag - set to true if items need to be made visible, false otherwise
	 */
	void toggleVisibility(List<GameObject> gameObjects, bool visibleFlag) {
		foreach (GameObject hiddenObject in gameObjects) {
			if (visibleFlag) { // Need to make objects visible
				if (hiddenObject.GetComponent<Stealth>().Permanent)
					makeVisibleWithinRange(hiddenObject);
				else 
					makeVisible(hiddenObject);
			} else // Need to make objects invisible
				hiddenObject.GetComponent<Stealth>().enabled = true;
		}
	}

	/**
	 * Make object visible regardless of range
	 * 
	 * Arguments
	 * - GameObject hiddenObject - The hidden object
	 */
	void makeVisible(GameObject hiddenObject) {
		hiddenObject.GetComponent<Stealth>().enabled = false;
		hiddenObject.GetComponentInChildren<MeshRenderer>().enabled = true;
	}

	/**
	 * Make object visible within range
	 * 
	 * Arguments
	 * - GameObject hiddenObject - The object to make visible
	 */
	void makeVisibleWithinRange(GameObject hiddenObject) {
		int distance; // The distance 

		if (hiddenObject.Equals(gameObject))
			return; // Skip over ourselves
		distance = MovementController.TileDistance(
			Player.MyPlayer.transform.position, 
			hiddenObject.transform.position,
			Player.MyPlayer.GetComponent<MovementController>().GetBlockedTiles()
			);
		if (distance <= Range) { // Within range
			if (hiddenObject.GetComponent<Stealth>().enabled) {
				hiddenObject.GetComponent<Stealth>().enabled = false;
				hiddenObject.GetComponentInChildren<MeshRenderer>().enabled = true;
			}
		} else { // Not within range
			if (!hiddenObject.GetComponent<Stealth>().enabled)
				hiddenObject.GetComponent<Stealth>().enabled = true;
		}
	}

	/**
	 * Make items invisible. Used for when this component is to be destroyed
	 */
	public void MakeInvisible() {
		List<GameObject> gameObjectsFound = new List<GameObject>(); // List of game objects
		/* Check each player, trap and item */
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.GetComponent<Stealth>() == null)
				continue; // Player has no stealth. Skip
			gameObjectsFound.Add(player);
		}
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Item")) {
			if (item.GetComponent<Stealth>() == null)
				continue; // Item has no stealth
			gameObjectsFound.Add(item);
		}
		foreach (GameObject trap in GameObject.FindGameObjectsWithTag("Trap")) {
			if (trap.GetComponent<Stealth>() == null)
				continue; // Trap has no stealth
			gameObjectsFound.Add(trap);
		}
		toggleVisibility(gameObjectsFound, false);
	}
}
