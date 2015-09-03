
/**
 * Consumable which restores AP when used
 */
public class APRestorer : RecoveryConsumables {

	/**
	 * On start, do the following:
	 * - Set item description
	 * - Set amount recovered
	 * - The stat affected
	 */
	void Start() {
		ItemDescription = "Item which restores your AP";

		/*
		 * For this item, we are only affecting one stat - AP
		 */
		ValueEffect = new double[1];
		StatsAffected = new Stat[1];

		ValueEffect[0] = 10.0;
		StatsAffected[0] = Stat.AP;
	}

	/**
	 * Overwrite toString function
	 * 
	 * Returns
	 * - A string providing info on this item
	 */
	public override string ToString() {
		string valueEffectString = "Value increase: "; // The values to affect stat by
		string statString = "Stats affected: "; // The stats affected
		int numberOfEffects = StatsAffected.Length; // As it says

		// The string to return. Start with the name
		string toReturn = "Item Name: " + name + StringMethodsScript.NEWLINE;

		// Next, add item description
		toReturn += "Description: " + ItemDescription + StringMethodsScript.NEWLINE;

		// Next, add the amount of this item
		toReturn += "Amount: " + Amount + StringMethodsScript.NEWLINE;

		/*
		 * Next, add the stats affected along with the value by which 
		 * these stats are affected by. However, we only have one effect so
		 * we won't even go into the for loop. However, if there is more 
		 * than one effect, then change the values below accordingly
		 */
		for (int i = 0; i < (numberOfEffects - 1); ++i) {
			valueEffectString += StringMethodsScript.NEWLINE + ValueEffect[i] + ", ";
			statString += StringMethodsScript.NEWLINE + EnumsToString.ConvertStatEnum(StatsAffected[i])+ ", ";
		}
		valueEffectString += StringMethodsScript.NEWLINE + ValueEffect[numberOfEffects - 1] + ".";
		statString += StringMethodsScript.NEWLINE + StatsAffected[numberOfEffects - 1] + ".";

		// Concatenate strings together and return the final string
		toReturn += statString + StringMethodsScript.NEWLINE + valueEffectString + StringMethodsScript.NEWLINE;
		return toReturn;
	}
}
