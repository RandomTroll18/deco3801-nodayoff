using UnityEngine;
using System.Collections;

/*
 * Hides this object from the main player
 */
public class Stealth : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Player.MyPlayer == null) {
			return;
		}

		Player p = Player.MyPlayer.GetComponent<Player>();
		int distance = p.DistanceToTile(Tile.TilePosition(transform.position));
		if (distance > p.GetVisionDistance()) {
			GetComponent<MeshRenderer>().enabled = false;
		} else {
			GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
