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

	/**
	 * Convert given activation type enum to a string
	 * 
	 * Arguments
	 * - ActivationType enumToConvert - The activation type to convert
	 * 
	 * Returns
	 * - The string version of the enum. "Unknown Activation Type" if unknown
	 */
	public static string ConvertActivationTypeEnum(ActivationType enumToConvert) {
		switch (enumToConvert) {
		case ActivationType.OFFENSIVE: return "OFFENISVE";
		case ActivationType.DEFENSIVE: return "DEFENSIVE";
		case ActivationType.SUPPORTIVE: return "SUPPORTIVE";
		default: return "Unknown Activation Type";
		}
	}

	/**
	 * Convert given range type enum to a string
	 * 
	 * Arguments
	 * - RangeType enumToConvert - The range type to convert
	 * 
	 * Returns
	 * - The string version of the enum. "Unknown Range Type" if unknown
	 */
	public static string ConvertRangeTypeEnum(RangeType enumToConvert) {
		switch (enumToConvert) {
		case RangeType.SQUARERANGE: return "SQUARE RANGE";
		case RangeType.STRAIGHTLINERANGE: return "STRAIGHT LINE RANGE";
		default: return "Unknown Range Type";
		}
	}
}
