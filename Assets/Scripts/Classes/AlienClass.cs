using UnityEngine;
using System.Collections.Generic;

public class AlienClass : PlayerClass {

	/**
	 * Constructor for Alien Class
	 */
	public AlienClass() {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 20.0;
		DefaultStats[Stat.VISION] = 7.0;
		DefaultStats[Stat.ENGMULTIPLIER] = 1.5;
		DefaultStats[Stat.MARINEMULTIPLIER] = 1.5;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 1.5;
		DefaultStats[Stat.TECHMULTIPLIER] = 1.5;
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Alien Class";
	}
}
