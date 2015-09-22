

using System.Collections.Generic;
using UnityEngine;

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
		double[] apEffect = new double[3];
		ItemDescription = "Item which restores your AP";

		/*
		 * For this item, we are only affecting one stat - AP
		 */
		InstantEffects = new Dictionary<Stat, double[]>();
		apEffect[0] = 10.0;
		apEffect[1] = -1;
		apEffect[2] = 1.0;
		InstantEffects[Stat.AP] = apEffect;
	}

	/**
	 * Override Activate function
	 */
	public override void Activate(Tile targetTile) {
		Debug.Log("AP Restorer activation");
	}

	/* Override abstract functions so that compiler doesn't whine */
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
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

		// The string to return. Start with the name
		string toReturn = "Item Name: " + name + StringMethodsScript.NEWLINE;

		// Next, add item description
		toReturn += "Description: " + ItemDescription + StringMethodsScript.NEWLINE;

		// Next, add the amount of this item
		toReturn += "Amount: " + Amount + StringMethodsScript.NEWLINE;

		valueEffectString += StringMethodsScript.NEWLINE + InstantEffects[Stat.AP][0] + ".";
		statString += StringMethodsScript.NEWLINE + EnumsToString.ConvertStatEnum(Stat.AP) + ".";

		// Concatenate strings together and return the final string
		toReturn += statString + StringMethodsScript.NEWLINE + valueEffectString + StringMethodsScript.NEWLINE;
		return toReturn;
	}
}
