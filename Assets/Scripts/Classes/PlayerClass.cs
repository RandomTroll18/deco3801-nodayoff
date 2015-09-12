using UnityEngine;
using System.Collections.Generic;

/**
 * Super class that all player classes (e.g. Technician) should 
 * inherit from/extend
 */
public abstract class PlayerClass {

	protected Dictionary<Stat, double> DefaultStats; // Default stats
	protected Dictionary<ActionCost, double> Discount; // Discounts
	protected Ability PrimaryAbility; // This class' primary ability

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
	 * Return the discount for particular action costs
	 * 
	 * Arguments
	 * - ActionCost actionCost - The action cost discount requested
	 * 
	 * Returns
	 * - The multiplier for the requested action cost
	 */
	public double GetActionDiscount(ActionCost actionCost) {
		return Discount[actionCost];
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
	public double GetDefaultStat(Stat playerStat) {
		return DefaultStats[playerStat];
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public abstract string GetPlayerClassType();
}
