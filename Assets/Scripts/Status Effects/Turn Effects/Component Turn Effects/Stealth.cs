using UnityEngine;
using System.Collections;

/*
 * Hides this object from the main player
 */
using System.Collections.Generic;


public class Stealth : MonoBehaviour {

	public bool Permanent = false; // Record if the stealth applied is permanent or vision binding only

	// Update is called once per frame
	void Update() {
		if (!Permanent) // Not permanent. Just hide if not within range
			hideFromOthers();
		else // Permanent. Make it permanently invisible
			permanentInvisibility();
	}

	/**
	 * Permanent invisibility
	 */
	void permanentInvisibility() {
		List<Renderer> renderers = new List<Renderer>(); // The renderers
		if (gameObject.tag.Equals("Trap")) { // Don't hide if trap belongs to this player
			if (gameObject.GetComponent<ScoutTrapScript>() != null
			    && gameObject.GetComponent<ScoutTrapScript>().GetOwner() == Player.MyPlayer)
				return; // Don't hide from owner
		}
		renderers.AddRange(GetComponents<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
		toggleRenderers(renderers, false);
	}

	/**
	 * Hide the player from others
	 */
	void hidePlayerFromOthers() {
		/* The renderers */
		Renderer rendererInMain = ClassPanelScript.CurrentPlayer.GetComponent<Renderer>();
		Renderer rendererinChildren = ClassPanelScript.CurrentPlayer.GetComponentInChildren<Renderer>();

		if (rendererInMain != null)
			rendererInMain.enabled = false;
		if (rendererinChildren != null)
			rendererinChildren.enabled = false;
	}

	/**
	 * Hide this object from the player
	 */
	void hideFromOthers() {
		Player p; // The player script
		int distance; // The distance to the player
		List<Renderer> renderers; // List of renderers

		if (Player.MyPlayer == null) // No player
			return;
		else if (gameObject.tag.Equals("Trap")) { // Don't hide if trap belongs to this player
			if (gameObject.GetComponent<ScoutTrapScript>() != null
					&& gameObject.GetComponent<ScoutTrapScript>().GetOwner() == Player.MyPlayer)
				return; // Don't hide from owner
		}
		p  = ClassPanelScript.CurrentPlayer.GetComponent<Player>();
		distance = p.DistanceToTile(Tile.TilePosition(transform.position), true);
		renderers = new List<Renderer>();
		renderers.AddRange(GetComponents<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
		Debug.Log("Distance to player: " + distance);
		if (distance > p.GetVisionDistance()) // Hidden from player, but are we hidden from surveillance cams
			hideFromSurvCams();
		else // Not hidden
			toggleRenderers(renderers, true);
	}

	/**
	 * Hide from surveillance cameras
	 */
	void hideFromSurvCams() {
		List<Renderer> renderers = new List<Renderer>(); // List of renderers
		GameObject[] survCams = GameObject.FindGameObjectsWithTag("SurveillanceCameras");

		/* Get renderers */
		renderers.AddRange(GetComponents<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<MeshRenderer>());

		if (survCams.Length > 0) { // There are surv cams. Need to check if they are active
			foreach (GameObject survCam in survCams) {
				if (survCam.activeSelf) { // Active surveillance camera. You are visible
					toggleRenderers(renderers, true);
					break;
				} else // Inactive camera. Set to be inactive
					toggleRenderers(renderers, false);
			}
		} else // There are no surv cameras
			toggleRenderers(renderers, false);

	}

	/**
	 * Enable/disable list of mesh renderers
	 * 
	 * Arguments
	 * - MeshRenderer[] renderers - The list of mesh renderers
	 * - bool enableFlag - Flag for enabling/disabling renderers
	 */
	void toggleRenderers(List<Renderer> renderers, bool enableFlag) {
		foreach (Renderer meshRenderer in renderers)
			meshRenderer.enabled = enableFlag;
	}
}
