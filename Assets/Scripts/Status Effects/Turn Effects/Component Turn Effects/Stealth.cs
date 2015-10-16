using UnityEngine;
using System.Collections;

/*
 * Hides this object from the main player
 */
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
		if (Player.MyPlayer == null)
			return;
		
		Player p = Player.MyPlayer.GetComponent<Player>();
		int distance = p.DistanceToTile(Tile.TilePosition(transform.position));
		if (distance > p.GetVisionDistance())
			GetComponent<MeshRenderer>().enabled = false;
		else
			GetComponent<MeshRenderer>().enabled = true;
	}
}
