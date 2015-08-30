using UnityEngine;
using System.Collections;

/**
 * The class for a Kevlar Vest (Armour)
 */
public class KevlarVest : Armour {

	/**
	 * On start, do the following:
	 * - Set item description
	 * - Record that this item has turn effects
	 * - Set the stats being affected by this armour
	 * - Set the percentage the stat is being affected by
	 * - Set the values the stat is being affected by
	 * - Create turn effects
	 */
	void Start () {
		TurnEffect effect = new TurnEffect(Stat.AP, 1.0); // The turn effect
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

		// Add turn effects
		this.turnEffects = new ArrayList();
		this.turnEffects.Add(effect);
	}

	/**
	 * Overwrite toString function
	 * 
	 * Returns
	 * - A string providing info on this item
	 */
	public override string ToString () {
		string valueEffectString = "Value increase: "; // The values to affect stat by
		string percentEffectString = "Percent increase: "; // Percent increases
		string statString = "Stats affected: "; // The stats affected
		int numberOfEffects = this.statsAffected.Length; // As it says
		
		// The string to return. Start with the name
		string toReturn = "Item Name: " + this.name 
				+ StringMethodsScript.NEWLINE;
		
		// Next, add item description
		toReturn += "Description: " + this.itemDescription 
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
			percentEffectString += StringMethodsScript.NEWLINE 
					+ this.percentEffect[i] + ", ";
			statString += StringMethodsScript.NEWLINE
					+ EnumsToString.convertStatEnum(this.statsAffected[i])
					+ ", ";
		}
		valueEffectString += StringMethodsScript.NEWLINE 
				+ this.valueEffect[numberOfEffects - 1] + ".";
		percentEffectString += StringMethodsScript.NEWLINE 
				+ this.percentEffect[numberOfEffects - 1] + ".";
		statString += StringMethodsScript.NEWLINE 
				+ this.statsAffected[numberOfEffects - 1] + ".";
		
		// Concatenate strings together and return the final string
		toReturn += statString + StringMethodsScript.NEWLINE + 
				valueEffectString + StringMethodsScript.NEWLINE + 
				percentEffectString + StringMethodsScript.NEWLINE;
		return toReturn;
	}
}
