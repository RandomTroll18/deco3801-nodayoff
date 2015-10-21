﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapDetector : SupportConsumables {

	Effect trapDetectingEffect; // Trap detecting effect

	void Start() {
		ItemDescription = "Trap Detector. Safeguard yourself against pranks";
		
		InstantEffects = null;
		
		Range = 0.0; // Range of 3 tiles
		
		/* Activatable and droppable */
		Activatable = true;
		Droppable = true;

		trapDetectingEffect = new ComponentTurnEffect(
				ComponentEffectType.TRAPDETECTOR, 
				"Trap Detector: Traps are now visible", 
				"Icons/Effects/trapdetectgreen", 
				3,
				3,
				false
		);
	}

	public override void Activate()
	{
		Player playerScript = Player.MyPlayer.GetComponent<Player>(); // The player's script
		int indexOfEffect; // The index of the effect if it is already attached
		List<Effect> turnEffects; // The turn effects in the player

		if (Amount == 0) // We are out cameras
			return;
		else {
			turnEffects = playerScript.GetTurnEffects();
			if (turnEffects.Contains(trapDetectingEffect)) { // Effect exists
				indexOfEffect = turnEffects.IndexOf(trapDetectingEffect);
				turnEffects[indexOfEffect].IncreaseTurnsRemaining(trapDetectingEffect.TurnsRemaining());
			} else // Attach the effect
				playerScript.AttachTurnEffect(trapDetectingEffect);
			Amount--;
			UpdateContextAwareBox();
			if (Amount == 0) { // Destroy this item
				Player.MyPlayer.GetComponent<Player>().RemoveItem(this, false);
				Destroy(gameObject);
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}

	/* Pacify compiler */
	public override void Activate(Tile targetTile)
	{
		throw new System.NotImplementedException();
	}

	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
	}
	
	public override string ToString()
	{
		string toReturn = "Item Name: " + ItemName + StringMethodsScript.NEWLINE;
		toReturn += "Description: " + ItemDescription + StringMethodsScript.NEWLINE;
		toReturn += "Amount: " + Amount + StringMethodsScript.NEWLINE;

		return toReturn;
	}
}
