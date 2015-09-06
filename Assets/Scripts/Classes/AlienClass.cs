using UnityEngine;
using System.Collections;

public class AlienClass : PlayerClass {

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
