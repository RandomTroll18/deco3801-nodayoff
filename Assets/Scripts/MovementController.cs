using System.Collections.Generic;
using UnityEngine;

/**
 * Controls the player's movement within the game.
 * Assumes: there is only one player with the script "Player".
 */
public class MovementController : MonoBehaviour {
	enum Moving {
		NO,			/* Player has not clicked a tile to move to */
		POSSIBLY, 	/* Player has clicked a tile to move to once */
		YES			/* Player has clicked a tile to move to twice and is moving there */
	}

	/**
	 * A tile on a movement path.
	 */
	class PathTile : Tile {
		public int Depth;
		public PathTile Parent;
		
		public PathTile(Tile tile) : base (tile.X, tile.Z) {
			Parent = null;
			Depth = 0;
		}
		
		public PathTile(PathTile parent, Tile tile) : base (tile.X, tile.Z) {
			this.Parent = parent;
			Depth = parent.Depth + 1;
		}
		
		public override string ToString() {
			return string.Format("depth: {0}, {1}", this.Depth, base.ToString());
		}
	}

	public float Speed;
	public GameObject PathMarker;
	public GameObject HighlightedTile;
	public GameObject Player;
	HashSet<Tile> blockedTiles = new HashSet<Tile>(new Tile());
	Moving moving = Moving.NO;									/* Player's "moving" status */
	LinkedList<Tile> path;
	LinkedList<Object> movPath = new LinkedList<Object>();
	CameraController camController;
	Player playerScript;
	Tile clickedTile;
	bool debugging = false;		/* Whether debugging output is used */
	
	void Start() {
		camController = Camera.main.GetComponent<CameraController>();
		playerScript = Player.GetComponent<Player>();

		// TODO: WASD
		// Should behave like a mouse click at a fixed offset


		// Initalises set of all blocked tiles
		GameObject[] blockers = GameObject.FindGameObjectsWithTag("Blocker");
		foreach (GameObject blocker in blockers) {
			// TODO: consider the declared size of each tile
			blockedTiles.Add(new Tile(
				Tile.TilePosition(blocker.transform.position.x), 
				Tile.TilePosition(blocker.transform.position.z)
			));
		}

		if (debugging) {
			foreach (Tile tile in blockedTiles) {
				Debug.Log(tile.X + " " + tile.Z);
				GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Vector3 v = Tile.TileMiddle(tile);
				o.transform.position = v;
			}
		}
	}

	void Update() {
		/* 
		 * Note that right now this is shifting the transform but we could do it the physics way I 
		 * just find the transform way easier.
		 */
		if (moving == Moving.YES) {
			// TODO: let the player cancel their move
			if (path.Count == 0) {
				moving = Moving.NO;
				Destroy(GameObject.FindGameObjectWithTag("Highlighted Tile"));
			} else if (Player.transform.position == Tile.TileMiddle(path.First.Value)) {
				path.RemoveFirst();
				if (movPath.Count > 0) {
					DestroyObject(movPath.Last.Value);
					movPath.RemoveLast();
				}
			} else {
				float step = Speed * Time.deltaTime;
				Player.transform.position = Vector3.MoveTowards(
					Player.transform.position, 
					Tile.TileMiddle(path.First.Value), 
					step
				);
				camController.ResetCamera(); // This is a little dodgy. Locking the camera (i.e. disable camera panning code) will fix this.
			}
		}
	}

	/**
	 * This done one of two things: visualises the player's movement choice or it confirms
	 * and moves a player's movement choice. Which one is chosen depends on whether this is the
	 * first time the player has clicked the goal or the second.
	 */
	public void RequestMovement(Tile goal) {
		if (moving == Moving.POSSIBLY && goal.Equals(clickedTile)) {
			moving = Moving.YES;
		} else if (moving == Moving.POSSIBLY) {
			Destroy(GameObject.FindGameObjectWithTag("Highlighted Tile"));
			foreach (Object obj in movPath) {
				DestroyObject(obj);
			}
			movPath.Clear();
		}

		PathTile dest = FindPath(goal);
		if (moving != Moving.YES && dest != null) {
			SpawnHighlitedTile(goal);
			clickedTile = goal;
			moving = Moving.POSSIBLY;
			path = FlipPath(dest);
			dest = dest.Parent;
		}
		
		while (moving != Moving.YES && dest != null && dest.Parent != null) {
			movPath.AddLast(SpawnPathTile(dest));
			dest = dest.Parent;
		}
	}

	Object SpawnPathTile(Tile pos) {
		// I could add direction to the spawned path. But maybe another day
		Vector3 tilePos = Tile.TileMiddle(pos);
		Quaternion tileRot = Quaternion.Euler(90, 0, 0);
		return Instantiate(PathMarker, tilePos, tileRot);
	}

	void SpawnHighlitedTile(Tile pos) {
		Vector3 tilePos = Tile.TileMiddle(pos);
		Quaternion tileRot = Quaternion.Euler(90, 0, 0);
		Instantiate(HighlightedTile, tilePos, tileRot);
	}

	/*
	 * Finds the shortest path from the player's position to the goal tile.
	 * Note that this returns the path as a link list and the front of the list
	 * is the end of the path.
	 */
	PathTile FindPath(Tile goal) {
		// Fixes:
		// Store an explored set. This should just be a hashset containing visited tiles
		// Use a different algorithm. BFS should be fine for our purposes
		if (goal != null && !blockedTiles.Contains(goal)) {
			Queue<PathTile> q = new Queue<PathTile>();
			q.Enqueue(new PathTile(playerScript.PlayerPosition()));
			while (q.Count != 0) {
				PathTile current = q.Dequeue();
				if (current.Equals(goal)) {
					return current;
				}
				for (int z = 1; z >= -1; z -= 2) {
					Tile neighbour = new Tile(current.X + 0, current.Z + z);
					if (!blockedTiles.Contains(neighbour)) {
						q.Enqueue(new PathTile(current, neighbour));
					}
				}
				for (int x = 1; x >= -1; x -= 2) {
					Tile neighbour = new Tile(current.X + x, current.Z + 0);
					if (!blockedTiles.Contains(neighbour)) {
						q.Enqueue(new PathTile(current, neighbour));
					}
				}
			}
		}
		return null;
	}

	/**
	 * Returns a backwards PathTile path as a LinkedList.
	 */
	LinkedList<Tile> FlipPath(PathTile path) {
		LinkedList<Tile> res = new LinkedList<Tile>();
		while (path != null) {
			res.AddFirst(new Tile(path.X, path.Z));
			path = path.Parent;
		}
		res.RemoveFirst(); // Don't need the original path position
		return res;
	}
}
