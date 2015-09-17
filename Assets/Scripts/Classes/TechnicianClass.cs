﻿using UnityEngine;
using System.Collections.Generic;

public class TechnicianClass : PlayerClass {

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - Reference to player
	 */
	public TechnicianClass(Player player) {
		DefaultStats = new Dictionary<Stat, double>();
		DefaultStats[Stat.AP] = 10.0;
		DefaultStats[Stat.VISION] = 5.0;
		DefaultStats[Stat.ENGMULTIPLIER] = 1.0;
		DefaultStats[Stat.MARINEMULTIPLIER] = 1.0;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 1.0;
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
