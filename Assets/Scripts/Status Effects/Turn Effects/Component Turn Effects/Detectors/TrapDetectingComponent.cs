using UnityEngine;
using System.Collections;

public class TrapDetectingComponent : MonoBehaviour {

	public int Range; // The range

	// Update is called once per frame
	void Update () {
		int distance; // The distance
		// Just iterate over each trap item
		foreach (GameObject trap in GameObject.FindGameObjectsWithTag("Trap")) {
			distance = MovementController.TileDistance(
					Player.MyPlayer.transform.position, 
			        trap.transform.position,
					Player.MyPlayer.GetComponent<MovementController>().GetBlockedTiles()
			);
			if (distance <= Range) { // Within range
				/*
				 * Possible implementation if the items have a Stealth component
				 * if (trap.GetComponent<Stealth>() != null && trap.GetComponent<Stealth>().enabled)
				 *     trap.GetComponent<Stealth>().enabled = false;
				 * 	   trap.GetComponent<MeshRenderer>().enabled = true;
				 */
				if (trap.GetComponentsInChildren<MeshRenderer>().Length > 0)
					toggleMeshRenderers(trap.GetComponentsInChildren<MeshRenderer>(), true);
			} else { // Not within range
				if (trap.GetComponentsInChildren<MeshRenderer>().Length > 0)
					toggleMeshRenderers(trap.GetComponentsInChildren<MeshRenderer>(), false);
			}
		}
	}

	/**
	 * Enable/disable the mesh renderers in the list
	 * 
	 * Arguments
	 * - MeshRenderer[] meshRenderers - The mesh renderers to enable
	 * - bool enableFlag - Flag for enabling/disabling mesh renderers
	 */
	void toggleMeshRenderers(MeshRenderer[] meshRenderers, bool enableFlag) {
		foreach (MeshRenderer meshRenderer in meshRenderers)
			meshRenderer.enabled = enableFlag;
	}

	/**
	 * Make the traps invisible
	 */
	public void MakeTrapsInvisible() {
		foreach (GameObject trap in GameObject.FindGameObjectsWithTag("Trap")) {
			/*
			 * Possible implementation if the items have a Stealth component
			 * if (trap.GetComponent<Stealth>() != null && trap.GetComponent<Stealth>().enabled)
			 *     trap.GetComponent<Stealth>().enabled = false;
			 * 	   trap.GetComponent<MeshRenderer>().enabled = true;
			 */
			if (trap.GetComponentsInChildren<MeshRenderer>().Length > 0)
				toggleMeshRenderers(trap.GetComponentsInChildren<MeshRenderer>(), false);
		}
	}
}
