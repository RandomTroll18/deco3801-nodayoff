using UnityEngine;
using System.Collections.Generic;

public class BaseClass : PlayerClass {

	/**
	 * Constructor
	 */
	public BaseClass() {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 10.0; // Default AP is 10
		DefaultStats[Stat.VISION] = 5.0; // Vision of base class is a range of 5 units
		Discount = new Dictionary<Stat, double>();
		Discount[Stat.AP] = 1.0;
		Discount[Stat.VISION] = 1.0;
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
