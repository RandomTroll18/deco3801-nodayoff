using UnityEngine;
using System.Collections.Generic;

/**
 * Super class that all player classes (e.g. Technician) should 
 * inherit from/extend
 */
public abstract class PlayerClass {

	protected Dictionary<Stat, double> defaultStats; // Default stats
	
	/**
	 * Return the default AP for this class
	 * 
	 * Returns
	 * - The default AP
	 */
	public double GetDefaultAP() {
		return defaultStats[Stat.AP];
	}

	/**
	 * Return the default vision value for this class
	 * 
	 * Returns
	 * - The default vision value
	 */
	public double GetDefaultVision() {
		return defaultStats[Stat.VISION];
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public abstract string GetPlayerClassType();
}
