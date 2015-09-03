using UnityEngine;
using System.Collections;

/**
 * This script contains the enums used in this game
 */

/*
 * Enum for stats
 * AP - Action Points
 * STUN - Stun Flag
 * VISION - Range of Sight
 */
public enum Stat {
	AP,
	STUN,
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
