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
	 * Return the discount for strength action costs
	 * 
	 * Returns
	 * - The multiplier for strength action costs
	 */
	public double GetStrengthActionDiscount() {
		return Discount[ActionCost.STRENGTH];
	}

	/**
	 * Return the discount for tech action costs
	 * 
	 * Returns
	 * - The multiplier for tech action costs
	 */
	public double GetTechActionDiscount() {
		return Discount[ActionCost.TECH];
	}

	/**
	 * Return the discount for movement action costs
	 * 
	 * Returns
	 * - The multiplier for movement action costs
	 */
	public double GetMovementActionDiscount() {
		return Discount[ActionCost.MOVEMENT];
	}

	/**
	 * Return the discount for repair action costs
	 * 
	 * Returns
	 * - The multiplier for repair action costs
	 */
	public double GetRepairActionDiscount() {
		return Discount[ActionCost.REPAIR];
	}

	/**
	 * Return the discount for door action costs
	 * 
	 * Returns
	 * - The multiplier for door action costs
	 */
	public double GetDoorActionDiscount() {
		return Discount[ActionCost.DOORS];
	}

	/**
	 * Return the default AP for this class
	 * 
	 * Returns
	 * - The default AP
	 */
	public double GetDefaultAP() {
		return DefaultStats[Stat.AP];
	}

	/**
	 * Return the default vision value for this class
	 * 
	 * Returns
	 * - The default vision value
	 */
	public double GetDefaultVision() {
		return DefaultStats[Stat.VISION];
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public abstract string GetPlayerClassType();
}
