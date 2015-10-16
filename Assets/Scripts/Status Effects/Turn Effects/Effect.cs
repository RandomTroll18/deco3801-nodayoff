using UnityEngine;
using System.Collections.Generic;

public abstract class Effect {

	/* Delegates to call whenever the effect's remaining number of turns is reduced */
	public delegate void TurnModifications(); // Delegate for turn modifications to do 
	public TurnModifications TurnModificationDelegates = null; // The function pointers

	protected Sprite Icon; // The icon for this turn effect
	protected string Description; // Description of turn effect
	protected int Turns; // The number of turns left for this turn effect
	protected TurnEffectType Type; // This turn effect's type
	protected bool ApplyPerTurn; // Record if this effect is to be applied per turn

	/**
	 * Calculate the amount of AP to give to the alien on activation of this ability
	 * 
	 * Returns
	 * - The amount of AP to give to the alien in this turn
	 */
	public static double CalculateAP() {
		int currentNumberOfTurns; // The current number of turns
		int initialNumberOfTurns; // The initial number of turns
		
		initialNumberOfTurns = Object.FindObjectOfType<GameManager>().InitialNumberOfTurns;
		currentNumberOfTurns = Object.FindObjectOfType<GameManager>().RoundsLeftUntilLose;
		
		return (2.0 + (double)(initialNumberOfTurns - currentNumberOfTurns));
	}

	/**
	 * Set basic values. For constructor use only
	 * 
	 * Arguments
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon of this turn effect
	 * - int turnsActive - The number of turns for this turn effect
	 * - bool applyPerTurnFlag - Set if this effect should be applied per turn
	 */
	protected void SetBasicValues(string newDescription, string iconPath, int turnsActive, bool applyPerTurnFlag) {
		Description = newDescription;
		Icon = Resources.Load<Sprite>(iconPath);
		Turns = turnsActive;
		ApplyPerTurn = applyPerTurnFlag;
	}

	/**
	 * Return whether or not this effect should be applied per turn
	 * 
	 * Return
	 * - true if this effect should be applied per turn
	 * - false if otherwise
	 */
	public bool IsAppliedPerTurn() {
		return ApplyPerTurn;
	}

	/**
	 * Additional attaching routines. For more complex effects
	 */
	public virtual void ExtraAttachActions() {
		return;
	}

	/**
	 * Additional detaching routines. For more complex effects
	 */
	public virtual void ExtraDetachActions() {
		return;
	}

	/**
	 * Get the icon of this turn effect
	 * 
	 * Return
	 * - The icon attached to this turn effect
	 */
	public Sprite GetIcon() {
		return Icon;
	}

	/**
	 * Get the type of this turn effect
	 * 
	 * Returns
	 * - The type of this turn effect
	 */
	public TurnEffectType GetTurnEffectType() {
		return Type;
	}

	/**
	 * Reduce the number of turns left for this turn effects
	 */
	public void ReduceTurnsRemaining() {
		if (Turns > 0) Turns--;
		if (TurnModificationDelegates != null) // Apply turn modifications
			TurnModificationDelegates();
	}

	/**
	 * The number of turns left that this turn effect will be active
	 * 
	 * Returns
	 * - The number of turns left for this effect
	 */
	public int TurnsRemaining() {
		return Turns;
	}



	/**
	 * Sets the description of this turn effect
	 * 
	 * Arguments
	 * - string newDescription - The new description of this turn effect
	 */
	public void SetDescription(string newDescription) {
		Description = newDescription;
	}

	/**
	 * Gets the description of this turn effect
	 * 
	 * Returns 
	 * - The description of this turn effect
	 */
	public string GetDescription() {
		return Description;
	}

	/**
	 * Tostring function
	 * 
	 * Return
	 * - The string form of this function
	 */
	public override string ToString() {
		string finalString = "Turn Effect: " + StringMethodsScript.NEWLINE;
		finalString += "Description: " + Description + StringMethodsScript.NEWLINE;
		return finalString;
	}

	/* Abstract functions to implement */

	/**
	 * Get the material in this turn effect
	 * 
	 * Returns
	 * - The material attached to this turn effect
	 */
	public abstract Material GetMaterial();

	/**
	 * Sets the mode of this turn effect
	 * 
	 * Arguments
	 * - int newMode - The new mode for this turn effect
	 */
	public abstract void SetMode(int newMode);
	
	/**
	 * Get the mode of this turn effect
	 * 
	 * Returns
	 * - The mode
	 */
	public abstract int GetMode();
	
	/**
	 * Get the stat affected by this turn effect
	 * 
	 * Returns
	 * - The stat this effect is affecting
	 */
	public abstract Stat GetStatAffected();
	
	/**
	 * Get the value by which the stat is to be affected
	 * 
	 * Returns
	 * - The value that the stat will be affected by
	 */
	public abstract double GetValue();
	
	/**
	 * Sets the stat affected
	 * 
	 * Arguments
	 * - Stat newStat - The stat being affected
	 */
	public abstract void SetStatAffected(Stat newStat);
	
	/**
	 * Sets the value by which the stat is affected by
	 * 
	 * Arguments
	 * - double newValue - the value by which the stat is affected
	 */
	public abstract void SetValue(double newValue);

	/**
	 * Get the type of item this turn effect affects
	 * 
	 * Returns
	 * - The type of item this effect affects
	 */
	public abstract System.Type GetAffectedItemType();

	/**
	 * Apply effect to given item
	 * 
	 * Arguments
	 * - Item item - The item to affect
	 */
	public abstract void ApplyEffectToItem(Item item);
	
}