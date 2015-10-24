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
		Stats[Stat.AP] = DefaultStats[Stat.AP] = 35.0; 
		Stats[Stat.ENGMULTIPLIER] = DefaultStats[Stat.ENGMULTIPLIER] = 0.5;
		Stats[Stat.MARINEMULTIPLIER] = DefaultStats[Stat.MARINEMULTIPLIER] = 0.5;
		Stats[Stat.SCOUTMULTIPLIER] = DefaultStats[Stat.SCOUTMULTIPLIER] = 0.5;
		Stats[Stat.TECHMULTIPLIER] = DefaultStats[Stat.TECHMULTIPLIER] = 0.5;

		PrimaryAbility = new ScoutPrimaryAbility();
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
