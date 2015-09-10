using UnityEngine;
using System.Collections;

/**
 * This script contains the enums used in this game
 */

/*
 * Enum for stats
 * AP - Action Points
 * VISION - Range of Sight
 */
public enum Stat {
	AP,
	VISION
};

/*
 * Enum for Contexts (for Context-Aware Box)
 * IDLE - Idle context
 * INVENTORY - Inventory context
 */
public enum Context {
	IDLE,
	INVENTORY
};

/*
 * Enum for action costs
 * DOORS - Door Actions
 * REPAIR - Repair Actions
 * MOVEMENT - Movement Actions
 * TECH - Tech Actions 
 * STRENGTH - Strength Actions
 */
public enum ActionCost {
	DOORS,
	REPAIR,
	MOVEMENT,
	TECH,
	STRENGTH
};

/*
 * Enum for Activation Types - Tile Generation Purposes
 * OFFENSIVE - Offensive Activation - Need Reddish Activation Tiles
 * DEFENSIVE - Defensive Activation - Need Blueish Activation Tiles
 * SUPPORTIVE - Supportive Activation (e.g. Healing) - Need Greenish Activation Tiles
 */
public enum ActivationType {
	OFFENSIVE, 
	DEFENSIVE, 
	SUPPORTIVE
};

/*
 * Enum for Range Types
 * SQUARERANGE - Range is in the form of a square
 * STRAIGHTLINERANGE - Range is in a straight line
 */
public enum RangeType {
	SQUARERANGE,
	STRAIGHTLINERANGE
};
