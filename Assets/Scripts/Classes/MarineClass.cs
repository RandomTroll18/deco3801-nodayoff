using UnityEngine;
using System.Collections.Generic;

public class MarineClass : PlayerClass {

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The calling player
	 */
	public MarineClass(Player player) {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 10.0;
		DefaultStats[Stat.VISION] = 5.0;
		DefaultStats[Stat.ENGMULTIPLIER] = 1.0;
		DefaultStats[Stat.MARINEMULTIPLIER] = 2.0;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 1.0;
		DefaultStats[Stat.TECHMULTIPLIER] = 1.0;

		PrimaryAbility = new MarinePrimaryAbility(player);
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Marine Class";
	}
}
