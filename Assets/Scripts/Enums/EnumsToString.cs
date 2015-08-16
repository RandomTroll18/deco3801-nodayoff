using UnityEngine;
using System.Collections;

/**
 * Class containing functions to convert the given 
 * enum to a string
 */
public class EnumsToString : MonoBehaviour {

	/**
	 * Function used to convert given stat enum to string
	 * 
	 * Arguments
	 * - Stat enumToConvert - The stat value to convert
	 * 
	 * Returns
	 * - The string version of the enum
	 */
	public static string convertStatEnum (Stat enumToConvert) {
		switch (enumToConvert) {
		case Stat.HP: return "HP";
		case Stat.DEF: return "DEF";
		case Stat.AP: return "AP";
		default: return "Unknown Stat";
		}
	}
}
