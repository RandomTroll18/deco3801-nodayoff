using UnityEngine;

/**
 * This is an abstract class which should be inherited 
 * by all items in the game.
 */
using System.Collections.Generic;


public abstract class Item : MonoBehaviour {

	public string ItemName; // The name of this item
	public Sprite Image; // The icon for this image
	protected string ItemDescription; // The description of this item
	protected List<TurnEffect> TurnEffects; // The turn effects in this item
	
	/*
	 * The effects in this item.
	 * 
	 * Concerning the array of doubles:
	 * double[0] => to be added (e.g. Stat.AP += double[0])
	 * double[1] => to be set (e.g. Stat.AP = double[1])
	 * double[2] => to be multiplied (e.g. Stat.AP *= double[2])
	 */
	protected Dictionary<Stat, double[]> Effects;
	
	/**
	 * Function used to activate the item. 
	 * Overridable by subclasses
	 */
	public virtual void Activate() {
		Debug.Log ("Item: " + ItemName + " Activated");
	}

	/**
	 * Get the turn effects in this item
	 * 
	 * Returns
	 * - A list of turn effects
	 */
	public List<TurnEffect> GetTurnEffects() {
		return TurnEffects;
	}

	/**
	 * Get the attached non-turn effects in this item.
	 * 
	 * Returns
	 * - A dictionary of effects
	 */
	public Dictionary<Stat, double[]> GetEffects() {
		return Effects;
	}
}
