using UnityEngine;
using System.Collections.Generic;

/**
 * Class handling all activation tiles
 */
public class ActivationTileController : MonoBehaviour {

	public GameObject StunGunTestPrefab; // Stun Gun Test Prefab
	public GameObject ActivationTilePrefab; // Activation Tile Prefab
	public GameObject TargettingTilePrefab; // Targetting Tile Prefab
	public Material OffensiveTileMaterial; // Material for an offensive tile
	public Material DefensiveTileMaterial; // Material for a defensive tile
	public Material SupportiveTileMaterial; // Material for a supportive tile

	HashSet<Tile> conceptualTiles = new HashSet<Tile>((new Tile())); // Set of conceptual (Tile) tiles
	HashSet<Tile> badActivationTiles = new HashSet<Tile>(new Tile()); // Container of bad activation tiles
	List<GameObject> gameObjectTiles = new List<GameObject>(); // List of instantiated tiles
	Item item; // Current item
	Ability ability; // Current calling ability
	MovementController movementController; // The movement controller

	/**
	 * Set the movement controller
	 * 
	 * Arguments
	 * - MovementController newMovementController
	 */
	public void SetMovementController(MovementController newMovementController) {
		movementController = newMovementController;
	}

	/**
	 * Start this activation tile controller script
	 */
	public void StartMe() {
		movementController = Player.MyPlayer.GetComponent<MovementController>();
	}

	/**
	 * Initiate Target Confirmation
	 * 
	 * Tile tileClicked - The tile that was clicked
	 */
	public void InitiateTargetConfirmation(Tile tileClicked) {
		Tile intendedTile; // The intended tile to spawn
		SoftDestroyActivationTiles(); // Destroy all tiles

		intendedTile = new Tile(tileClicked.X, tileClicked.Z); // Create a new conceptual tile
		conceptualTiles.Add(intendedTile);

		generateIndividualTargetTile((float)(tileClicked.X * 2), (float)(tileClicked.Z * 2));
	}

	/**
	 * Activate Item/Ability on the given tile
	 * 
	 * Tile tileClicked - The tile that was clicked
	 */
	public void Activate(Tile tileClicked) {
		if (item != null) {
			item.SetTestPrefab(StunGunTestPrefab);
			item.Activate(tileClicked); // Second, determine what to activate. 
		} else if (ability != null) {
			ability.Activate(tileClicked); // Activate ability
		}

		DestroyActivationTiles(); // Destroy all tiles
	}

	/**
	 * Returns a set of activation tiles. 
	 * 
	 * Returns
	 * - The set of activation tile objects
	 */
	public HashSet<Tile> ActivationTiles() {
		return conceptualTiles;
	}

	/**
	 * Destroy all activation tiles, but don't remove references
	 */
	public void SoftDestroyActivationTiles() {
		foreach (GameObject activationTile in gameObjectTiles) {
			Destroy(activationTile);
		}
		gameObjectTiles.Clear(); // Clear the activation tile list
		conceptualTiles.Clear(); // Clear the set of activation tiles (Tile class)
		badActivationTiles.Clear(); // Clear the list of bad activation tiles
	}

	/**
	 * Destroy all activation tiles
	 */
	public void DestroyActivationTiles() {
		foreach (GameObject activationTile in gameObjectTiles) {
			Destroy(activationTile);
		}
		gameObjectTiles.Clear(); // Clear the activation tile list
		conceptualTiles.Clear(); // Clear the set of activation tiles (Tile class)
		badActivationTiles.Clear(); // Clear the list of bad activation tiles

		/* Set references to null */
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
		
		// Second, set reference item. Set ability to be null
		item = null;
		ability = newAbility;

		// Third, start generating
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

		// Second, set references to item. Set ability to be null
		item = newItem;
		ability = null;

		// Third, start generating
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
	 * Helper function for checking if we can generate a tile at a given position
	 * 
	 * Arguments
	 * - float tileX - The x coordinate of the intended tile
	 * - float tilez - The z coordinate of the intended tile
	 * 
	 * Returns
	 * - True if we can generate a tile at this location
	 * - False if otherwise
	 */
	bool canGenerate(float tileX, float tileZ) {
		Tile intendedTile; // The intended tile to spawn

		intendedTile = new Tile(Tile.TilePosition(tileX), Tile.TilePosition(tileZ));

		if (movementController.IsTileBlocked(intendedTile)) return false;  // Tile is blocked

		conceptualTiles.Add(intendedTile);
		return true;
	}

	/**
	 * Helper function to generate one target tile
	 *
	 * Arguments
	 * - float x - The x coordinate
	 * - float y - The y coordinate
	 */
	void generateIndividualTargetTile(float x, float z) {
		GameObject tileGenerated; // The generated tile

		tileGenerated = Instantiate<GameObject>(TargettingTilePrefab);
		tileGenerated.GetComponent<Transform>().position = new Vector3(x, -0.49f, z);

		gameObjectTiles.Add(tileGenerated);
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

		tileGenerated = Instantiate<GameObject>(ActivationTilePrefab);
		tileGenerated.GetComponent<Renderer>().material = tileMaterial;
		tileGenerated.GetComponent<Transform>().position = new Vector3(x, -0.49f, z);

		gameObjectTiles.Add(tileGenerated);
	}

	/**
	 * Checks if the tiles adjacent to the corner are not blocking
	 * 
	 * Arugments
	 * - float cornerX - The corner's x coordinate
	 * - float cornerZ - The corner's y coordinate
	 * - float initialX - The origin x coordinate
	 * - float initialZ - The origin z coordinate
	 * 
	 * Returns
	 * - True if both adjacent tiles are valid
	 * - False if otherwise
	 */
	bool checkCornerAdjacency(float cornerX, float cornerZ, float initialX, float initialZ) {
		Tile adjacentTileOne, adjacentTileTwo; // The tiles adjacent to the corner

		if (cornerX > initialX && cornerZ > initialZ) { // This is the upper right corner
			adjacentTileOne = new Tile(Tile.TilePosition(cornerX - 2.0f), Tile.TilePosition(cornerZ));
			adjacentTileTwo = new Tile(Tile.TilePosition(cornerX), Tile.TilePosition(cornerZ - 2.0f));
		} else if (cornerX > initialX && cornerZ < initialZ) { // This is the lower right corner
			adjacentTileOne = new Tile(Tile.TilePosition(cornerX - 2.0f), Tile.TilePosition(cornerZ));
			adjacentTileTwo = new Tile(Tile.TilePosition(cornerX), Tile.TilePosition(cornerZ + 2.0f));
		} else if (cornerX < initialX && cornerZ > initialZ) { // This is the upper left corner
			adjacentTileOne = new Tile(Tile.TilePosition(cornerX + 2.0f), Tile.TilePosition(cornerZ));
			adjacentTileTwo = new Tile(Tile.TilePosition(cornerX), Tile.TilePosition(cornerZ - 2.0f));
		} else { // This is the lower left corner
			adjacentTileOne = new Tile(Tile.TilePosition(cornerX + 2.0f), Tile.TilePosition(cornerZ));
			adjacentTileTwo = new Tile(Tile.TilePosition(cornerX), Tile.TilePosition(cornerZ + 2.0f));
		}

		if (!movementController.IsTileBlocked(adjacentTileOne) && !movementController.IsTileBlocked(adjacentTileTwo))
			return true; // Adjacent tiles are valid. Corner tile can be generated

		return false; // One adjacen tile is invalid. Corner tile can't be generated
	}

	/**
	 * Generate tiles straight from the corner
	 * 
	 * Arguments
	 * - float range - The range from the corner tile we are trying to generate
	 * - float currentX - This corner's x coordinate
	 * - float currentZ - This corner's z coordinate
	 * - float initialX - The origin x coordinate
	 * - float initialZ - The origin z coordinate
	 * - Material tileMaterial - The material of the tile
	 */
	void generateStraightFromCorner(float range, float currentX, float currentZ, float initialX, float initialZ, 
			Material tileMaterial) {
		if (currentX > initialX && currentZ > initialZ) { // This is the upper right corner
			generateStraightLine(range, currentX, currentZ, tileMaterial, "right");
			generateStraightLine(range, currentX, currentZ, tileMaterial, "up");
		} else if (currentX > initialX && currentZ < initialZ) { // This is the lower right corner
			generateStraightLine(range, currentX, currentZ, tileMaterial, "right");
			generateStraightLine(range, currentX, currentZ, tileMaterial, "down");
		} else if (currentX < initialZ && currentZ > initialZ) { // This is the upper left corner
			generateStraightLine(range, currentX, currentZ, tileMaterial, "left");
			generateStraightLine(range, currentX, currentZ, tileMaterial, "up");
		} else { // This is the lower left corner
			generateStraightLine(range, currentX, currentZ, tileMaterial, "left");
			generateStraightLine(range, currentX, currentZ, tileMaterial, "down");
		}
	}

	/** 
	 * Generate a corner tile
	 * 
	 * Arguments
	 * - float range - The range from the corner tile we are trying to generate
	 * - float currentX - This corner tile's x coordinate
	 * - float currentZ - This corner tile's z coordinate
	 * - float initialX - The x coordinate from which this corner tile is going to be generated from
	 * - float initialZ - The z coordinate from which this corner tile is going to be generated from
	 * - Material tileMaterial - The tile material
	 */
	void generateCornerTile(float range, float currentX, float currentZ, float initialX, float initialZ, 
			Material tileMaterial) {
		if (range <= 0.0f && range >= 0.0f) return; // Don't generate anything

		// Generate centre tile
		if (!checkCornerAdjacency(currentX, currentZ, initialX, initialZ)) return; // Can't generate centre
		else if (!canGenerate(currentX, currentZ)) return; // Can't generate corner tile. Just don't do it

		generateIndividualTile(currentX, currentZ, tileMaterial); // Generate the corner tile

		if (range <= 2.0f && range >= 2.0f) return; // Only generate the corner tile

		// Generate tiles in a straight line from the corner
		generateStraightFromCorner(range, currentX, currentZ, initialX, initialZ, tileMaterial);

		if (range <= 4.0f && range >= 4.0f) return; // We only want to generate the tiles straight from the corner

		// Generate the next corner tile based on the position of this corner tile
		if (currentX > initialX && currentZ >= initialZ) { // Upper right corner 
			generateCornerTile(range - 2.0f, currentX + 2.0f, currentZ + 2.0f, currentX, currentZ, tileMaterial);
		} else if (currentX > initialX && currentZ <= initialZ) { // Lower right corner 
			generateCornerTile(range - 2.0f, currentX + 2.0f, currentZ - 2.0f, currentX, currentZ, tileMaterial);
		} else if (currentX < 0.0f && currentZ > 0.0f) { // Upper left corner
			generateCornerTile(range - 2.0f, currentX - 2.0f, currentZ + 2.0f, currentX, currentZ, tileMaterial);
		} else { // Lower left corner
			generateCornerTile(range - 2.0f, currentX - 2.0f, currentZ - 2.0f, currentX, currentZ, tileMaterial);
		}
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
		float trueRange = (float)range * 2.0f; // The true range for positioning purposes
		float trueX = (float)initialX; // True float values for X
		float trueZ = (float)initialZ; // True float values for Z

		// Firstly, we can just generate tiles in a straight line from the origin
		generateStraightLineTiles(range, initialX, initialZ, tileMaterial);

		// Secondly, check if the range is just 1. 
		if (range <= 1.0f && range >= 1.0f) return; // No need to generate more. 

		// Generate the corner tiles pseudo-recursively, if the corner's adjacent tiles are valid
		generateCornerTile(trueRange - 2.0f, trueX - 2.0f, trueZ + 2.0f, trueX, trueZ, tileMaterial);
		generateCornerTile(trueRange - 2.0f, trueX - 2.0f, trueZ - 2.0f, trueX, trueZ, tileMaterial);
		generateCornerTile(trueRange - 2.0f, trueX + 2.0f, trueZ - 2.0f, trueX, trueZ, tileMaterial);
		generateCornerTile(trueRange - 2.0f, trueX + 2.0f, trueZ + 2.0f, trueX, trueZ, tileMaterial);

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
		float trueRange = (float)range * 2.0f; // The true range for positioning purposes
		float trueX = (float)initialX; // True float values for X
		float trueZ = (float)initialZ; // True float values for Z

		// Simple. Just generate the tiles in a straight line
		generateStraightLine(trueRange, trueX, trueZ, tileMaterial, "left");
		generateStraightLine(trueRange, trueX, trueZ, tileMaterial, "right");
		generateStraightLine(trueRange, trueX, trueZ, tileMaterial, "up");
		generateStraightLine(trueRange, trueX, trueZ, tileMaterial, "down");

		// Generate the centre tile only if the centre tile is not blocked
		if (!canGenerate(trueX, trueZ)) return; // Centre tile is blocked
		generateIndividualTile(trueX, trueZ, tileMaterial);
	}

	/**
	 * Helper in generating tiles in a straight line. Only generate tiles until an invalid tile
	 * 
	 * Arguments
	 * - double range - The range
	 * - float initialX - The x coordinate of the centre
	 * - float initialZ - The z cordinate of the centre
	 * - Material tileMaterial - The material of the tiles
	 * - string direction - Determines the direction
	 */
	void generateStraightLine(double range, float initialX, float initialZ, Material tileMaterial, string direction) {
		switch (direction) { // Only generate based on the given direction 
		case "left": // Towards the left
			for (float currentX = (initialX - 2.0f); currentX >= (initialX - range); currentX -= 2.0f) {
				if (!canGenerate(currentX, initialZ)) return; // Blocked. Just stop
				generateIndividualTile(currentX, initialZ, tileMaterial);
			}
			return;
		case "right": // Towards the right
			for (float currentX = (initialX + 2.0f); currentX <= (initialX + range); currentX += 2.0f) {
				if (!canGenerate(currentX, initialZ)) return; // Blocked. Just stop
				generateIndividualTile(currentX, initialZ, tileMaterial);
			}
			return;
		case "up": // Upwards
			for (float currentZ = (initialZ + 2.0f); currentZ <= (initialZ + range); currentZ += 2.0f) {
				if (!canGenerate(initialX, currentZ)) return; // Blocked. Just stop
				generateIndividualTile(initialX, currentZ, tileMaterial);
			}
			return;
		case "down": // Downwards
			for (float currentZ = (initialZ - 2.0f); currentZ >= (initialZ - range); currentZ -= 2.0f) {
				if (!canGenerate(initialX, currentZ)) return; // Blocked. Just stop
				generateIndividualTile(initialX, currentZ, tileMaterial);
			}
			return;
		}
	}
	
}
