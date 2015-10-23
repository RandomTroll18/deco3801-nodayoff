using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Abstract Class for an ability
 */
public abstract class Ability {

	protected GameObject ClassPanel; // The class panel for this ability
	protected GameObject SpawnAPCounterPanel; // The AP counter for a spawned object
	protected Text SpawnAPCounterText; // AP Counter text for a spawned object
	protected string AbilityName; // The name of this ability
	protected double Range; // The range of this ability
	protected int RemainingTurns = 3; // The number of turns left for this ability
	protected bool IsActive = false; // Record if this ability is active
	protected RangeType AbilityRangeType; // The range type of this ability
	protected ActivationType AbilityActivationType; // The activation type of this ability

	/**
	 * Set the Spawn AP Counter and text for a spawned object
	 * 
	 * Arguments
	 * - GameObject newSpawnAPCounterPanel - The new Spawn AP Counter Panel
	 * - Text newSpawnAPCounterText - The new Spawn AP Counter Text
	 */
	public void SetSpawnAPCounterPanel(GameObject newSpawnAPCounterPanel, Text newSpawnAPCounterText) {
		SpawnAPCounterPanel = newSpawnAPCounterPanel;
		SpawnAPCounterText = newSpawnAPCounterText;
	}

	/**
	 * Set the class panel of this ability
	 * 
	 * Arguments
	 * - GameObject newClassPanel - The new class panel to attach this ability to
	 */
	public void SetClassPanel(GameObject newClassPanel) {
		ClassPanel = newClassPanel;
	}

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
		if (RemainingTurns != 0 && IsActive) 
			RemainingTurns--;
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
	 * Activate this ability (with a targetted tile)
	 * 
	 * Arguments
	 * - Tile targetTile - The targetted tile
	 */
	public virtual void Activate(Tile targetTile) {
		IsActive = true; // This ability is now active
	}

	/**
	 * Activate this ability (with no target)
	 */
	public virtual void Activate() {
		IsActive = true;
	}

	/**
	 * Deactivate this ability
	 */
	public virtual void Deactivate() {
		IsActive = false;
	}

	/**
	 * Extra initialization. This should be extended if you need any initializations that 
	 * can't be done on Start()
	 */
	public virtual void ExtraInitializing() {
		return;
	}
}
