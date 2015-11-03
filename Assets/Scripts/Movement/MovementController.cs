using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public GameObject APCounter;
	public GameObject Counter;


	HashSet<Tile> blockedTiles = new HashSet<Tile>(new Tile());
	List<InteractiveObject> InteractiveTiles = new List<InteractiveObject>();
	/* Player's "moving" status */
	Moving moving = Moving.NO;
	/* The path of tiles the player is moving across */
	LinkedList<Tile> path;
	/* The list of objects used to visualise the movement path */
	LinkedList<GameObject> visualPath = new LinkedList<GameObject>();
	/* Tile player has selected for movement */
	Tile clickedTile;
	CameraController camController;
	Player playerScript;
	/* Whether debugging output is used */
	bool debugging;
	const float HIGHLIGHTED_TILE_ELEVATION = 0.45f;
	int cost;
	
	public void StartMe() {
		BlockedTiles bt; // Blocked tiles
		GameObject[] blockers; // Tile blockers
		GameObject[] doors; // List of doors
		List<GameObject> allBlockedTiles; // List of all blocked tiles
		Tile t; // Generated tile

		SetPublicVariables();
		debugging = false;
		camController = GetComponentInChildren<CameraController>();
		playerScript = gameObject.GetComponent<Player>();

		/*
		 * IMPORTANT: if you make a tag type that you want to add to blockedTiles, use AddRange()
		 * like I have here.
		 */ 
		blockers = GameObject.FindGameObjectsWithTag("Blocker");
		doors = GameObject.FindGameObjectsWithTag("Door");
		allBlockedTiles = new List<GameObject>();
		allBlockedTiles.AddRange(blockers);
		allBlockedTiles.AddRange(doors);

		allBlockedTiles.ForEach(delegate(GameObject blocker) {
			bt = blocker.GetComponent<BlockedTiles>();

			/* Not all blockers will have a BlockedTiles script (e.g. doors) */
			if (bt == null)
				bt = new BlockedTiles();

			for (int i = -bt.Down; i <= bt.Up; i++) {
				t = new Tile(
					Tile.TilePosition(blocker.transform.position.x), 
					Tile.TilePosition(blocker.transform.position.z) + i
				);
				blockedTiles.Add(t);
			}
			for (int i = -bt.Left; i <= bt.Right; i++) {
				t = new Tile(
					Tile.TilePosition(blocker.transform.position.x) + i, 
					Tile.TilePosition(blocker.transform.position.z)
				);
				blockedTiles.Add(t);
			}
		});

		GameObject o;
		Vector3 v ;
		/* Checks if blockedTiles is correct */
		if (debugging) {
			foreach (Tile tile in blockedTiles) {
				Debug.Log(tile.X + " " + tile.Z);
				o = GameObject.CreatePrimitive(PrimitiveType.Cube);
				v = Tile.TileMiddle(tile);
				o.transform.position = v;
			}
		}
	
	}

	/**
	 * Setting public variables in this script
	 */
	void SetPublicVariables() {
		Speed = 10f;
		InvalidPathMarker = Resources.Load("Invalid Path Marker") as GameObject;
		ValidPathMarker = Resources.Load("Valid Path Marker") as GameObject;
		HighlightedTile = Resources.Load("Highlighted Tile") as GameObject;
		APCounter = Resources.Load("UI/AP Cost") as GameObject;
	}

	/**
	 * Set moving animation
	 * 
	 * Arguments
	 * - bool enableFlag - flag used to determine whether we are moving or not moving
	 */
	void setMovingAnimation(bool enableFlag) {
		List<Animator> animators = new List<Animator>(); // The animators

		animators.AddRange(GetComponents<Animator>());
		animators.AddRange(GetComponentsInChildren<Animator>());

		if (animators.Count > 0) {
			foreach (Animator animator in animators) {
				animator.enabled = false;
				animator.SetBool("moving", enableFlag);
				animator.enabled = true;
			}
		}
	}

	/**
	 * Check for nulls when doing movement routine
	 * 
	 * Returns
	 * - true if a null is detected. False otherwise
	 */
	bool checkForNulls() {
		if (visualPath == null) {
			Debug.Log("Visual path is null");
			return true;
		} else if (transform == null) {
			Debug.Log("Transform is null, strangely");
			return true;
		} else if (visualPath.Last == null) {
			Debug.Log("Visual path last is null");
			return true;
		} else if (visualPath.Last.Value == null) {
			Debug.Log("Visual Path Last Value is null");
			return true;
		} else if (visualPath.Last.Value.transform == null) {
			Debug.Log("Visual Path Last Value transform is null");
			return true;
		}
		return false;
	}

	void Update() {
		float step; // A step for moving the player
		Vector3 targetPostition; // The position to move to

		if (moving == Moving.YES)
			setMovingAnimation(true);
		else
			setMovingAnimation(false);

		if (moving == Moving.YES) {
			if (Counter != null) {
				Destroy(Counter);
				Counter = null;
			}

			camController.LockCamera();
			if (path.Count == 0) {
				StopMoving();
			} else if (gameObject.transform.position == Tile.TileMiddle(path.First.Value)) {
				/*
				 * The next tile in the movement path has just been reached.
				 */

				path.RemoveFirst();
				if (visualPath.Count > 0) { // Most of the time this condition is true
					DestroyObject(visualPath.Last.Value);
					visualPath.RemoveLast();
				}

				playerScript.ReduceStatValue(Stat.AP, 1);
				if (playerScript.GetStatValue(Stat.AP) <= 0f && playerScript.GetStatValue(Stat.AP) >= 0f) {
					ClearPath();
					StopMoving();
				}
			} else { 
				step = Speed * Time.deltaTime;

				if (checkForNulls()) { // Nulls detected in movement routine. Stop
					ClearPath();
					StopMoving();
					return;
				}

				/* Look towards movement direction */
				targetPostition = new Vector3(visualPath.Last.Value.transform.position.x, 
				        transform.position.y, visualPath.Last.Value.transform.position.z );
				gameObject.transform.LookAt(targetPostition);

				gameObject.transform.position = Vector3.MoveTowards(
					gameObject.transform.position, 
					Tile.TileMiddle(path.First.Value), 
					step
				);

				camController.ResetCamera();
			}
		}
	}

	/**
	 * Stop this player from moving
	 */
	void StopMoving() {
		moving = Moving.NO;
		camController.UnlockCamera();
		Destroy(GameObject.FindGameObjectWithTag("Highlighted Tile"));
	}

	/**
	 * Return true if we are currently moving
	 * 
	 * Returns
	 * - true if moving == Moving.YES
	 */
	public bool IsMoving() {
		return moving == Moving.YES;
	}

	/**
	 * This does one of two things: visualises the player's movement choice, or it confirms
	 * and moves a player's movement choice. Which one is chosen depends on whether this is the
	 * first time the player has clicked the goal tile or the second time.
	 */
	public void RequestMovement(Tile goal) {
		PathTile dest; // The destination tile

		if (InteractiveTiles.Count >= 1) {
			InteractiveTiles[0].CloseEvent();
		}

		if (moving == Moving.YES) {
			return;
		}

		if (moving == Moving.POSSIBLY && goal.Equals(clickedTile)) {
			if (playerScript.GetStatValue(Stat.AP) < 0f || playerScript.GetStatValue(Stat.AP) > 0f)
				moving = Moving.YES;
			return;
		}

		if (moving == Moving.POSSIBLY) { /* Clears all visual elements of selected path */
			Destroy(GameObject.FindGameObjectWithTag("Highlighted Tile"));
			ClearPath();
		}

		if (UseInteractable(goal)) // We interacted with an object
			return;
		
		dest = FindPath(goal, false, false);

		if (dest != null) {
			cost = dest.Depth;
			SpawnHighlightedTile(goal);
			clickedTile = goal;
			moving = Moving.POSSIBLY;
			path = FlipPath(dest);
			dest = dest.Parent;
		}
		
		while (dest != null && dest.Parent != null) {
			if (dest.Depth > playerScript.GetStatValue(Stat.AP)) // Not enough AP to reach tile
				visualPath.AddLast(SpawnPathTile(dest, InvalidPathMarker) as GameObject);
			else // Enough AP to reach tile
				visualPath.AddLast(SpawnPathTile(dest, ValidPathMarker) as GameObject);
			dest = dest.Parent;
		}
	}

	/**
	 * Clearing the visual path
	 */
	public void ClearPath() {
		foreach (Object obj in visualPath)
			DestroyObject(obj);
		visualPath.Clear();
		if (Counter != null) {
			Destroy(Counter);
			Counter = null;
		}
	}

	/**
	 * Spawn a path tile
	 * 
	 * Arguments
	 * - Tile pos - The position to spawn this path tile
	 * - GameObject pathMarker - The type of tile to instantiate
	 */
	Object SpawnPathTile(Tile pos, GameObject pathMarker) {
		Vector3 tilePos = Tile.TileMiddle(pos); // The tile position
		tilePos.y = tilePos.y - HIGHLIGHTED_TILE_ELEVATION;
		Quaternion tileRot = Quaternion.Euler(90, 0, 0); // The rotation of the tile

		return Instantiate(pathMarker, tilePos, tileRot);
	}

	/**
	 * Spawn a highlighted tile
	 * 
	 * Arguments
	 * - Tile pos - The position to spawn this path tile
	 */
	void SpawnHighlightedTile(Tile pos) {
		Vector3 tilePos = Tile.TileMiddle(pos); // The position of the tile
		tilePos.y = tilePos.y - HIGHLIGHTED_TILE_ELEVATION;
		Quaternion tileRot = Quaternion.Euler(90, 0, 0); // The rotation of the tile

		visualPath.AddLast(Instantiate(HighlightedTile, tilePos, tileRot) as GameObject);
		ShowAPCost(pos);
	}

	/**
	 * Display the AP cost
	 * 
	 * Arguments
	 * - Tile pos - The position where we spawned the path tile
	 */
	void ShowAPCost(Tile pos) {
		Vector3 tilePos = Tile.TileMiddle(pos); // The position of the tile

		Counter = Instantiate(APCounter, tilePos, Quaternion.identity) as GameObject;
		Counter.transform.SetParent(GameObject.Find("Player UI").transform, true);
		Counter.GetComponentInChildren<Text>().text = "value goes here";
		Counter.transform.position = new Vector3(Input.mousePosition.x + 50f, 
		        Input.mousePosition.y + 50f, transform.position.z);
		Counter.GetComponentInChildren<Text>().text = cost.ToString();
	}

	/*
	 * Finds the shortest path from the player's position to the goal tile.
	 * Note that this returns the path as a link list and the front of the list
	 * is the end of the path.
	 * 
	 * Arguments
	 * - Tile goal - The goal of the path
	 * - bool allowBlockedGoal - Allow us to keep looking if the goal is blocked
	 * - bool stealth - Stops finding a path if the distance is too far. Used by Stealth objects
	 * as an performance improvement.
	 */
	PathTile FindPath(Tile goal, bool allowBlockedGoal, bool stealth) {
		HashSet<Tile> explored = new HashSet<Tile>(); // The explored tiles
		Tile neighbour; // The neighbour to a currently looked at tile
		PathTile current; // The currently looked at tile
		Queue<PathTile> q; // Queue of path tiles

		/* Breadth first search */
		if (goal != null && (allowBlockedGoal || !blockedTiles.Contains(goal))) {
			q = new Queue<PathTile>();
			q.Enqueue(new PathTile(playerScript.PlayerPosition()));
			while (q.Count != 0) {
				current = q.Dequeue();
				if (current.Equals(goal)) // Found the goal
					return current;
				for (int z = 1; z >= -1; z -= 2) {
					neighbour = new Tile(current.X + 0, current.Z + z);
					if (neighbour.Equals(goal)) // Goal is the neighbour
						return new PathTile(current, neighbour);
					if (stealth && new PathTile(current, neighbour).Depth > 10)
						continue;
					if (!blockedTiles.Contains(neighbour) && !explored.Contains(neighbour)) {
						explored.Add(neighbour);
						q.Enqueue(new PathTile(current, neighbour));
					}
				}
				for (int x = 1; x >= -1; x -= 2) {
					neighbour = new Tile(current.X + x, current.Z + 0);
					if (neighbour.Equals(goal)) // Neighbour is the goal
						return new PathTile(current, neighbour);
					if (stealth && new PathTile(current, neighbour).Depth > 10)
						continue;
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
	 * Static method for finding the distance between 2 positions
	 * 
	 * Arguments
	 * - Vector3 startPosition - The starting position
	 * - Vector3 endPosition - The destination
	 * - HashSet<Tile> blockedTiles - The blocked tiles
	 * 
	 * Returns
	 * - The distance between the start and destination. -1 if no path found
	 */
	public static int TileDistance(Vector3 startPosition, Vector3 endPosition, HashSet<Tile> blockedTiles) {
		PathTile pathFound = null; // The path that was found
		Tile startTile = Tile.TilePosition(startPosition); // The starting tile
		Tile endTile = Tile.TilePosition(endPosition); // The destination
		HashSet<Tile> explored = new HashSet<Tile>(); // The set of explored tiles
		Queue<PathTile> q; // Queue of path tiles
		Tile neighbour; // Neighbouring tile
		PathTile current; // The currently looked at tile

		if (endTile != null && !blockedTiles.Contains(endTile)) {
			q = new Queue<PathTile>();
			q.Enqueue(new PathTile(startTile));
			while (q.Count != 0) {
				current = q.Dequeue();
				if (current.Equals(endTile)) {
					pathFound = current;
				}
				for (int z = 1; z >= -1; z -= 2) {
					neighbour = new Tile(current.X + 0, current.Z + z);
					if (!blockedTiles.Contains(neighbour) && !explored.Contains(neighbour)) {
						explored.Add(neighbour);
						q.Enqueue(new PathTile(current, neighbour));
					}
				}
				for (int x = 1; x >= -1; x -= 2) {
					neighbour = new Tile(current.X + x, current.Z + 0);
					if (!blockedTiles.Contains(neighbour) && !explored.Contains(neighbour)) {
						explored.Add(neighbour);
						q.Enqueue(new PathTile(current, neighbour));
					}
				}
			}
		}
		if (pathFound == null)
			return -1;
		return pathFound.Depth;
	}

	/*
	 * Returns the distance between this player and another position. -1 if no path found
	 */
	public int TileDistance(Vector3 position, bool stealth) {
		PathTile foundPath = FindPath(Tile.TilePosition(position), true, stealth);

		if (foundPath == null) { // No path found. Unknown
			return 10000;
		}
		else { // Path found
			return foundPath.Depth;
		}
	}

	public int TileDistance(Tile position, bool stealth) {
		return TileDistance(Tile.TileMiddle(position), stealth);
	}
	
	/**
	 * Returns a backwards PathTile path as a LinkedList.
	 * 
	 * Arguments
	 * - PathTile pathToReverse - The path to flip
	 */
	LinkedList<Tile> FlipPath(PathTile pathToReverse) {
		LinkedList<Tile> res = new LinkedList<Tile>(); // New list of tiles

		while (pathToReverse != null) {
			res.AddFirst(new Tile(pathToReverse.X, pathToReverse.Z));
			pathToReverse = pathToReverse.Parent;
		}

		res.RemoveFirst(); // Don't need the original path position
		return res;
	}

	/**
	 * Sets the given tile to no longer be blocking. This means the movement path could now go 
	 * through that tile. NOTE: your blocker visuals (e.g. a wall) will still appear on
	 * the map unless you delete it yourself. 
	 */
	public void UnblockTile(Tile tile) {
		GameObject o; // The game object used to test blocked tiles
		Vector3 v; // The position of the tile we are unblocking

		if (blockedTiles.Contains(tile)) {
			if (debugging) {
				o = GameObject.CreatePrimitive(PrimitiveType.Cube);
				v = Tile.TileMiddle(tile);
				o.transform.position = v;
			}
			blockedTiles.Remove(tile);
		}
	}

	/**
	 * Block the given tile
	 * 
	 * Arguments
	 * - Tile tile - The tile to block
	 */
	public void BlockTile(Tile tile) {
		if (blockedTiles.Contains(tile))
			Debug.LogWarning("You tried to block a tile that was blocked. Did you want to do" +
				"this? FROM BEN");
		else
			blockedTiles.Add(tile);
	}

	/*
	 * Get Index of Interactable on a certain Tile.
	 * 
	 * Arguments
	 * - Tile tile - The tile to find
	 */
	public int GetInteractable(Tile tile) {
		for (int i = 0; i < InteractiveTiles.Count; i++) {
			if (tile.Equals(InteractiveTiles[i].GetTile())) // We found the tile
				return i;
		}
		return -1;
	}

	/*
	 * Checks if player is Adjacent to tile. 
	 * 
	 * Arguments
	 * - Tile tile - The tile to check for adjacency
	 * - Player player - The player to check for adjacency
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
	 * 
	 * Arguments
	 * - Tile tile - The tile to check
	 */ 
	public bool IsTileBlocked(Tile tile) {
		return blockedTiles.Contains(tile);
	}

	/**
	 * Add an interactable to the list of tiles
	 * 
	 * Arguments
	 * - InteractiveObject toAdd - the interactive object to add
	 */
	public void AddInteractable(InteractiveObject toAdd) {
		InteractiveTiles.Add(toAdd);
		blockedTiles.Add(toAdd.GetTile());
	}

	/**
	 * Remove this interactable from the list of tiles
	 * 
	 * Arguments
	 * - InteractiveObject toRemove - The interactive object to remove
	 */
	public void RemoveInteractable(InteractiveObject toRemove) {
		InteractiveTiles.Remove(toRemove);
		blockedTiles.Remove(toRemove.GetTile());
		Debug.Log("int Remove: " + toRemove.GetTile().ToString());
	}

	/**
	 * Remove the interactable on the given tile from the list of tiles
	 * 
	 * Arguments
	 * - Tile toRemove - The tile that contains an interactable object
	 */
	public void RemoveInteractable(Tile toRemove) {
		int index = GetInteractable(toRemove); // The index of the interactive object in the list

		if (index == -1) {
			Debug.Log("Can't find Tile");
			return;
		}
		blockedTiles.Remove(toRemove);
		InteractiveTiles.Remove(InteractiveTiles[index]);
		Debug.Log("int Remove: " + toRemove.ToString());
		Object.FindObjectOfType<GameManager>().OpenDoor(toRemove);
	}

	/**
	 * Use the interactable
	 * 
	 * Arguments
	 * - Tile goal - The tile that contains the interactable
	 */
	public bool UseInteractable(Tile goal) {
		int index; // The index of this tile in the list of interactable tiles

		if (((index = GetInteractable(goal)) != -1) && IsNear(goal, playerScript)) { // Found the interactive tile
			Debug.Log(index);
			InteractiveTiles[index].Interact();
			return true;
		}
		return false;
	}

	/**
	 * Reassign the player script (once the Player GameObject changes)
	 */
	public void ChangePlayerScript() { 
		playerScript = gameObject.GetComponent<Player>();
	}

	/**
	 * Return the hash set of blocked tiles
	 * 
	 * Return
	 * - The hash set of blocked tiles
	 */
	public HashSet<Tile> GetBlockedTiles() {
		return blockedTiles;
	}
}