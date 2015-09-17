using UnityEngine;
using System.Collections.Generic;

public class ScoutClass : PlayerClass {

	/**
	 * Constructor
	 */
	public ScoutClass() {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 10.0;
		DefaultStats[Stat.VISION] = 5.0;
		
		Discount = new Dictionary<ActionCost, double>();
		Discount[ActionCost.DOORS] = 1.0;
		Discount[ActionCost.MOVEMENT] = 0.5;
		Discount[ActionCost.REPAIR] = 1.0;
		Discount[ActionCost.STRENGTH] = 0.5;
		Discount[ActionCost.TECH] = 1.0;
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Scout Class";
	}
}
