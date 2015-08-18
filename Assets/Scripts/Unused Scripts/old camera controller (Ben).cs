//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class CameraController : MonoBehaviour {
//	
//	class Tile {
//		public int x;
//		public int z;
//		
//		public Tile (int x, int z) {
//			this.x = x;
//			this.z = z;
//		}
//		
//		public Tile () {
//			;
//		}
//	}
//	
//	class OpenTile : Tile {
//		public int depth;
//		public Tile parent;
//	}
//	
//	class TileComparer : IComparer<Tile> {
//		public int Compare (Tile tile1, Tile tile2) {
//			if (tile1 == null || tile2 == null)
//				throw new System.NullReferenceException ();
//			
//			if (tile1.z == tile2.z)
//				return tile1.x - tile2.x;
//			else
//				return tile2.z - tile2.z;
//		}
//	}
//	
//	public GameObject player;
//	public float camSpeed;
//	// The distance the mouse pointer needs to be from the edge before the screen moves.
//	public float GUISize;
//	public LayerMask layerMask;
//	
//	private Vector3 offset;
//	// private List<Tile> blockedTiles = new List<Tile> ();
//	
//	void Start () {
//		offset = transform.position - player.transform.position;
//		
//		/*
//		// Initalise sorted list of all blocked tiles
//		GameObject[] blockers = GameObject.FindGameObjectsWithTag ("Blocking");
//		foreach (GameObject element in blockers) {
//			blockedTiles.Add (new Tile (
//				TilePosition (element.transform.position.x), 
//				TilePosition (element.transform.position.z)
//				));
//			blockedTiles.Sort (new TileComparer ());
//		}
//		*/
//		
//		/*
//		blockedTiles.ForEach (delegate (Tile tile) {
//			Debug.Log (tile.x + " " + tile.z);
//			GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			Vector3 v = TileMiddle (tile);
//			o.transform.position = v;
//		});
//		*/
//	}
//	
//	void Update() {
//		// Camera panning with mouse
//		Rect recdown = new Rect (0, 0, Screen.width, GUISize);
//		Rect recup = new Rect (0, Screen.height - GUISize, Screen.width, GUISize);
//		Rect recleft = new Rect (0, 0, GUISize, Screen.height);
//		Rect recright = new Rect (Screen.width - GUISize, 0, GUISize, Screen.height);
//		
//		if (recdown.Contains(Input.mousePosition))
//			transform.Translate(0, 0, -camSpeed, Space.World);
//		
//		if (recup.Contains(Input.mousePosition))
//			transform.Translate(0, 0, camSpeed, Space.World);
//		
//		if (recleft.Contains(Input.mousePosition))
//			transform.Translate(-camSpeed, 0, 0, Space.World);
//		
//		if (recright.Contains(Input.mousePosition))
//			transform.Translate(camSpeed, 0, 0, Space.World);
//		
//		// Mouse click detection
//		if (Input.GetMouseButtonUp (0)) {
//			Tile goal = new Tile();
//			
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit;
//			if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
//				// Debug.Log (hit.point);
//				goal.x = TilePosition (hit.point.x);
//				goal.z = TilePosition (hit.point.z);
//				// Debug.Log (goal.x + " " + goal.z);
//			}
//			
//			/*
//			if (blockedTiles.BinarySearch (goal, new TileComparer ()) >= 0) {
//				Debug.Log ("Goal is free");
//				Queue q = new Queue ();
//				q.Enqueue (goal);
//			} else {
//				Debug.Log ("Goal is blocked");
//			}
//			*/
//		}
//	}
//	
//	/*
//	 * Converts the given pos to a tile relative to the tile at (0,0).
//	 * For example, if you give 2.5, the tile is the 1st tile away from
//	 * the centre tile.
//	 */ 
//	int TilePosition (float pos) {
//		return (int) Mathf.Ceil ((pos - 1) / 2);
//	}
//	
//	/*
//	 * Returns the middle position of a tile.
//	 */
//	Vector3 TileMiddle (Tile t) {
//		Vector3 res = new Vector3 (t.x * 2, 0, t.z * 2);
//		return res;
//	}
//	
//	/*
//	 * Call this when the player moves.
//	 */
//	public void ResetCamera () {
//		transform.position = player.transform.position + offset;
//	}
//}
