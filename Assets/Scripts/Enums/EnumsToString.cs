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
	 * - Context enumToConvert - the context value to convert
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
	 * Convert given action cost enum to a string
	 * 
	 * Arguments
	 * - ActionCost enumToCovert - The action cost value to convert
	 * 
	 * Returns
	 * - The string version of the enum. "Unknown Action Cost" if unknown
	 */
	public static string ConvertActionCostEnum(ActionCost enumToConvert) {
		switch (enumToConvert) {
		case ActionCost.DOORS: return "DOORCOST";
		case ActionCost.MOVEMENT: return "MOVEMENTCOST";
		case ActionCost.REPAIR: return "REPAIRCOST";
		case ActionCost.STRENGTH: return "STRENGTHCOST";
		case ActionCost.TECH: return "TECHCOST";
		default: return "Unknown Action Cost";
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
