using UnityEngine;
using System.Collections.Generic;

public class MarineClass : BaseClass {

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The calling player
	 */
	public MarineClass(Player player) : base(){
		Stats[Stat.MARINEMULTIPLIER] = DefaultStats[Stat.MARINEMULTIPLIER] = 1.5;
		PrimaryAbility = new MarinePrimaryAbility(player);
		ClassTypeEnum = Classes.MARINE;
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
