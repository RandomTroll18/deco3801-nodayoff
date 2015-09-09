using UnityEngine;
using System.Collections.Generic;

/**
 * Class handling all activation tiles
 */
public class ActivationTileHandler : MonoBehaviour {

	public GameObject ActivationTilePrefab; // Activation Tile Prefab

	List<GameObject> activationTileList = new List<GameObject>(); // List of activation tiles

	/**
	 * Destroy all activation tiles
	 */
	public void DestroyActivationTiles() {
		foreach (GameObject activationTile in activationTileList) {
			Destroy(activationTile);
		}
	}

	/**
	 * Generate Activation Tiles based on the range and 
	 * center given
	 * 
	 * Arguments
	 * - double range - The range
	 * - int initialX - The X coordinate of the centre
	 * - int initialY - The Y coordinate of the centre
	 * - RangeType rangeType - The type of range
	 * - ActivationType activationType - The activation type
	 */
	public void GenerateActivationTiles(double range, int initialX, int initialZ, RangeType rangeType, 
			ActivationType activationType) {
		Color tileColour ; // The tile colour
		// Some debugging
		Debug.Log("Generate Activation Tiles");
		Debug.Log("Range: " + range);
		Debug.Log("Centre: (" + initialX + ", 0, " + initialZ + ")");

		// First, destroy previous activation tiles
		DestroyActivationTiles();

		// Second, determine the tile colour
		switch (activationType) {
		case ActivationType.OFFENSIVE: 
			tileColour = new Color((float)180, (float)0, (float)0);
			break;
		case ActivationType.DEFENSIVE:
			tileColour = Color.blue;
			break;
		case ActivationType.SUPPORTIVE:
			tileColour = Color.green;
			break;
		default: goto case ActivationType.DEFENSIVE; // Default is just blue
		}

		// Third determine how tiles are generated
		switch (rangeType) {
		case RangeType.SQUARERANGE:
			generateSquareRangeTiles(range, initialX, initialZ, tileColour);
			break;
		case RangeType.STRAIGHTLINERANGE:
			generateStraightLineTiles(range, initialX, initialZ, tileColour);
			break;
		default: goto case RangeType.SQUARERANGE; // Default is the square range
		}
	}

	/**
	 * Generate tiles in the form of a square
	 * 
	 * Arguments
	 * - double range - The range
	 * - int initialX - The X coordinate of the centre
	 * - int initialZ - The Z coordinate of the centre
	 * - Color tileColour - The colour of the tiles
	 */
	void generateSquareRangeTiles(double range, int initialX, int initialZ, Color tileColour) {
		GameObject tileGenerated; // The generated tile
		float trueRange = (float)range + (float)1; // The true range for positioning purposes

		/*
		 * Easy case: simply generate tiles sticking out of the square at the following positions:
		 * - (initialX + range, initialZ)
		 * - (initialX - range, initialZ)
		 * - (initialX, initialZ + range)
		 * - (initialX, initialZ - range)
		 */
		tileGenerated = Instantiate<GameObject>(ActivationTilePrefab);
		tileGenerated.GetComponent<Renderer>().material.SetColor("_Color", tileColour);
		tileGenerated.GetComponent<Transform>().position = 
				new Vector3(((float)initialX + trueRange), (float)0.0, (float)initialZ);
		activationTileList.Add(tileGenerated);

		tileGenerated = Instantiate<GameObject>(ActivationTilePrefab);
		tileGenerated.GetComponent<Renderer>().material.SetColor("_Color", tileColour);
		tileGenerated.GetComponent<Transform>().position = 
			new Vector3(((float)initialX - trueRange), (float)0.0, (float)initialZ);
		activationTileList.Add(tileGenerated);

		tileGenerated = Instantiate<GameObject>(ActivationTilePrefab);
		tileGenerated.GetComponent<Renderer>().material.SetColor("_Color", tileColour);
		tileGenerated.GetComponent<Transform>().position = 
			new Vector3((float)initialX, (float)0.0, ((float)initialZ + trueRange));
		activationTileList.Add(tileGenerated);

		tileGenerated = Instantiate<GameObject>(ActivationTilePrefab);
		tileGenerated.GetComponent<Renderer>().material.SetColor("_Color", tileColour);
		tileGenerated.GetComponent<Transform>().position = 
			new Vector3((float)initialX, (float)0.0, ((float)initialZ - trueRange));
		activationTileList.Add(tileGenerated);
	}

	/**
	 * Generate tiles in the straight lines only
	 * 
	 * Arguments
	 * - double range - The range
	 * - int initialX - The X coordinate of the centre
	 * - int initialZ - The Z coordinate of the centre
	 * - Color tileColour - The colour of the tiles
	 */
	void generateStraightLineTiles(double range, int initialX, int initialZ, Color tileColour) {
		
	}
	
}
