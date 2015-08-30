using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public GameObject gameManagerObject; // The game manager object
	private GameManager gameManagerScript; // The game manager script

	enum Moving {
		NO,
		POSSIBLY, 
		YES
	}

	class Tile : IComparer<Tile> {
		public int x;
		public int z;

		public Tile(int x, int z) {
			this.x = x;
			this.z = z;
		}

		public Tile() {
			;
		}

		public override bool Equals(object obj) {
			if (!(obj is Tile)) {
				return false;
			}

			Tile t = obj as Tile;
			return (t.z == this.z) && (t.x == this.x);
		}

		/**
		 * Overriding hash code function
		 */
		public override int GetHashCode() {
			int prime = 13; // Prime number
			int hashCode = 0; // The hash code to return
			hashCode = (hashCode * prime) + this.x;
			hashCode = (hashCode * prime) + this.z;
			return hashCode;
		}

		public int Compare(Tile tile1, Tile tile2) {
			if (tile1 == null || tile2 == null) {
				throw new System.NullReferenceException();
			}
			
			if (tile1.z == tile2.z) {
				return tile1.x - tile2.x;
			} else {
				return tile2.z - tile2.z;
			}
		}

		public override string ToString() {
			return string.Format("X: {0}, Z: {1}", this.x, this.z);
		}
	}

	class OpenTile : Tile {
		/* Commented out for the sake of removing warnings. */ /* do this again, Josh, and I will hurt u */
		public int depth;
		public OpenTile parent;

		public OpenTile(Tile tile) : base (tile.x, tile.z) {
			parent = null;
			depth = 0;
		}

		public OpenTile(OpenTile parent, Tile tile) : base (tile.x, tile.z) {
			this.parent = parent;
			depth = parent.depth + 1;
		}

		public override string ToString() {
			return string.Format("depth: {0}, {1}", this.depth, base.ToString());
		}
	}

	class TileEqualityComparer : IEqualityComparer<Tile> {
		public bool Equals(Tile t1, Tile t2) {
			return (t1.x == t2.x) && (t1.z == t2.z);
		}

		public int GetHashCode(Tile t) {
			return (t.x.GetHashCode() + t.z).GetHashCode();
		}
	}
	
	public GameObject player;
	public float camSpeed;
	// The distance the mouse pointer needs to be from the edge before the screen moves.
	public float GUISize;
	public LayerMask layerMask;
	public float speed;
	public GameObject highlightedTile;
	public GameObject pathMarker;
	private Vector3 offset;
	private HashSet<Tile> blockedTiles = new HashSet<Tile>(new TileEqualityComparer());
	private Moving moving = Moving.NO;
	private LinkedList<Tile> path;
	private Tile clickedTile;
	private LinkedList<Object> movPath = new LinkedList<Object>();
	
	void Start() {
		offset = transform.position - player.transform.position;
		//this.gameManagerScript = this.gameManagerObject.GetComponent<GameManager>();

		// Initalises set of all blocked tiles
		GameObject[] blockers = GameObject.FindGameObjectsWithTag("Blocker");
		foreach (GameObject blocker in blockers) {
			// TODO: consider the declared size of each tile
			blockedTiles.Add(new Tile(
				TilePosition(blocker.transform.position.x), 
				TilePosition(blocker.transform.position.z)
			));
		}

		foreach (Tile tile in blockedTiles) {
			Debug.Log(tile.x + " " + tile.z);
//			GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			Vector3 v = TileMiddle (tile);
//			o.transform.position = v;
		}
	}

	void Update() {
		// Camera panning with mouse
		Rect recdown = new Rect(0, 0, Screen.width, GUISize);
		Rect recup = new Rect(0, Screen.height - GUISize, Screen.width, GUISize);
		Rect recleft = new Rect(0, 0, GUISize, Screen.height);
		Rect recright = new Rect(Screen.width - GUISize, 0, GUISize, Screen.height);
		
		if (recdown.Contains(Input.mousePosition)) {
			transform.Translate(0, 0, -camSpeed, Space.World);
		}
		
		if (recup.Contains(Input.mousePosition)) {
			transform.Translate(0, 0, camSpeed, Space.World);
		}
		
		if (recleft.Contains(Input.mousePosition)) {
			transform.Translate(-camSpeed, 0, 0, Space.World);
		}
		
		if (recright.Contains(Input.mousePosition)) {
			transform.Translate(camSpeed, 0, 0, Space.World);
		}


		// TODO: WASD
		// Should behave like a mouse click at a fixed offset


		// Mouse click detection and path finding
		if ((moving != Moving.YES) && (Input.GetMouseButtonUp(0))) {
			Tile goal = null;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
				goal = new Tile();
				goal.x = TilePosition(hit.point.x);
				goal.z = TilePosition(hit.point.z);
				Debug.Log(goal.x + " " + goal.z);

				if (moving == Moving.POSSIBLY && goal.Equals(clickedTile)) {
					moving = Moving.YES;
				} else if (moving == Moving.POSSIBLY) {
					Destroy(GameObject.FindGameObjectWithTag("Highlighted Tile"));
					foreach (Object obj in movPath) {
						DestroyObject(obj);
					}
					movPath.Clear();
				}
			}

			OpenTile dest = FindPath(goal);
			if (moving != Moving.YES && dest != null) {
				SpawnHighlitedTile(goal);
				clickedTile = goal;
				moving = Moving.POSSIBLY;
				path = FlipPath(dest);
				dest = dest.parent;
			}

			// Just for logging
			while (moving != Moving.YES && dest != null && dest.parent != null) {
				movPath.AddLast(SpawnPathTile(dest));
				dest = dest.parent;
			}
		}

		/* Movement. Note that right now this is shifting the transform but we could do it the physics way
		 * I just find the transform way easier.
		 */
		if (moving == Moving.YES) {
			// TODO: let the player cancel their move
			if (path.Count == 0) {
				moving = Moving.NO;
				Destroy(GameObject.FindGameObjectWithTag("Highlighted Tile"));
			} else if (player.transform.position == TileMiddle(path.First.Value)) {
				path.RemoveFirst();
				if (movPath.Count > 0) {
					DestroyObject(movPath.Last.Value);
					movPath.RemoveLast();
				}
			} else {
				float step = speed * Time.deltaTime;
				player.transform.position = Vector3.MoveTowards(
					player.transform.position, 
					TileMiddle(path.First.Value), 
					step
				);
				ResetCamera(); // This is a little dodgy. Locking the camera (i.e. disable camera panning code) will fix this.
			}
		}
	}

	private Object SpawnPathTile(Tile pos) {
		// I could add direction to the spawned path. But maybe another day
		Vector3 tilePos = TileMiddle(pos);
		Quaternion tileRot = Quaternion.Euler(90, 0, 0);
		return Instantiate(pathMarker, tilePos, tileRot);
	}

	private void SpawnHighlitedTile(Tile pos) {
		Vector3 tilePos = TileMiddle(pos);
		Quaternion tileRot = Quaternion.Euler(90, 0, 0);
		Instantiate(highlightedTile, tilePos, tileRot);
	}

	private LinkedList<Tile> FlipPath(OpenTile path) {
		LinkedList<Tile> res = new LinkedList<Tile>();
		while (path != null) {
			res.AddFirst(new Tile(path.x, path.z));
			path = path.parent;
		}
		res.RemoveFirst(); // Don't need the original position
		return res;
	}

	private Tile PlayerPosition() {
		Tile pos = new Tile();
		pos.x = TilePosition(player.transform.position.x);
		pos.z = TilePosition(player.transform.position.z);
		return pos;
	}

	/*
	 * Finds the shortest path from the player's position to the goal tile.
	 * Note that this returns the path as a link list and the front of the list
	 * is the end of the path.
	 */
	private OpenTile FindPath(Tile goal) {
		// Fixes:
		// Store an explored set. This should just be a hashset containing visited tiles
		// Use a different algorithm. BFS should be fine for our purposes
		if (goal != null && !blockedTiles.Contains(goal)) {
			Queue<OpenTile> q = new Queue<OpenTile>();
			q.Enqueue(new OpenTile(PlayerPosition()));
			while (q.Count != 0) {
				OpenTile current = q.Dequeue();
				if (current.Equals(goal)) {
					return current;
				}
				for (int z = 1; z >= -1; z -= 2) {
					Tile neighbour = new Tile(current.x + 0, current.z + z);
					if (!blockedTiles.Contains(neighbour)) {
						q.Enqueue(new OpenTile(current, neighbour));
					}
				}
				for (int x = 1; x >= -1; x -= 2) {
					Tile neighbour = new Tile(current.x + x, current.z + 0);
					if (!blockedTiles.Contains(neighbour)) {
						q.Enqueue(new OpenTile(current, neighbour));
					}
				}
			}
		}
		return null;
	}

	/*
	 * Converts the given pos to a tile relative to the tile at (0,0).
	 * For example, if you give 2.5, the tile is the 1st tile away from
	 * the centre tile.
	 */ 
	private int TilePosition(float pos) {
		return (int) Mathf.Ceil((pos - 1) / 2);
	}

	/*
	 * Returns the middle position of a tile.
	 */
	private Vector3 TileMiddle(Tile t) {
		Vector3 res = new Vector3(t.x * 2, 0, t.z * 2);
		return res;
	}

	/*
	 * Call this when the player moves.
	 */
	public void ResetCamera() {
		transform.position = player.transform.position + offset;
	}
}
