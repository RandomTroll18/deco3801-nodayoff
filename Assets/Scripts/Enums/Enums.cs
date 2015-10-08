﻿using UnityEngine;
using System.Collections;

/**
 * This script contains the enums used in this game
 */

/*
 * Enum for stats
 * AP - Action Points
 * VISION - Range of Sight
 * TECHMULTIPLIER
 */
public enum Stat {
	AP,
	/*
	 * Can only be a value of 1, 2 or 3
	 */
	VISION, 
	TECHMULTIPLIER,
	ENGMULTIPLIER,
	MARINEMULTIPLIER,
	SCOUTMULTIPLIER,
	NOMULTIPLIER
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
 * GLOBALTARGETRANGE - Range is global, showing valid targets around the map
 */
public enum RangeType {
	SQUARERANGE,
	STRAIGHTLINERANGE, 
	GLOBALTARGETRANGE
};

/*
 * Enum for turn effect type
 * STATEFFECT - Affect stat
 * MATERIALEFFECT - Affect material
 * ITEMEFFECT - Affect items
 */
public enum TurnEffectType {
	STATEFFECT, 
	MATERIALEFFECT,
	ITEMEFFECT
}

/*
 * Enum for item turn effect type
 * EXTRAUSE - Extra use per turn
 * COOLDOWNREDUCTION - Reduction in cooldown
 */
public enum ItemTurnEffectType {
	EXTRAUSE, 
	COOLDOWN
}
