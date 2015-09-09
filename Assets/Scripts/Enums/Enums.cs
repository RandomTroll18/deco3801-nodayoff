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
