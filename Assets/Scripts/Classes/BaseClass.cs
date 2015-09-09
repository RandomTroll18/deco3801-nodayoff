using UnityEngine;
using System.Collections.Generic;

public class BaseClass : PlayerClass {

	/**
	 * Constructor
	 */
	public BaseClass() {
		defaultStats = new Dictionary<Stat, double>();
		defaultStats[Stat.AP] = 10.0; // Default AP is 10
		defaultStats[Stat.VISION] = 5.0; // Vision of base class is a range of 5 units
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Base Class";
	}
}
