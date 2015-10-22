using UnityEngine;
using System.Collections.Generic;

public class BaseClass : PlayerClass {
	
	/**
	 * Constructor
	 */
	public BaseClass() {
		Stats = new Dictionary<Stat, double>();
		DefaultStats = new Dictionary<Stat, double>();
		Stats[Stat.AP] = DefaultStats[Stat.AP] = 250.0; 
		Stats[Stat.VISION] = DefaultStats[Stat.VISION] = 2.0; // Vision of base class is medium
		Stats[Stat.ENGMULTIPLIER] = DefaultStats[Stat.ENGMULTIPLIER] = 1.0;
		Stats[Stat.MARINEMULTIPLIER] = DefaultStats[Stat.MARINEMULTIPLIER] = 1.0;
		Stats[Stat.SCOUTMULTIPLIER] = DefaultStats[Stat.SCOUTMULTIPLIER] = 1.0;
		Stats[Stat.TECHMULTIPLIER] = DefaultStats[Stat.TECHMULTIPLIER] = 1.0;

		PrimaryAbility = null; // This class has no primary ability
		ClassTypeEnum = Classes.BETRAYER; // Base class is a betrayer
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Base Class";
	}
}
