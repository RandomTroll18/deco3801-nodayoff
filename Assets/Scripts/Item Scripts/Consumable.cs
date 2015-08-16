using UnityEngine;
using System.Collections;

/**
 * This is the class inherited by items that can 
 * be consumed (but not equipped)
 */
public abstract class Consumable : Item {

	/*
	 * The amount of this consumable. Needs to be public 
	 * because some objects that are designated to be 
	 * consumables may have more than 1
	 */
	public int amount;
}
