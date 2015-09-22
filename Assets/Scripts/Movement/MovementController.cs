using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/* Known bugs:
 * The visual path is not reset when the end turn button is pressed.
 */ 

/**
 * Controls the player's movement within the game.
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
			return string.Format("depth: {0}, {1}", Depth, base.ToString());
		}
	}

	public float Speed;
	public GameObject InvalidPathMarker;
	public GameObject ValidPathMarker;
	public GameObject HighlightedTile;
	public GameObject Player;
	HashSet<Tile> blockedTiles = new HashSet<Tile>(new Tile());
	List<InteractiveObject> InteractiveTiles = new List<InteractiveObject>();
	/* Player's "moving" status */
	Moving moving = Moving.NO;
	/* The path of tiles the player is moving with */
	LinkedList<Tile> path;
	/* The list of objects used to visualise the movement path */
	LinkedList<Object> visualPath = new LinkedList<Object>();
	/* Tile player has selected for movement */
	Tile clickedTile;
	CameraController camController;
	GameManager gameManager;
	Player playerScript;
	/* Whether debugging output is used */
	bool debugging;
	
	void Awake() {
		debugging = false;
		camController = Camera.main.GetComponent<CameraController>();
		playerScript = Player.GetComponent<Player>();
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

		// TODO: WASD well it would have to be directional arrows now
		// Should behave like a mouse click at a fixed offset


		// Initalises set of all blocked tiles
		/*
		 * IMPORTANT: if you make a tag type that you want to add to blockedTiles, use AddRange()
		 * like I have here.
		 */ 
		GameObject[] blockers = GameObject.FindGameObjectsWithTag("Blocker");
		GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
		List<GameObject> allBlockedTiles = new List<GameObject>();
		allBlockedTiles.AddRange(blockers);
		allBlockedTiles.AddRange(doors);

		allBlockedTiles.ForEach(delegate(GameObject blocker) {
			BlockedTiles bt = blocker.GetComponent<BlockedTiles>();

			/* Not all blockers will have a BlockedTiles script (e.g. doors) */
			if (bt == null) {
				bt = new BlockedTiles();
			} 

			/* I like duplicate code :} */
			for (int i = -bt.Down; i <= bt.Up; i++) {
				Tile t = new Tile(
					Tile.TilePosition(blocker.transform.position.x), 
					Tile.TilePosition(blocker.transform.position.z) + i
				);
				blockedTiles.Add(t);
			}
			for (int i = -bt.Left; i <= bt.Right; i++) {
				Tile t = new Tile(
					Tile.TilePosition(blocker.transform.position.x) + i, 
					Tile.TilePosition(blocker.transform.position.z)
				);
				blockedTiles.Add(t);
			}
		});

		/* Checks if blockedTiles is correct */
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
			camController.LockCamera();
			// TODO: let the player cancel their move
			if (path.Count == 0) {
				StopMoving();
			} else if (Player.transform.position == Tile.TileMiddle(path.First.Value)) {
				path.RemoveFirst();
				if (visualPath.Count > 0) { // Most of the time this condition is true
					DestroyObject(visualPath.Last.Value);
					visualPath.RemoveLast();
				}

				playerScript.ReduceStatValue(Stat.AP, 1);
				if (playerScript.GetStatValue(Stat.AP) == 0) {
					ClearPath();
					StopMoving();
				}
			} else {
				/* Moves the player */
				float step = Speed * Time.deltaTime;
				Player.transform.position = Vector3.MoveTowards(
					Player.transform.position, 
					Tile.TileMiddle(path.First.Value), 
					step
				);
				camController.ResetCamera();
			}
		}
	}

	void StopMoving() {
		moving = Moving.NO;
		camController.UnlockCamera();
		Destroy(GameObject.FindGameObjectWithTag("Highlighted Tile"));
	}

	/**
	 * This does one of two things: visualises the player's movement choice or it confirms
	 * and moves a player's movement choice. Which one is chosen depends on whether this is the
	 * first time the player has clicked the goal or the second.
	 */
	public void RequestMovement(Tile goal) {
		if (InteractiveTiles.Count >= 1) {
			InteractiveTiles[0].CloseEvent();
		}

		if (moving == Moving.YES) {
			return;
		}

		if (moving == Moving.POSSIBLY && goal.Equals(clickedTile)) {
			if (playerScript.GetStatValue(Stat.AP) != 0)
				moving = Moving.YES;
			return;
		}

		if (moving == Moving.POSSIBLY) {
			/* Clears all visual elements of selected path */
			Destroy(GameObject.FindGameObjectWithTag("Highlighted Tile"));
			ClearPath();
		}

		if (UseInteractable (goal, playerScript)) {
			return;
		}
		
		PathTile dest = FindPath(goal);

		if (dest != null) {
			SpawnHighlightedTile(goal);
			clickedTile = goal;
			moving = Moving.POSSIBLY;
			path = FlipPath(dest);
			dest = dest.Parent;
		}
		
		while (dest != null && dest.Parent != null) {
			if (dest.Depth > playerScript.GetStatValue(Stat.AP)) {
				visualPath.AddLast(SpawnPathTile(dest, InvalidPathMarker));
			} else {
				visualPath.AddLast(SpawnPathTile(dest, ValidPathMarker));
			}
			dest = dest.Parent;
		}
	}

	void ClearPath() {
		foreach (Object obj in visualPath) {
			DestroyObject(obj);
		}
		visualPath.Clear();
	}

	Object SpawnPathTile(Tile pos, GameObject pathMarker) {
		// I could add direction to the spawned path. But maybe another day
		Vector3 tilePos = Tile.TileMiddle(pos);
		tilePos.y = tilePos.y - 0.49f;
		Quaternion tileRot = Quaternion.Euler(90, 0, 0);
		return Instantiate(pathMarker, tilePos, tileRot);
	}

	void SpawnHighlightedTile(Tile pos) {
		Vector3 tilePos = Tile.TileMiddle(pos);
		tilePos.y = tilePos.y - 0.49f;
		Quaternion tileRot = Quaternion.Euler(90, 0, 0);
		Instantiate(HighlightedTile, tilePos, tileRot);
	}

	/*
	 * Finds the shortest path from the player's position to the goal tile.
	 * Note that this returns the path as a link list and the front of the list
	 * is the end of the path.
	 */
	PathTile FindPath(Tile goal) {
		HashSet<Tile> explored = new HashSet<Tile>();
		if (goal != null && !blockedTiles.Contains(goal)) {
			Queue<PathTile> q = new Queue<PathTile>();
			q.Enqueue(new PathTile(playerScript.PlayerPosition()));
			while (q.Count != 0) {
				PathTile current = q.Dequeue();
				if (current.Equals(goal)) {
					return current;
				}
				/* I like duplicate code :} */
				for (int z = 1; z >= -1; z -= 2) {
					Tile neighbour = new Tile(current.X + 0, current.Z + z);
					if (!blockedTiles.Contains(neighbour) && !explored.Contains(neighbour)) {
						explored.Add(neighbour);
						q.Enqueue(new PathTile(current, neighbour));
					}
				}
				for (int x = 1; x >= -1; x -= 2) {
					Tile neighbour = new Tile(current.X + x, current.Z + 0);
					if (!blockedTiles.Contains(neighbour) && !explored.Contains(neighbour)) {
						explored.Add(neighbour);
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

	/**
	 * Sets the given tile to no longer be blocking. This means the movement path could now go 
	 * through that tile. NOTE: your blocker will still appear on
	 * the map unless you delete it yourself. 
	 */
	public void UnblockTile(Tile tile) {
		if (!blockedTiles.Contains(tile)) {
			Debug.LogWarning("You tried to unblock a tile that wasn't blocked. Did you want to do" +
				"this? FROM BEN");
		} else {
			if (debugging) {
				GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Vector3 v = Tile.TileMiddle(tile);
				o.transform.position = v;
			}
			blockedTiles.Remove(tile);
		}
	}

	public void BlockTile(Tile tile) {
		if (blockedTiles.Contains(tile)) {
			Debug.LogWarning("You tried to block a tile that was blocked. Did you want to do" +
				"this? FROM BEN");
		} else {
			blockedTiles.Add(tile);
		}
	}

	/*
	 * Get Index of Interactable on a certain Tile.
	 */
	public int GetInteractable(Tile tile) {
		for (int i = 0; i < InteractiveTiles.Count; i++) {
			if (tile.Equals(InteractiveTiles[i].GetTile())) {
				return i;
			}
		}
		return -1;
	}

	/*
	 * Checks if player is Adjacent to tile. 
	 */
	public bool IsNear(Tile tile, Player player) {
		if (player.PlayerPosition().Equals(new Tile(tile.X + 1, tile.Z)) || 
			player.PlayerPosition().Equals(new Tile(tile.X - 1, tile.Z)) ||
			player.PlayerPosition().Equals(new Tile(tile.X, tile.Z + 1)) ||
			player.PlayerPosition().Equals(new Tile(tile.X, tile.Z - 1))) {
			return true;
		}
		return false;
	}

	/*
	 * Returns whether the given tile blocks movement.
	 * 
	 * These tiles include walls, doors, interactables + whatever else you want.
	 */ 
	public bool IsTileBlocked(Tile tile) {
		return blockedTiles.Contains(tile);
	}

	public void AddInteractable(InteractiveObject ToAdd) {
		//c = i.GetComponent<InteractiveObject>();
		InteractiveTiles.Add(ToAdd);
		blockedTiles.Add(ToAdd.GetTile());
		Debug.Log("int added: " + ToAdd.GetTile().ToString());
		// I can't see a reason why you need to call getTile() when this function exists:
		//blockedTiles.Add(Tile.TilePosition(i.transform.position));
	}
	
	public void RemoveInteractable(InteractiveObject ToRemove) {
		//c = i.GetComponent<InteractiveObject>();
		InteractiveTiles.Remove(ToRemove);
		blockedTiles.Remove(ToRemove.GetTile());
		Debug.Log("int Remove: " + ToRemove.GetTile().ToString());
		// I can't see a reason why you need to call getTile() when this function exists:
		//blockedTiles.Add(Tile.TilePosition(i.transform.position));
	}

	public void RemoveInteractable(Tile ToRemove) {
		int index = GetInteractable (ToRemove);
		if (index == -1) {
			Debug.Log("Can't find Tile");
			return;
		}
		blockedTiles.Remove(ToRemove);
		InteractiveTiles.Remove(InteractiveTiles[index]);
		Debug.Log("int Remove: " + ToRemove.ToString());
	}

	public bool UseInteractable(Tile goal, Player playerSCript) {
		int index;
		if (((index = this.GetInteractable(goal)) != -1) && IsNear(goal, playerScript)) {
			//Debug.Log ("Int clicked. Count of int is " + InteractiveTiles.Count());
			Debug.Log(index);
			InteractiveTiles[index].Interact();
			//Debug.Log(InteractiveTiles[index]);
			return true;
		}
		return false;
	}

	/**
	 * Reassign the player script (once the Player GameObject changes)
	 */
	public void ChangePlayerScript() { 
		playerScript = Player.GetComponent<Player>();
	}
}