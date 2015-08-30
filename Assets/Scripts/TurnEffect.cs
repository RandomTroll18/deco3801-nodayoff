using UnityEngine;
using System.Collections;

public class TurnEffect {

	private Stat statAffected; // The stat affected
	/*
	 * The value that the stat is affected by.
	 * The value can be negative
	 */
	private double value;

	/**
	 * The constructor
	 * 
	 * Arguments
	 * - Stat statAffected - the stat to be affected
	 * - double value - The value that the stat will be affected by
	 */
	public TurnEffect (Stat statAffected, double value) {
		this.statAffected = statAffected;
		this.value = value;
	}

	/**
	 * Get the stat affected by this turn effect
	 * 
	 * Returns
	 * - The stat this effect is affecting
	 */
	public int getStatAffected () {
		return (int)this.statAffected;
	}

	/**
	 * Get the value by which the stat is to be affected
	 * 
	 * Returns
	 * - The value that the stat will be affected by
	 */
	public double getValue () {
		return this.value;
	}

	/**
	 * Sets the stat affected
	 * 
	 * Arguments
	 * - Stat newStat - The stat being affected
	 */
	public void setStatAffected (Stat newStat) {
		this.statAffected = newStat;
	}

	/**
	 * Sets the value by which the stat is affected by
	 * 
	 * Arguments
	 * - double newValue - the value by which the stat is affected
	 */
	public void setValue (double newValue) {
		this.value = newValue;
	}

	/**
	 * Tostring function
	 * 
	 * Return
	 * - The string form of this function
	 */
	public override string ToString ()
	{
		string finalString = "Turn Effect: " + StringMethodsScript.NEWLINE;
		finalString += 
				"Stat: " + EnumsToString.convertStatEnum(this.statAffected)
				+ StringMethodsScript.NEWLINE;
		finalString += "Value: " + this.value;
		return finalString;
	}
}
