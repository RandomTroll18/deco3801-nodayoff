using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * This is an abstract class which should be inherited 
 * by all items in the game.
 */
public abstract class Item : MonoBehaviour {

	public string itemName; // The name of this item
	public Sprite image; // The icon for this image
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

	/**
	 * Function used to activate the item. 
	 * Overridable by subclasses
	 */
	public virtual void Activate () {
		Debug.Log ("Item: " + this.itemName + " Activated");
	}


	/**
	 * Function used to activate this item's turn status effects
	 * Overridable by subclasses
	 */
	public virtual void inflictTurnStatEffect () {
		Debug.Log ("Turn-based stat effect activated");
	}
}
