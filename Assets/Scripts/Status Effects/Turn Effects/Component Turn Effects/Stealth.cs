using UnityEngine;
using System.Collections;

/*
 * Hides this object from the main player
 */
using System.Collections.Generic;


public class Stealth : MonoBehaviour {

	public bool AttachedToPlayer = false; // Record if this component is attached to the player

	// Update is called once per frame
	void Update() {
		if (!AttachedToPlayer) // Attached to non-player item. Hide from player
			hideFromPlayer();
		else // Attached to a player. Make them hidden
			hidePlayerFromOthers();

	}

	/**
	 * Hide the player from others
	 */
	void hidePlayerFromOthers() {
		Player.MyPlayer.GetComponentInChildren<MeshRenderer>().enabled = false;
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
		List<MeshRenderer> renderers = new List<MeshRenderer>();
		renderers.AddRange(GetComponents<MeshRenderer>());
		renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
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
	void toggleRenderers(List<MeshRenderer> renderers, bool enableFlag) {
		foreach (MeshRenderer meshRenderer in renderers)
			meshRenderer.enabled = enableFlag;
	}
}
