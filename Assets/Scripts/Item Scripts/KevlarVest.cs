using UnityEngine;
using System.Collections;

/**
 * The class for a Kevlar Vest (Armour)
 */
public class KevlarVest : Armour {

	/**
	 * On start, do the following:
	 * - Set item description
	 * - Set the stats being affected by this armour
	 * - Set the percentage the stat is being affected by
	 * - Set the values the stat is being affected by
	 */
	void Start () {
		this.itemDescription = "All-purpose vest. Probably won't " +
			"do much against an alien, but it's better than nothing";

		/*
		 * We are affecting the following stats:
		 * - HP - More Health
		 * 
		 * We are affecting the stats by percentage, but 
		 * not by simple addition
		 * (More coming soon)
		 */
		this.statsAffected = new Stat[1];
		this.valueEffect = new double[1];
		this.percentEffect = new double[1];

		this.statsAffected[0] = Stat.HP;
		this.valueEffect[0] = 0;
		this.percentEffect[0] = 1.05; // Increase health by 5%
	}

	/**
	 * Overwrite toString function
	 * 
	 * Returns
	 * - A string providing info on this item
	 */
	public override string ToString () {
		string newline = System.Environment.NewLine; // Newline char
		string valueEffectString = "Value increase: "; // The values to affect stat by
		string percentEffectString = "Percent increase: "; // Percent increases
		string statString = "Stats affected: "; // The stats affected
		int numberOfEffects = this.statsAffected.Length; // As it says
		
		// The string to return. Start with the name
		string toReturn = "Item Name: " + this.name + newline;
		
		// Next, add item description
		toReturn += "Description: " + this.itemDescription + newline;
		
		/*
		 * Next, add the stats affected along with the value by which 
		 * these stats are affected by. However, we only have one effect so
		 * we won't even go into the for loop. However, if there is more 
		 * than one effect, then change the values below accordingly
		 */
		for (int i = 0; i < (numberOfEffects - 1); ++i) {
			valueEffectString += newline + this.valueEffect[i] + ", ";
			percentEffectString += newline + this.percentEffect[i] + ", ";
			statString += newline
					+ EnumsToString.convertStatEnum(this.statsAffected[i])
					+ ", ";
		}
		valueEffectString += newline + this.valueEffect[numberOfEffects - 1] + ".";
		percentEffectString += newline + this.percentEffect[numberOfEffects - 1] + ".";
		statString += newline + this.statsAffected[numberOfEffects - 1] + ".";
		
		// Concatenate strings together and return the final string
		toReturn += statString + newline + 
				valueEffectString + newline + 
				percentEffectString + newline;
		return toReturn;
	}
}
