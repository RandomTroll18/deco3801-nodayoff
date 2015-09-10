using UnityEngine;
using System.Collections.Generic;

public class EngineerClass : PlayerClass {
	
	/**
	 * Constructor
	 */
	public EngineerClass() {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 10.0; // Default AP is 10
		DefaultStats[Stat.VISION] = 5.0; // Vision of base class is a range of 5 units
		
		Discount = new Dictionary<ActionCost, double>();
		Discount[ActionCost.DOORS] = 0.5;
		Discount[ActionCost.MOVEMENT] = 1.0;
		Discount[ActionCost.REPAIR] = 0.5;
		Discount[ActionCost.STRENGTH] = 1.0;
		Discount[ActionCost.TECH] = 1.0;
		
		PrimaryAbility = new EngineerPrimaryAbility(); // This class has no primary ability
	}


	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Engineer Class";
	}
}
