using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a tile's positin in the game. These positions are relative to the tile at origin.
 */
public class Tile : IComparer<Tile>, IEqualityComparer<Tile> {
	public int X;
	public int Z;
	
	public Tile(int x, int z) {
		this.X = x;
		this.Z = z;
	}
	
	public Tile() {
		;
	}
	
	public override bool Equals(object obj) {
		if (!(obj is Tile)) {
			return false;
		}
		
		Tile t = obj as Tile;
		return (t.Z == this.Z) && (t.X == this.X);
	}
	
	public override int GetHashCode() {
		/* josh sucks
		 * 		- Ben
		 */
		int prime = 13; // Prime number
		int hashCode = 0; // The hash code to return
		hashCode = (hashCode * prime) + this.X;
		hashCode = (hashCode * prime) + this.Z;
		return hashCode;
	}
	
	public int Compare(Tile tile1, Tile tile2) {
		if (tile1 == null || tile2 == null) {
			throw new System.NullReferenceException();
		}
		
		if (tile1.Z == tile2.Z) {
			return tile1.X - tile2.X;
		} else {
			return tile2.Z - tile2.Z;
		}
	}
	
	public override string ToString() {
		return string.Format("X: {0}, Z: {1}", this.X, this.Z);
	}
	
	public bool Equals(Tile t1, Tile t2) {
		return (t1.X == t2.X) && (t1.Z == t2.Z);
	}
	
	public int GetHashCode(Tile t) {
		return (t.X.GetHashCode() + t.Z).GetHashCode();
	}

	
	/*
	 * Converts the given pos to a tile relative to the tile at (0,0).
	 * For example, if you give 2.5, the tile is the 1st tile away from
	 * the centre tile.
	 */ 
	public static int TilePosition(float pos) {
		return (int) Mathf.Ceil((pos - 1) / 2);
	}

	public static Tile TilePosition(float x, float z){
		return new Tile(
			TilePosition(x),
			TilePosition(z)
		);
	}
	
	/*
	 * Returns the middle Vector3 position of a tile.
	 */
	public static Vector3 TileMiddle(Tile t) {
		Vector3 res = new Vector3(t.X * 2, 0, t.Z * 2);
		return res;
	}

	/**
	 * Converts the mouse's current position to a Tile.
	 * 
	 * If you're unsure about what the LayerMask does, ask Ben or try something.
	 * 
	 * PROBLEMS: works when clicking UI
	 */
	public static Tile MouseToTile(LayerMask layerMask) {
		Tile goal = null;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
			goal = new Tile();
			goal.X = Tile.TilePosition(hit.point.x);
			goal.Z = Tile.TilePosition(hit.point.z);
		}

		return goal;
	}
}
