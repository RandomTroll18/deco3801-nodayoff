﻿using UnityEngine;
using System.Collections;

public class StatusTurnEffect : Effect {

	protected Stat StatAffected; // The stat affected
	/*
	 * How this turn effect is to be applied
	 * - 0 => to be added (e.g. Stat.AP += value)
	 * - 1 => to be set (e.g. Stat.AP = value)
	 * - 2 => to be multiplied (e.g. Stat.AP *= value);
	 */
	protected int StatMode;
	/*
	 * The value that the stat is affected by.
	 * The value can be negative
	 */
	protected double StatEffectValue;
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
	 * - bool applyPerTurnFlag - set this effect to be applied per turn or not
	 */
	public StatusTurnEffect(Stat newStatAffected, double newValue, int newMode, 
	                  string newDescription, string iconPath, int turnsActive, bool applyPerTurnFlag) {
		SetBasicValues(newDescription, iconPath, turnsActive, applyPerTurnFlag);
		StatAffected = newStatAffected;
		StatEffectValue = newValue;
		StatMode = newMode;
		Type = TurnEffectType.STATEFFECT;
	}

	/**
	 * Override toString function
	 * 
	 * Returns
	 * - The string form of this class
	 */
	public override string ToString()
	{
		string toReturn = base.ToString();
		toReturn += "Stat Affected: " + EnumsToString.ConvertStatEnum(StatAffected) + StringMethodsScript.NEWLINE;
		toReturn += "Stat Mode: " + StatMode + StringMethodsScript.NEWLINE;
		toReturn += "Value of effect: " + StatEffectValue + StringMethodsScript.NEWLINE;
		return toReturn;
	}

	/**
	 * Sets the mode of this turn effect
	 * 
	 * Arguments
	 * - int newMode - The new mode for this turn effect
	 */
	public override void SetMode(int newMode) {
		switch (newMode) { // Apparently we are not allowed to fall through cases :(
		case 2: goto case 0;
		case 1: goto case 0;
		case 0: // Valid mode
			StatMode = newMode;
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
	public override int GetMode() {
		return StatMode;
	}

	/**
	 * Get the stat affected by this turn effect
	 * 
	 * Returns
	 * - The stat this effect is affecting
	 */
	public override Stat GetStatAffected() {
		return StatAffected;
	}

	/**
	 * Get the value by which the stat is to be affected
	 * 
	 * Returns
	 * - The value that the stat will be affected by
	 */
	public override double GetValue() {	
		return StatEffectValue;
	}

	/**
	 * Sets the stat affected
	 * 
	 * Arguments
	 * - Stat newStat - The stat being affected
	 */
	public override void SetStatAffected(Stat newStat) {
		StatAffected = newStat;
	}
	
	/**
	 * Sets the value by which the stat is affected by
	 * 
	 * Arguments
	 * - double newValue - the value by which the stat is affected
	 */
	public override void SetValue(double newValue) {
		StatEffectValue = newValue;
	}

	/* Override abstruct stuff so that compiler doesn't whine */
	public override Material GetMaterial()
	{
		throw new System.NotImplementedException();
	}

	public override System.Type GetAffectedItemType()
	{
		throw new System.NotImplementedException();
	}

	public override void ApplyEffectToItem(Item item)
	{
		throw new System.NotImplementedException();
	}
}
