using UnityEngine;
using System.Collections;

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
	void Start () {
		this.itemDescription = "Item which restores your AP";

		/*
		 * For this item, we are only affecting one stat - AP
		 */
		this.valueEffect = new double[1];
		this.statsAffected = new Stat[1];

		this.valueEffect[0] = 10.0;
		this.statsAffected[0] = Stat.AP;
	}

	/**
	 * Overwrite toString function
	 * 
	 * Returns
	 * - A string providing info on this item
	 */
	public override string ToString () {
		string valueEffectString = "Value increase: "; // The values to affect stat by
		string statString = "Stats affected: "; // The stats affected
		int numberOfEffects = this.statsAffected.Length; // As it says

		// The string to return. Start with the name
		string toReturn = "Item Name: " + this.name 
				+ StringMethodsScript.NEWLINE;

		// Next, add item description
		toReturn += "Description: " + this.itemDescription 
				+ StringMethodsScript.NEWLINE;

		// Next, add the amount of this item
		toReturn += "Amount: " + this.amount 
			+ StringMethodsScript.NEWLINE;

		/*
		 * Next, add the stats affected along with the value by which 
		 * these stats are affected by. However, we only have one effect so
		 * we won't even go into the for loop. However, if there is more 
		 * than one effect, then change the values below accordingly
		 */
		for (int i = 0; i < (numberOfEffects - 1); ++i) {
			valueEffectString += StringMethodsScript.NEWLINE 
					+ this.valueEffect[i] + ", ";
			statString += StringMethodsScript.NEWLINE
					+ EnumsToString.convertStatEnum(this.statsAffected[i])
					+ ", ";
		}
		valueEffectString += StringMethodsScript.NEWLINE 
				+ this.valueEffect[numberOfEffects - 1] + ".";
		statString += StringMethodsScript.NEWLINE 
				+ this.statsAffected[numberOfEffects - 1] + ".";

		// Concatenate strings together and return the final string
		toReturn += statString + StringMethodsScript.NEWLINE 
			+ valueEffectString + StringMethodsScript.NEWLINE;
		return toReturn;
	}
}
