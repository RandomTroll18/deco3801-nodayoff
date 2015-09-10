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

		Discount = new Dictionary<ActionCost, double>();
		Discount[ActionCost.DOORS] = 1.0;
		Discount[ActionCost.MOVEMENT] = 1.0;
		Discount[ActionCost.REPAIR] = 1.0;
		Discount[ActionCost.STRENGTH] = 1.0;
		Discount[ActionCost.TECH] = 1.0;

		PrimaryAbility = null; // This class has no primary ability
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
