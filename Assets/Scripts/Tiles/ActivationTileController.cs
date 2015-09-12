using UnityEngine;
using System.Collections.Generic;

/**
 * Class handling all activation tiles
 */
public class ActivationTileController : MonoBehaviour {

	public GameObject ActivationTilePrefab; // Activation Tile Prefab
	public Material OffensiveTileMaterial; // Material for an offensive tile
	public Material DefensiveTileMaterial; // Material for a defensive tile
	public Material SupportiveTileMaterial; // Material for a supportive tile

	HashSet<Tile> activationTiles = new HashSet<Tile>((new Tile())); // Container of activation tiles
	List<GameObject> activationTileList = new List<GameObject>(); // List of activation tiles
	Player player; // Current calling player
	Item item; // Current item
	Ability ability; // Current calling ability
	MovementController movementController; // The movement controller

	/**
	 * Activate Item/Ability on the given tile
	 * 
	 * Tile tileClicked - The tile that was clicked
	 */
	public void Activate(Tile tileClicked) {
		if (ability == null) {
			Debug.Log("Ability is null during activation");
		} else {
			Debug.Log("Ability being activated: " + ability.GetAbilityName());
		}
		if (item != null) {
			Debug.Log("Add Activation Tile Prefab To Stun Gun");
			item.SetTestPrefab(ActivationTilePrefab);
			item.Activate(tileClicked); // Second, determine what to activate. 
		} else if (item == null) ability.Activate(tileClicked); // Activate ability

		DestroyActivationTiles(); // Destroy all visual tiles
	}

	/**
	 * Returns a set of activation tiles. 
	 * 
	 * Returns
	 * - The set of activation tile objects
	 */
	public HashSet<Tile> ActivationTiles() {
		return activationTiles;
	}

	/**
	 * Destroy all activation tiles
	 */
	public void DestroyActivationTiles() {
		foreach (GameObject activationTile in activationTileList) {
			Destroy(activationTile);
		}
		activationTileList.Clear(); // Clear the activation tile list

		/* Set referenes to null */
		player = null;
		item = null;
		ability = null;
	}


	/**
	 * Interface for Player used to generate  activation tiles for ability activation
	 * 
	 * Arguments
	 * - Player player - The player
	 * - Ability ability - The ability being activated
	 */
	public void GeneratorInterface(Player newPlayer, Ability newAbility) {
		Debug.Log("Ability Activation Interface");
		double abilityRange = newAbility.GetRange();
		/* The X and Z Coordinates of the calling player */
		int playerPositionX = newPlayer.PlayerPosition().X * 2;
		int playerPositionZ = newPlayer.PlayerPosition().Z * 2;
		RangeType rangeType = newAbility.GetRangeType();
		ActivationType activationType = newAbility.GetActivationType();

		// First, destroy previous activation tiles
		DestroyActivationTiles();
		
		// Second, set references to player and item. Set ability to be null
		player = newPlayer;
		item = null;
		ability = newAbility;

		// Third, get movement controller
		movementController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MovementController>();
		
		// Fourth, start generating
		generateActivationTiles(abilityRange, playerPositionX, playerPositionZ, rangeType, activationType);
	}

	/**
	 * Interface for Context Aware Box used to generate activation tiles for item activation
	 * 
	 * Arguments
	 * - Player player - The player
	 * - Item item - The item
	 */
	public void GeneratorInterface(Player newPlayer, Item newItem) {
		double itemRange = newItem.GetRange(); // The item's range
		/* The X and Z Coordinates of the calling player */
		int playerPositionX = newPlayer.PlayerPosition().X * 2;
		int playerPositionZ = newPlayer.PlayerPosition().Z * 2;
		RangeType rangeType = newItem.GetRangeType();
		ActivationType activationType = newItem.GetActivationType();

		// First, destroy previous activation tiles
		DestroyActivationTiles();

		// Second, set references to player and item. Set ability to be null
		player = newPlayer;
		item = newItem;
		ability = null;

		// Third, get movement controller
		movementController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MovementController>();


		// Fourth, start generating
		generateActivationTiles(itemRange, playerPositionX, playerPositionZ, rangeType, activationType);
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
	void generateActivationTiles(double range, int initialX, int initialZ, RangeType rangeType, 
			ActivationType activationType) {
		Material tileMaterial; // The material to set
		// Some debugging
		Debug.Log("Generate Activation Tiles");
		Debug.Log("Range: " + range);
		Debug.Log("Centre: (" + initialX + ", 0, " + initialZ + ")");

		// First, determine the tile colour
		switch (activationType) {
		case ActivationType.OFFENSIVE: 
			tileMaterial = OffensiveTileMaterial;
			break;
		case ActivationType.DEFENSIVE:
			tileMaterial = DefensiveTileMaterial;
			break;
		case ActivationType.SUPPORTIVE:
			tileMaterial = SupportiveTileMaterial;
			break;
		default: goto case ActivationType.DEFENSIVE; // Default is just blue
		}

		// Second determine how tiles are generated and generate them accordingly
		switch (rangeType) {
		case RangeType.SQUARERANGE:
			generateSquareRangeTiles(range, initialX, initialZ, tileMaterial);
			break;
		case RangeType.STRAIGHTLINERANGE:
			generateStraightLineTiles(range, initialX, initialZ, tileMaterial);
			break;
		default: goto case RangeType.SQUARERANGE; // Default is the square range
		}
	}

	/**
	 * Helper function to generate one tile
	 * 
	 * Arguments
	 * - float x - The x coordinate to generate the tile
	 * - float z - The z coordinate to generate the tile
	 * - Material tileMaterial - The tile material
	 */
	void generateIndividualTile(float x, float z, Material tileMaterial) {
		GameObject tileGenerated; // The generated tile
		Tile newTile; // The new tile object

		newTile = new Tile(Tile.TilePosition(x), Tile.TilePosition(z));
		if (movementController.IsTileBlocked(newTile)) return; // Don't generate tile

		/* Tile is not blocked. Generate it */
		tileGenerated = Instantiate<GameObject>(ActivationTilePrefab);
		tileGenerated.GetComponent<Renderer>().material = tileMaterial;
		Debug.Log("Tile Generated Colour: " + tileMaterial.ToString());
		tileGenerated.GetComponent<Transform>().position = 
				new Vector3(x, 0.0f, z);

		activationTileList.Add(tileGenerated);
		activationTiles.Add(newTile);
	}

	/**
	 * Generate tiles in the form of a square
	 * 
	 * Arguments
	 * - double range - The range
	 * - int initialX - The X coordinate of the centre
	 * - int initialZ - The Z coordinate of the centre
	 * - Material tileMaterial - The material of the tiles
	 */
	void generateSquareRangeTiles(double range, int initialX, int initialZ, Material tileMaterial) {
		float trueRange = (float)range + (float)1; // The true range for positioning purposes
		float trueX = (float)initialX; // True float values for X
		float trueZ = (float)initialZ; // True float values for Z

		/*
		 * Easy case: simply generate tiles sticking out of the square at the following positions:
		 * - (initialX + range, initialZ)
		 * - (initialX - range, initialZ)
		 * - (initialX, initialZ + range)
		 * - (initialX, initialZ - range)
		 */

		generateIndividualTile(trueX + trueRange, trueZ, tileMaterial);
		generateIndividualTile(trueX - trueRange, trueZ, tileMaterial);
		generateIndividualTile(trueX, trueZ + trueRange, tileMaterial);
		generateIndividualTile(trueX, trueZ - trueRange, tileMaterial);
		
		if (range - 1.0f <= 0f && range - 1.0f >= 0) {
			generateIndividualTile(trueX, trueZ, tileMaterial);
			return;
		}
		else if (range - 2.0f <= 0f && range - 2.0f >= 0) { // Simply generate a square of tiles
			for (float currentZ = (trueZ + 2.0f); currentZ > (trueZ - 3.0f); currentZ -= 2.0f) {
				for (float currentX = (trueX - 2.0f); currentX < (trueX + 3.0f); currentX += 2.0f) {
					Debug.Log("Generate at: (" + currentX + ", " + currentZ + ")");
					generateIndividualTile(currentX, currentZ, tileMaterial);
				}
			}
			return;
		}
		else { // Range is bigger than 2.
			Debug.Log("Range is bigger than 2");

			/*
			 * Additional easy cases: beneath each of the 4 easy cases, three tiles are generated at the 
			 * following positions, assuming we are handling the top easy case:
			 * 
			 * - (easyCaseX - 1, easyCaseZ - 1)
			 * - (easyCaseX, easyCaseZ - 1)
			 * - (easyCaseX + 1, easyCaseZ - 1)
			 */

			/* Right case */
			generateIndividualTile((trueX + trueRange - 1.0f), trueZ + 1.0f, tileMaterial);
			generateIndividualTile((trueX + trueRange - 1.0f), trueZ, tileMaterial);
			generateIndividualTile((trueX + trueRange - 1.0f), trueZ + 1.0f, tileMaterial);

			/* Left case */
			generateIndividualTile((trueX - trueRange + 1.0f), trueZ + 1.0f, tileMaterial);
			generateIndividualTile((trueX - trueRange + 1.0f), trueZ, tileMaterial);
			generateIndividualTile((trueX - trueRange + 1.0f), trueZ - 1.0f, tileMaterial);

			/* Top case */
			generateIndividualTile(trueX + 1.0f, (trueZ + trueRange - 1.0f), tileMaterial);
			generateIndividualTile(trueX, (trueZ + trueRange - 1.0f), tileMaterial);
			generateIndividualTile(trueX - 1.0f, (trueZ + trueRange - 1.0f), tileMaterial);

			/* Bottom case */
			generateIndividualTile(trueX + 1.0f, (trueZ - trueRange + 1.0f), tileMaterial);
			generateIndividualTile(trueX, (trueZ - trueRange + 1.0f), tileMaterial);
			generateIndividualTile(trueX - 1.0f, (trueZ - trueRange + 1.0f), tileMaterial);

			/* 
			 * Generate square where the bounds are:
			 * trueX - trueRange + 1 < X < trueX + trueRange - 1
			 * trueZ - trueRange + 1 < Z < trueZ + trueRange - 1
			 */
			for (float currentZ = (trueZ + trueRange - 2.0f); 
					currentZ > (trueZ - trueRange + 1.0f); 
			     	currentZ -= 1.0f) {
				for (float currentX = (trueX - trueRange + 2.0f);
						currentX < (trueX + trueRange - 1.0f);
						currentX += 1.0f) {
					generateIndividualTile(currentX, currentZ, tileMaterial);
				}
			}
		}

	}

	/**
	 * Generate tiles in the straight lines only
	 * 
	 * Arguments
	 * - double range - The range
	 * - int initialX - The X coordinate of the centre
	 * - int initialZ - The Z coordinate of the centre
	 * - Material tileMaterial - The material of the tiles
	 */
	void generateStraightLineTiles(double range, int initialX, int initialZ, Material tileMaterial) {
		
	}
	
}
