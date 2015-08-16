using UnityEngine;
using System.Collections;

/**
 * This is the weapon class inherited by equipment that 
 * are to be weapons
 */
public abstract class Weapon : Equipment {
	
	//protected WeaponSlots weaponSlot; // The weapon slots used by this weapon
	protected double damage; // The amount of damage done by this weapon
	protected double range; // The range of the weapon. Unit of range not determined
	/*
	 * Number of rounds per use. Needs to be double because you can have something 
	 * like a flamethrower
	 */
	protected double rounds; // The number of rounds per use. Needs to be dou
}
