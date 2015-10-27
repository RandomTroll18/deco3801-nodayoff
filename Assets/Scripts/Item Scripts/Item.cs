using UnityEngine;

/**
 * This is an abstract class which should be inherited 
 * by all items in the game.
 */
using System.Collections.Generic;


public abstract class Item : MonoBehaviour {

	public string ItemName; // The name of this item
	public Sprite Image; // The icon for this image
	public List<AudioClip> ActivateEfx; // The sound effects for this item

	protected string ItemDescription; // The description of this item
	protected List<Effect> Effects; // The effects in this item
	protected int CoolDown; // Cool down for activation. This is in terms of turns
	protected int CoolDownSetting; // The amount of turns that this item should be cooling down for
	protected int DefaultCoolDownSetting; // The default cooldown setting
	protected int UsePerTurn = 1; // The number of uses for this item per turn
	protected int DefaultUsePerTurn = 1; // The default number of uses per turn
	protected int CurrentNumberOfUses; // The current number of uses
	protected double Range = 1; // The range of this item's activation action
	protected ActivationType ItemActivationType; // Activation Type of this item
	protected RangeType ItemRangeType; // The range type of this item
	protected bool Activatable; // Record if this item can be activated
	protected bool Droppable = true; // Record if this item can be dropped
	
	/*
	 * The effects in this item.
	 * 
	 * Concerning the array of doubles:
	 * double[0] => to be added (e.g. Stat.AP += double[0])
	 * double[1] => to be set (e.g. Stat.AP = double[1])
	 * double[2] => to be multiplied (e.g. Stat.AP *= double[2])
	 */
	protected Dictionary<Stat, double[]> InstantEffects;

	/**
	 * Set the current number of uses for this item
	 * 
	 * Arguments
	 * - int newCurrentNumberOfUses - The new number of uses for this item
	 */
	public void SetCurrentNumberOfUses(int newCurrentNumberOfUses) {
		CurrentNumberOfUses = newCurrentNumberOfUses;
	}

	/**
	 * Reset the number of uses per turn
	 */
	public void ResetUsePerTurn() {
		UsePerTurn = DefaultUsePerTurn;
	}

	/**
	 * Set the number of uses per turn
	 * 
	 * Arguments
	 * - int newUsePerTurn - The new number of uses per turn
	 */
	public void SetUsePerTurn(int newUsePerTurn) {
		UsePerTurn = newUsePerTurn;
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
	public virtual void ReduceCoolDown() {
		if (CoolDown != 0) 
			CoolDown--;
		if (CoolDown == 0) 
			CurrentNumberOfUses = UsePerTurn;
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
	 * Set cool down setting
	 * 
	 * Arguments
	 * - int newCoolDownSetting - The new cooldown setting
	 */
	public void SetCoolDownSetting(int newCoolDownSetting) {
		CoolDownSetting = newCoolDownSetting;
	}

	/**
	 * Reset cool down setting
	 */
	public void ResetCoolDownSetting() {
		CoolDownSetting = DefaultCoolDownSetting;
	}
	
	/**
	 * Get the turn effects in this item
	 * 
	 * Returns
	 * - A list of turn effects
	 */
	public List<Effect> GetTurnEffects() {
		return Effects;
	}

	/**
	 * Get the attached non-turn effects in this item.
	 * 
	 * Returns
	 * - A dictionary of effects
	 */
	public Dictionary<Stat, double[]> GetEffects() {
		return InstantEffects;
	}

	/**
	 * Use this function when instantiating this object, which, 
	 * apparently, bypasses the Start() function
	 */
	public abstract void StartAfterInstantiate();

	/**
	 * Function used to activate the item. 
	 * Overridable by subclasses
	 * 
	 * Arguments
	 * - Tile targetTile - The target tile 
	 */
	public abstract void Activate(Tile targetTile);

	/**
	 * Activate the item. Has no target
	 */
	public abstract void Activate();

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

	/**
	 * Return if this item is droppable
	 * 
	 * Returns
	 * - true if item is droppable. false otherwise
	 */
	public bool IsDroppable() {
		return Droppable;
	}

	/**
	 * Make this item appear/disappear
	 * 
	 * Arguments
	 * - bool appearFlag - set to true if item is to appear. false 
	 * otherwise
	 */
	[PunRPC]
	public void SetActive(bool appearFlag) {
		Debug.Log("RPC Call: Item Set Active: " + appearFlag);
		gameObject.SetActive(appearFlag);
	}

	/**
	 * Set position of this item
	 * 
	 * Arguments
	 * - float x - The x coordinate
	 * - float z - The z coordinate
	 */
	[PunRPC]
	public void SetPosition(float x, float z) {
		Debug.Log("RPC Call: Item Set Position To: " + x + ", " + z);
		transform.position = new Vector3(x, 0f, z);
	}
	
}
