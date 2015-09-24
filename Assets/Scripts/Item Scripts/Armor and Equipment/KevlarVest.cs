using System.Collections.Generic;

/**
 * The class for a Kevlar Vest (Armour)
 */
using UnityEngine;


public class KevlarVest : Armour {

	/**
	 * On start, do the following:
	 * - Set item description
	 * - Record that this item has turn effects
	 * - Set the stats being affected by this armour
	 * - Set the percentage the stat is being affected by
	 * - Set the values the stat is being affected by
	 * - Create turn effects
	 * - Set this item to be inactivatable
	 */
	void Start() {
		// The turn effect
		Effect effect = new StatusTurnEffect(Stat.AP, 1.0, 0, "Increase AP", "Icons/Effects/APup100px", -1, true);
		ItemDescription = "All-purpose vest. Probably won't " + StringMethodsScript.NEWLINE +
				"do much against an alien, but it's better than nothing";

		// Add turn effects
		Effects = new List<Effect>();
		Effects.Add(effect);
		Activatable = false; // This item can't be activated
	}

	/* Implement abstract functions so that compiler doesn't whine */
	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(Tile targetTile)
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
		string turnEffectsString = "Turn Effects: "; // String of turn effects
		int numberOfTurnEffects = Effects.Count; // The number of turn effects
		
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
			turnEffectsString += StringMethodsScript.NEWLINE + Effects[i] + ", ";
		}
		turnEffectsString += StringMethodsScript.NEWLINE + Effects[numberOfTurnEffects - 1] + ".";
		
		// Concatenate strings together and return the final string
		toReturn += turnEffectsString + StringMethodsScript.NEWLINE;
		return toReturn;
	}
}
