using UnityEngine;

/**
 * This is an abstract class which should be inherited 
 * by all items in the game.
 */
using System.Collections.Generic;


public abstract class Item : MonoBehaviour {

	public string ItemName; // The name of this item
	public Sprite Image; // The icon for this image
	public GameObject TestPrefab; // Prefab to show where Stun Gun is activated - MVP Purposes

	protected string ItemDescription; // The description of this item
	protected List<TurnEffect> TurnEffects; // The turn effects in this item
	protected int CoolDown; // Cool down for activation. This is in terms of turns
	protected int CoolDownSetting; // The amount of turns that this item should be cooling down for
	protected double Range = 1; // The range of this item's activation action
	protected ActivationType ItemActivationType; // Activation Type of this item
	protected RangeType ItemRangeType; // The range type of this item
	protected bool Activatable; // Record if this item can be activated
	
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
	 * Function for MVP purposes
	 * 
	 * Arguments
	 * - GameObject newTestPrefab - The new test prefab for stun gun
	 */
	public void SetTestPrefab(GameObject newTestPrefab) {
		TestPrefab = newTestPrefab;
	}

	/**
	 * Get the Activation Type of this item
	 * 
	 * Returns
	 * - The activation type of this item
	 */
	public ActivationType GetActivationType() {
		return ItemActivationType;
	}

	/**
	 * Get the Range Type of this item
	 * 
	 * Returns
	 * - The range type of this item
	 */
	public RangeType GetRangeType() {
		return ItemRangeType;
	}

	/**
	 * Get the range of this item
	 * 
	 * Returns
	 * - The range of this item
	 */
	public double GetRange() {
		return Range;
	}

	/**
	 * Reset Cool Down
	 */
	public void ResetCoolDown() {
		CoolDown = 0;
	}
	
	/**
	 * Reduce the amount of turns before this item can be used again.
	 * Simply decrement.
	 */
	public void ReduceCoolDown() {
		if (CoolDown != 0) CoolDown--;
		Debug.Log(ItemName + " Cool Down Remaining: " + CoolDown);
	}

	/**
	 * Get the amount of turns left before this item can be used again
	 * 
	 * Returns
	 * - Remaining Cool Down Turns
	 */
	public int RemainingCoolDownTurns() {
		return CoolDown;
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

	/**
	 * Use this function when instantiating this object, which, 
	 * apparently, bypasses the Start() function
	 */
	public virtual void StartAfterInstantiate() {
		Debug.Log("Item: " + ItemName + " started after instantiation");
	}

	/**
	 * Function used to activate the item. 
	 * Overridable by subclasses
	 * 
	 * Arguments
	 * - Tile targetTile - The target tile 
	 */
	public virtual void Activate(Tile targetTile) {
		Debug.Log(ItemName + " attempting to be activated");
	}

	/**
	 * Return if this item is activatable
	 * 
	 * Returns
	 * - true if this item is activatable
	 * - false if otherwise
	 */
	public bool IsActivatable() {
		return Activatable;
	}

}
