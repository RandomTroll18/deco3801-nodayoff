using UnityEngine;
using System.Collections.Generic;

/**
 * Super class that all player classes (e.g. Technician) should 
 * inherit from/extend
 */
public abstract class PlayerClass {

	protected Dictionary<Stat, double> DefaultStats; // Default stats
	protected Dictionary<Stat, double> Stats; // Actual stats
	protected Ability PrimaryAbility; // This class' primary ability
	protected Classes ClassTypeEnum; // The enum representing this class' type

	/**
	 * Return the primary ability of this class
	 * 
	 * Returns
	 * - The primary ability of this class
	 */
	public Ability GetPrimaryAbility() {
		return PrimaryAbility;
	}

	/**
	 * Return the default value for a particular stat for this class
	 * 
	 * Arguments
	 * - Stat playerStat - The stat requested
	 * 
	 * Returns
	 * - The default value for the given player stat. NULL if not set
	 */
	public double GetStat(Stat playerStat) {
		return Stats[playerStat];
	}

	/**
	 * Restore the default stat multiplier
	 *
	 * Arguments
	 * - Stat statMultiplier - The stat multiplier to reset
	 */
	public void RestoreDefaultStat(Stat statMultiplier) {
		double valueToSet = DefaultStats[statMultiplier];
		SetMultiplierStat(statMultiplier, valueToSet);
	}

	/**
	 * Restore default stats
	 */
	public void RestoreDefaultStats() {
		Stats[Stat.AP] = DefaultStats[Stat.AP];
		Stats[Stat.VISION] = DefaultStats[Stat.VISION];
		Stats[Stat.ENGMULTIPLIER] = DefaultStats[Stat.ENGMULTIPLIER];
		Stats[Stat.MARINEMULTIPLIER] = DefaultStats[Stat.MARINEMULTIPLIER];
		Stats[Stat.SCOUTMULTIPLIER] = DefaultStats[Stat.SCOUTMULTIPLIER];
		Stats[Stat.TECHMULTIPLIER] = DefaultStats[Stat.TECHMULTIPLIER];
	}

	/**
	 * Set the value for a given multiplier
	 * 
	 * Arguments
	 * - Stat multiplier - The multiplier to modify
	 * - double newValue - The new value
	 */
	public void SetMultiplierStat(Stat multiplier, double newValue) {
		switch (multiplier) {
		case Stat.AP: goto default; /* We don't handle non-multiplier stats */
		case Stat.VISION: goto default; /* We don't handle non-multiplier stats */
		case Stat.ENGMULTIPLIER: goto case Stat.TECHMULTIPLIER;
		case Stat.MARINEMULTIPLIER: goto case Stat.TECHMULTIPLIER;
		case Stat.SCOUTMULTIPLIER: goto case Stat.TECHMULTIPLIER;
		case Stat.TECHMULTIPLIER: /* Valid stat multipliers */
			Stats[multiplier] = newValue;
			break;
		default:
			throw new System.NotSupportedException("Trying to set a stat that isn't a multiplier");
		}
	}

	/**
	 * Increase the stat multiplier with the given stat
	 * 
	 * Arguments
	 * - Stat multiplier - The multiplier stat to modify
	 * - double newValue - The new value
	 */
	public void IncreaseStatMultiplierValue(Stat multiplier, double newValue) {
		double valueToSet = DefaultStats[multiplier] + newValue;
		SetMultiplierStat(multiplier, valueToSet);
	}

	/**
	 * Decrease the stat multiplier with the given stat
	 * 
	 * Arguments
	 * - Stat multiplier - The multiplier stat to modify
	 * - double newValue - The new value
	 */
	public void DecreaseStatMultiplierValue(Stat multiplier, double newValue) {
		double valueToSet = DefaultStats[multiplier] - newValue;
		SetMultiplierStat(multiplier, valueToSet);
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public abstract string GetPlayerClassType();

	/**
	 * Return the type of this class as an enum
	 * 
	 * Returns
	 * - A Classes Enum representing the type of this class
	 */
	public Classes GetClassTypeEnum() {
		return ClassTypeEnum;
	}
}
