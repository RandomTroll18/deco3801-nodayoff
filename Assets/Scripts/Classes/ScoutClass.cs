using UnityEngine;
using System.Collections.Generic;

public class ScoutClass : BaseClass {

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The player that belongs to this class
	 */
	public ScoutClass(Player player) : base() {
		DefaultStats[Stat.AP] = 10.0;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 2.0;

		PrimaryAbility = new ScoutPrimaryAbility(player);
		ClassTypeEnum = Classes.SCOUT;
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
