using UnityEngine;
using System.Collections.Generic;

public class EngineerRobotClass : BaseClass {

	/**
	 * Constructor
	 */
	public EngineerRobotClass() : base() {
		/* Robot is like the engineer, except it can't do as much */
		Stats[Stat.AP] = DefaultStats[Stat.AP] = 10.0; // Default AP is 10
		Stats[Stat.VISION] = DefaultStats[Stat.VISION] = 2.0; // Vision of base class is a range of 2 units
		Stats[Stat.ENGMULTIPLIER] = DefaultStats[Stat.ENGMULTIPLIER] = 2.0;
		Stats[Stat.MARINEMULTIPLIER] = DefaultStats[Stat.MARINEMULTIPLIER] = 1.0;
		Stats[Stat.SCOUTMULTIPLIER] = DefaultStats[Stat.SCOUTMULTIPLIER] = 1.0;
		Stats[Stat.TECHMULTIPLIER] = DefaultStats[Stat.TECHMULTIPLIER] = 1.0;

		PrimaryAbility = new EngineerRobotPrimaryAbility();
		ClassTypeEnum = Classes.ENGINEER;
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
