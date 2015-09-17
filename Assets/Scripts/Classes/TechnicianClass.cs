using UnityEngine;
using System.Collections.Generic;

public class TechnicianClass : PlayerClass {

	/**
	 * Constructor
	 */
	public TechnicianClass() {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 10.0;
		DefaultStats[Stat.VISION] = 5.0;
		
		Discount = new Dictionary<ActionCost, double>();
		Discount[ActionCost.DOORS] = 1.0;
		Discount[ActionCost.MOVEMENT] = 1.0;
		Discount[ActionCost.REPAIR] = 1.0;
		Discount[ActionCost.STRENGTH] = 1.0;
		Discount[ActionCost.TECH] = 0.5;
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Technician Class";
	}
}
