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
 * Enum for Armour Slots.
 * HEAD - The head of the player
 * CHEST - The chest of the player
 * ARMS - The arms of the player
 * HANDS - The hands of the player
 * LEGS - The legs of the player
 * FEET - The feet of the player
 * ACC - An accessory for the player
 */
public enum ArmourSlots {
	HEAD, 
	CHEST,
	ARMS,
	HANDS,
	LEGS,
	FEET,
	ACC
};

/*
 * Enum for weapon slots. Only 2 types for now:
 * ONEHAND - Weapon can be used with only one hand
 * TWOHAND - Weapon needs to be used by both hands
 */
public enum WeaponSlots {
	ONEHAND,
	TWOHAND
};
