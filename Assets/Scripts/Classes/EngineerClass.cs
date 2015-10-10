using UnityEngine;
using System.Collections.Generic;

public class EngineerClass : BaseClass {
	
	/**
	 * Constructor
	 * 
	 * Arguments
	 * - The player object
	 */
	public EngineerClass(Player player) : base() {
		DefaultStats[Stat.ENGMULTIPLIER] = 2.0;
		DefaultStats[Stat.MARINEMULTIPLIER] = 1.0;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 1.0;
		DefaultStats[Stat.TECHMULTIPLIER] = 1.0;
		
		PrimaryAbility = new EngineerPrimaryAbility(player); // This class has no primary ability
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
