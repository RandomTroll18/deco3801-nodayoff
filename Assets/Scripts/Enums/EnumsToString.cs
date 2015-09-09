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
	 * - The string version of the enum. "Unknown Stat" if unknown.
	 */
	public static string ConvertStatEnum(Stat enumToConvert) {
		switch (enumToConvert) {
		case Stat.AP: return "AP";
		case Stat.VISION: return "VISION";
		default: return "Unknown Stat";
		}
	}

	/**
	 * Function used to convert the given context enum to 
	 * a string
	 * 
	 * Arguments
	 * - Contexts enumToConvert - the context value to convert
	 * 
	 * Returns
	 * - The string version of the enum. "Unknown Context" if unknown
	 */
	public static string ConvertContextEnum(Context enumToConvert) {
		switch (enumToConvert) {
		case Context.IDLE: return "IDLE";
		case Context.INVENTORY: return "INVENTORY";
		default: return "Unknown Context";
		}
	}
}
