using UnityEngine;
using System.Collections;

/**
 * This is an abstract class which should be inherited 
 * by all items in the game.
 */
public abstract class Item : MonoBehaviour {

	public string itemName; // The name of this item
	protected string itemDescription; // The description of this item

	/*
	 * The stat affected by the given equipment and the value 
	 * by which the stat is affected by.
	 * If this has a size of 0, then no stat is affected
	 * Else, the indices of stats correspond to the indices 
	 * of the values
	 * 
	 * e.g. If statsAffected[0] == Stat.HP, then 
	 * valueEffect[0] corresponds to how much that 
	 * stat will be increased/decreased.
	 * 
	 * if a value in valueEffect is positive, it means that the 
	 * affected stat is increased. Otherwise, it is decreased.
	 * 
	 * Note: valueEffect is not in terms of percentages. Which means 
	 * that if valueEffect[0] == 1, then Stat.HP is now: Stat.HP + 1.
	 * 
	 * For consumables, statsAffected is how much a stat is 
	 * affected on use
	 */
	protected Stat[] statsAffected; 
	protected double[] valueEffect;

}
