using UnityEngine;
using System.Collections.Generic;

public class EngineerRobotClass : BaseClass {

	/**
	 * Constructor
	 */
	public EngineerRobotClass() : base() {
		DefaultStats[Stat.AP] = 10.0; // Default AP is 10
		DefaultStats[Stat.VISION] = 2.0; // Vision of base class is a range of 2 units
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
