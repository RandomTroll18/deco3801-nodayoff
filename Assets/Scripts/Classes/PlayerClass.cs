using UnityEngine;
using System.Collections.Generic;

/**
 * Super class that all player classes (e.g. Technician) should 
 * inherit from/extend
 */
public abstract class PlayerClass {

	protected Dictionary<Stat, double> DefaultStats; // Default stats
	protected Dictionary<Stat, double> Discount; // Discounts

	/**
	 * Return the discount for Vision
	 * 
	 * Returns
	 * - The discount multiplier for vision
	public double GetVisionDiscount() {
		return Discount[Stat.VISION];
	}

	/**
	 * Return the discount for AP
	 * 
	 * Returns
	 * -  The discount multiplier for AP
	 */
	public double GetAPDiscount() {
		return Discount[Stat.AP];
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
