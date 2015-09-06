
using UnityEngine;

public class TurnEffect {


	Sprite icon; // The icon for this turn effect
	Stat statAffected; // The stat affected
	string description; // Description of turn effect
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

	/**
	 * The constructor
	 * 
	 * Arguments
	 * - Stat statAffected - the stat to be affected
	 * - double value - The value that the stat will be affected by
	 * - int mode - The way the stat would be applied
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon (with the Resources folder as the root)
	 */
	public TurnEffect(Stat newStatAffected, double newValue, int newMode, 
			string newDescription, string iconPath) {
		statAffected = newStatAffected;
		value = newValue;
		mode = newMode;
		description = newDescription;
		icon  = Resources.Load<Sprite>(iconPath);
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
