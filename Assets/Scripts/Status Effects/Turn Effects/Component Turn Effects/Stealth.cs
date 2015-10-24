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
			hideFromPlayer();
		else // Permanent. Make it permanently invisible
			permanentInvisibility();
	}

	/**
	 * Permanent invisibility
	 */
	void permanentInvisibility() {
		List<Renderer> renderers = new List<Renderer>(); // The renderers
		renderers.AddRange(GetComponents<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
		toggleRenderers(renderers, false);
	}

	/**
	 * Hide the player from others
	 */
	void hidePlayerFromOthers() {
		Player.MyPlayer.GetComponentInChildren<Renderer>().enabled = false;
	}

	/**
	 * Hide this object from the player
	 */
	void hideFromPlayer() {
		Player p; // The player script
		int distance; // The distance to the player
		if (Player.MyPlayer == null) // No player
			return;
		else if (gameObject.tag.Equals("Trap")) { // Don't hide if trap belongs to this player
			if (gameObject.GetComponent<ScoutTrapScript>() != null
					&& gameObject.GetComponent<ScoutTrapScript>().GetOwner() == Player.MyPlayer)
				return; // Don't hide from owner
		}
		
		p  = Player.MyPlayer.GetComponent<Player>();
		distance = p.DistanceToTile(Tile.TilePosition(transform.position));
		List<Renderer> renderers = new List<Renderer>();
		renderers.AddRange(GetComponents<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
		if (distance > p.GetVisionDistance())
			toggleRenderers(renderers, false);
		else
			toggleRenderers(renderers, true);
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
