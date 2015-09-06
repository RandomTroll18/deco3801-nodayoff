using UnityEngine;
using System.Collections;

/**
 * Super class that all player classes (e.g. Technician) should 
 * inherit from/extend
 */
public abstract class PlayerClass {

	protected double DefaultAP; // Default AP for this class
	protected double DefaultStun; // Default Stun Value
	protected double DefaultVision; // Default Vision

	/**
	 * Return the default AP for this class
	 * 
	 * Returns
	 * - The default AP
	 */
	public double GetDefaultAP() {
		return DefaultAP;
	}

	/**
	 * Return the default value for the stun flag for this class
	 * 
	 * Returns
	 * - The default stun flag
	 */
	public double GetDefaultStun() {
		return DefaultStun;
	}

	/**
	 * Return the default vision value for this class
	 * 
	 * Returns
	 * - The default vision value
	 */
	public double GetDefaultVision() {
		return DefaultVision;
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public abstract string GetPlayerClassType();
}
