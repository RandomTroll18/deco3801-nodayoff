using UnityEngine;
using System.Collections.Generic;

public class AlienClass : PlayerClass {

	/**
	 * Constructor for Alien Class
	 */
	public AlienClass() {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 20.0;
		DefaultStats[Stat.VISION] = 7.0;

		Discount = new Dictionary<ActionCost, double>();
		Discount[ActionCost.DOORS] = 0.8;
		Discount[ActionCost.MOVEMENT] = 0.8;
		Discount[ActionCost.REPAIR] = 0.8;
		Discount[ActionCost.STRENGTH] = 0.8;
		Discount[ActionCost.TECH] = 0.8;
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Alien Class";
	}
}
