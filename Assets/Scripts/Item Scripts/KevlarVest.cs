using System.Collections.Generic;

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
	void Start() {
		// The turn effect
		TurnEffect effect = new TurnEffect(Stat.AP, 1.0, 0, "Increase AP", "Icons/Effects/DefaultEffect");
		ItemDescription = "All-purpose vest. Probably won't " + StringMethodsScript.NEWLINE +
				"do much against an alien, but it's better than nothing";

		// Add turn effects
		TurnEffects = new List<TurnEffect>();
		TurnEffects.Add(effect);
	}

	/**
	 * Overwrite toString function
	 * 
	 * Returns
	 * - A string providing info on this item
	 */
	public override string ToString() {
		string turnEffectsString = "Turn Effects: "; // String of turn effects
		int numberOfTurnEffects = TurnEffects.Count; // The number of turn effects
		
		// The string to return. Start with the name
		string toReturn = "Item Name: " + name + StringMethodsScript.NEWLINE;
		
		// Next, add item description
		toReturn += "Description: " + ItemDescription + StringMethodsScript.NEWLINE;
		
		/*
		 * Next, add the stats affected along with the value by which 
		 * these stats are affected by. However, we only have one effect so
		 * we won't even go into the for loop. However, if there is more 
		 * than one effect, then change the values below accordingly
		 */
		for (int i = 0; i < (numberOfTurnEffects - 1); ++i) {
			turnEffectsString += StringMethodsScript.NEWLINE + TurnEffects[i] + ", ";
		}
		turnEffectsString += StringMethodsScript.NEWLINE + TurnEffects[numberOfTurnEffects - 1] + ".";
		
		// Concatenate strings together and return the final string
		toReturn += turnEffectsString + StringMethodsScript.NEWLINE;
		return toReturn;
	}
}
