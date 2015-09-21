using UnityEngine;
using System.Collections.Generic;

public class TechnicianClass : BaseClass {

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - Reference to player
	 */
	public TechnicianClass(Player player) : base() {
		DefaultStats[Stat.AP] = 10.0;
		DefaultStats[Stat.TECHMULTIPLIER] = 2.0;

		PrimaryAbility = new TechnicianPrimaryAbility(player);
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
