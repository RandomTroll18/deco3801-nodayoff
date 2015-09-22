using UnityEngine;
using System.Collections.Generic;

public class TurnEffect {

	Sprite icon; // The icon for this turn effect
	Stat statAffected; // The stat affected
	string description; // Description of turn effect
	Material material; // The material to be set from this effect
	/*
	 * How this turn effect is to be applied
	 * - 0 => to be added (e.g. Stat.AP += value)
	 * - 1 => to be set (e.g. Stat.AP = value)
	 * - 2 => to be multiplied (e.g. Stat.AP *= value);
	 */
	int mode;
	/*
	 * The value that the stat is affected by.
	 * The value can be negative
	 */
	double value;
	int turns; // The number of turns left for this turn effect
	TurnEffectType type; // This turn effect's type

	/**
	 * Set basic values. For constructor use only
	 * 
	 * Arguments
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon of this turn effect
	 * - int turnsActive - The number of turns for this turn effect
	 * - TurnEffectType newType - The type of this turn effect
	 */
	void setBasicValues(string newDescription, string iconPath, int turnsActive, TurnEffectType newType) {
		description = newDescription;
		icon = Resources.Load<Sprite>(iconPath);
		turns = turnsActive;
		type = newType;
	}

	/**
	 * The constructor for a stat effect
	 * 
	 * Arguments
	 * - Stat statAffected - the stat to be affected
	 * - double value - The value that the stat will be affected by
	 * - int mode - The way the stat would be applied
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon (with the Resources folder as the root)
	 * - int turnsActive - Number of turns active
	 */
	public TurnEffect(Stat newStatAffected, double newValue, int newMode, 
	                  string newDescription, string iconPath, int turnsActive, TurnEffectType newType) {
		setBasicValues(newDescription, iconPath, turnsActive, newType);
		statAffected = newStatAffected;
		value = newValue;
		mode = newMode;
		Debug.Log("Stat turn effect constructed");
		Debug.Log("Value: " + newValue);
		Debug.Log("Mode: " + newMode);
		Debug.Log("Desc: " + newDescription);
		Debug.Log("Turns Active: " + turnsActive);
	}

	/**
	 * The constructor for a material effect
	 * 
	 * Arguments
	 * - string materialPath - The path to the material of this effect
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon (with the Resources folder as the root)
	 * - int turnsActive - Number of turns active
	 * - TurnEffectType newType - The type of this effect
	 */
	public TurnEffect(string materialPath, string newDescription, string iconPath, int turnsActive, 
			TurnEffectType newType) {
		setBasicValues(newDescription, iconPath, turnsActive, newType);
		material = Resources.Load<Material>(materialPath);
		Debug.Log("Material turn effect constructed");
	}

	/**
	 * Get the material in this turn effect
	 * 
	 * Returns
	 * - The material attached to this turn effect
	 */
	public Material GetMaterial() {
		return material;
	}

	
	/**
	 * Get the type of this turn effect
	 * 
	 * Returns
	 * - The type of this turn effect
	 */
	public TurnEffectType GetTurnEffectType() {
		return type;
	}

	/**
	 * Reduce the number of turns left for this turn effects
	 */
	public void ReduceTurnsRemaining() {
		if (turns > 0) turns--;
	}

	/**
	 * The number of turns left that this turn effect will be active
	 * 
	 * Returns
	 * - The number of turns left for this effect
	 */
	public int TurnsRemaining() {
		return turns;
	}

	/**
	 * Get the icon of this turn effect
	 * 
	 * Return
	 * - The icon attached to this turn effect
	 */
	public Sprite GetIcon() {
		return icon;
	}

	/**
	 * Sets the mode of this turn effect
	 * 
	 * Arguments
	 * - int newMode - The new mode for this turn effect
	 */
	public void SetMode(int newMode) {
		switch (newMode) { // Apparently we are not allowed to fall through cases :(
		case 2: goto case 0;
		case 1: goto case 0;
		case 0: // Valid mode
			mode = newMode;
			goto default;
		default: // Invalid modes
			break;
		}
	}

	/**
	 * Get the mode of this turn effect
	 * 
	 * Returns
	 * - The mode
	 */
	public int GetMode() {
		return mode;
	}

	/**
	 * Get the stat affected by this turn effect
	 * 
	 * Returns
	 * - The stat this effect is affecting
	 */
	public Stat GetStatAffected() {
		return statAffected;
	}

	/**
	 * Get the value by which the stat is to be affected
	 * 
	 * Returns
	 * - The value that the stat will be affected by
	 */
	public double GetValue() {	
		return value;
	}

	/**
	 * Sets the stat affected
	 * 
	 * Arguments
	 * - Stat newStat - The stat being affected
	 */
	public void SetStatAffected(Stat newStat) {
		statAffected = newStat;
	}

	/**
	 * Sets the value by which the stat is affected by
	 * 
	 * Arguments
	 * - double newValue - the value by which the stat is affected
	 */
	public void SetValue(double newValue) {
		value = newValue;
	}

	/**
	 * Sets the description of this turn effect
	 * 
	 * Arguments
	 * - string newDescription - The new description of this turn effect
	 */
	public void SetDescription(string newDescription) {
		description = newDescription;
	}

	/**
	 * Gets the description of this turn effect
	 * 
	 * Returns 
	 * - The description of this turn effect
	 */
	public string GetDescription() {
		return description;
	}

	/**
	 * Tostring function
	 * 
	 * Return
	 * - The string form of this function
	 */
	public override string ToString() {
		string finalString = "Turn Effect: " + StringMethodsScript.NEWLINE;
		finalString += "Stat: " + EnumsToString.ConvertStatEnum(statAffected) + StringMethodsScript.NEWLINE;
		finalString += "Value: " + value + StringMethodsScript.NEWLINE;
		finalString += "Description: " + description;
		return finalString;
	}
}