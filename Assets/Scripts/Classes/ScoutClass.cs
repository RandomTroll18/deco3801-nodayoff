using UnityEngine;
using System.Collections.Generic;

public class ScoutClass : PlayerClass {

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The player that belongs to this class
	 */
	public ScoutClass(Player player) {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 10.0;
		DefaultStats[Stat.VISION] = 5.0;
		DefaultStats[Stat.ENGMULTIPLIER] = 1.0;
		DefaultStats[Stat.MARINEMULTIPLIER] = 1.0;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 2.0;
		DefaultStats[Stat.TECHMULTIPLIER] = 1.0;

		PrimaryAbility = new ScoutPrimaryAbility(player);
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
