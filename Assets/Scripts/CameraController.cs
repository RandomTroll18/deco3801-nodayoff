using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	class Tile : IComparer<Tile> {
		public int x;
		public int z;

		public Tile (int x, int z) {
			this.x = x;
			this.z = z;
		}

		public Tile () {
			;
		}

		public override bool Equals (object obj) {
			if (!(obj is Tile))
				return false;

			Tile t = obj as Tile;
			return (t.z == this.z) && (t.x == this.x);
		}

		public int Compare (Tile tile1, Tile tile2) {
			if (tile1 == null || tile2 == null)
				throw new System.NullReferenceException ();
			
			if (tile1.z == tile2.z)
				return tile1.x - tile2.x;
			else
				return tile2.z - tile2.z;
		}

		public override string ToString ()
		{
			return string.Format ("X: {0}, Z: {1}", this.x, this.z);
		}
	}

	class OpenTile : Tile {
		/* Commented out for the sake of removing warnings. */ /* do this again, Josh, and I will hurt u */
		public int depth;
		public OpenTile parent;

		public OpenTile (Tile tile) : base (tile.x, tile.z) {
			parent = null;
			depth = 0;
		}

		public OpenTile (OpenTile parent, Tile tile) : base (tile.x, tile.z) {
			this.parent = parent;
			depth = parent.depth + 1;
		}

		public override string ToString ()
		{
			return string.Format ("depth: {0}, {1}", this.depth, base.ToString());
		}
	}

	class TileEqualityComparer : IEqualityComparer<Tile> {
		public bool Equals(Tile t1, Tile t2) {
			return (t1.x == t2.x) && (t1.z == t2.z);
		}

		public int GetHashCode(Tile t) {
			return (t.x.GetHashCode () + t.z).GetHashCode ();
		}
	}
	
	public GameObject player;
	public float camSpeed;
	// The distance the mouse pointer needs to be from the edge before the screen moves.
	public float GUISize;
	public LayerMask layerMask;

	private Vector3 offset;
	private HashSet<Tile> blockedTiles = new HashSet<Tile> (new TileEqualityComparer ());
	
	void Start () {
		offset = transform.position - player.transform.position;

		// Initalise set of all blocked tiles
		GameObject[] blockers = GameObject.FindGameObjectsWithTag ("Blocker");
		foreach (GameObject blocker in blockers) {
			// TODO: consider the declared size of each tile
			blockedTiles.Add (new Tile (
				TilePosition (blocker.transform.position.x), 
				TilePosition (blocker.transform.position.z)
				));
		}

		foreach (Tile tile in blockedTiles) {
			Debug.Log (tile.x + " " + tile.z);
			GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Vector3 v = TileMiddle (tile);
			o.transform.position = v;
		}
	}

	void Update() {
		// Camera panning with mouse
		Rect recdown = new Rect (0, 0, Screen.width, GUISize);
		Rect recup = new Rect (0, Screen.height - GUISize, Screen.width, GUISize);
		Rect recleft = new Rect (0, 0, GUISize, Screen.height);
		Rect recright = new Rect (Screen.width - GUISize, 0, GUISize, Screen.height);
		
		if (recdown.Contains(Input.mousePosition))
			transform.Translate(0, 0, -camSpeed, Space.World);
		
		if (recup.Contains(Input.mousePosition))
			transform.Translate(0, 0, camSpeed, Space.World);
		
		if (recleft.Contains(Input.mousePosition))
			transform.Translate(-camSpeed, 0, 0, Space.World);
		
		if (recright.Contains(Input.mousePosition))
			transform.Translate(camSpeed, 0, 0, Space.World);


		// Mouse click detection
		if (Input.GetMouseButtonUp (0)) {
			Tile goal = null;

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
				goal = new Tile ();
				goal.x = TilePosition (hit.point.x);
				goal.z = TilePosition (hit.point.z);
				Debug.Log (goal.x + " " + goal.z);
			}

			if (goal != null && !blockedTiles.Contains (goal)) {
				// Finds the shortest path from current tile to goal tile
				Queue<OpenTile> q = new Queue<OpenTile>();
				q.Enqueue (new OpenTile (PlayerPosition()));
				while (q.Count != 0) {
					OpenTile current = q.Dequeue();

					if (current.Equals (goal)) {
						Debug.Log ("done");
						// TODO: replace since the shortest path has been found
						while (current != null) {
							Debug.Log (current.ToString ());
							current = current.parent;
						}
						break;
					}

					for (int z = 1; z >= -1; z -= 2) {
						Tile neighbour = new Tile (current.x + 0, current.z + z);
						if (!blockedTiles.Contains (neighbour)) {
							q.Enqueue (new OpenTile (current, neighbour));
						}
					}
					for (int x = 1; x >= -1; x -= 2) {
						Tile neighbour = new Tile (current.x + x, current.z + 0);
						if (!blockedTiles.Contains (neighbour)) {
							q.Enqueue (new OpenTile (current, neighbour));
						}
					}
				}
			}
		}
	}

	private Tile PlayerPosition() {
		Tile pos = new Tile ();
		pos.x = TilePosition (player.transform.position.x);
		pos.z = TilePosition (player.transform.position.z);
		return pos;
	}

	/*
	 * Converts the given pos to a tile relative to the tile at (0,0).
	 * For example, if you give 2.5, the tile is the 1st tile away from
	 * the centre tile.
	 */ 
	private int TilePosition (float pos) {
		return (int) Mathf.Ceil ((pos - 1) / 2);
	}

	/*
	 * Returns the middle position of a tile.
	 */
	private Vector3 TileMiddle (Tile t) {
		Vector3 res = new Vector3 (t.x * 2, 0, t.z * 2);
		return res;
	}

	/*
	 * Call this when the player moves.
	 */
	public void ResetCamera () {
		transform.position = player.transform.position + offset;
	}
}
