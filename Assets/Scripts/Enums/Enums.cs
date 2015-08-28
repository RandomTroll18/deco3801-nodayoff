using UnityEngine;
using System.Collections;

/**
 * This script contains the enums used in this game
 */

/*
 * Enum for stats
 * HP - Health
 * DEF - Defense - Not Confirmed
 * AP - Action Points
 */
public enum Stat {
	HP,
	DEF, 
	AP
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
