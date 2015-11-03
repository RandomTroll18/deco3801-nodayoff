using UnityEngine;

/**
 * Any game objects that block tiles need this. By having this script as a component, they
 * block the tile their transform point is on and they optionally block more tiles.
 */
public class BlockedTiles : MonoBehaviour {
	public int Up;		/* Number of tiles that are blocked "upwards" */
	public int Down;	/* Number of tiles that are blocked "downwards" */
	public int Left;
	public int Right;
}
