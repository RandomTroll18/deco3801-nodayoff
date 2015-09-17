using UnityEngine;
using System.Collections.Generic;

public class EngineerRobotClass : PlayerClass {

	/**
	 * Constructor
	 */
	public EngineerRobotClass() {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 10.0; // Default AP is 10
		DefaultStats[Stat.VISION] = 5.0; // Vision of base class is a range of 5 units
		DefaultStats[Stat.ENGMULTIPLIER] = 2.0;
		DefaultStats[Stat.MARINEMULTIPLIER] = 1.0;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 1.0;
		DefaultStats[Stat.TECHMULTIPLIER] = 1.0;

		PrimaryAbility = new EngineerRobotPrimaryAbility();
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
