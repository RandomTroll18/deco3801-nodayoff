using UnityEngine;
using System.Collections.Generic;

public class BaseClass : PlayerClass {

	/**
	 * Constructor
	 */
	public BaseClass() {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 20.0; // Default AP is 20
		DefaultStats[Stat.VISION] = 5.0; // Vision of base class is a range of 5 units
		DefaultStats[Stat.ENGMULTIPLIER] = 1.0;
		DefaultStats[Stat.MARINEMULTIPLIER] = 1.0;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 1.0;
		DefaultStats[Stat.TECHMULTIPLIER] = 1.0;

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
