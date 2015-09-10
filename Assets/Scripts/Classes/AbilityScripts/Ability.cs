using UnityEngine;
using System.Collections;

/**
 * Abstract Class for an ability
 */
public abstract class Ability {

	protected string AbilityName; // The name of this ability
	protected double Range; // The range of this ability
	protected int RemainingTurns = 3; // The number of turns left for this ability
	protected bool IsActive = false; // Record if this ability is active
	protected RangeType AbilityRangeType; // The range type of this ability
	protected ActivationType AbilityActivationType; // The activation type of this ability

	/**
	 * Get the activation type of this ability
	 * 
	 * Returns
	 * - The activation type of this ability
	 */
	public ActivationType GetActivationType() {
		return AbilityActivationType;
	}

	/**
	 * Get the range type of this ability
	 * 
	 * Returns
	 * - The range type of this ability
	 */
	public RangeType GetRangeType() {
		return AbilityRangeType;
	}

	/**
	 * Check if this ability is still active
	 * 
	 * Returns
	 * - True if this ability is still active. False if otherwise
	 */
	public bool AbilityIsActive() {
		return IsActive;
	}

	/**
	 * Reduce the number of turns that this ability will be active for
	 */
	public virtual void ReduceNumberOfTurns() {
		if (RemainingTurns != 0 && IsActive) RemainingTurns--;
	}

	/**
	 * Get the remaining number of turns that this ability will be active for
	 * 
	 * Returns
	 * - The number of turns left before this ability becomes inactive
	 */
	public int RemainingNumberOfTurns() {
		return RemainingTurns;
	}

	/**
	 * Get the range of this ability
	 * 
	 * Returns
	 * - The range of this ability (as a double)
	 */
	public double GetRange() {
		return Range;
	}

	/**
	 * Get the name of this ability
	 * 
	 * Returns
	 * - The string form of this ability's name
	 */
	public string GetAbilityName() {
		return AbilityName;
	}

	/**
	 * Activate this ability
	 * 
	 * Arguments
	 * - Tile targetTile - The targetted tile
	 */
	public virtual void Activate(Tile targetTile) {
		IsActive = true; // This ability is not active
		Debug.Log("Ability is now active");
	}
}
