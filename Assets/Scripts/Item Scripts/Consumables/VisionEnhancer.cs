using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionEnhancer : SupportConsumables {

	Effect visionEffect; // The vision effect to add to the player

	void Start() {
		ItemDescription = "Vision Enhancer. Pretend to be Legolas for a short amount of time";

		Range = 1.0; // Range of 1 tiles
		ItemActivationType = ActivationType.SUPPORTIVE;
		
		/* Activatable and droppable */
		Activatable = true;
		Droppable = true;
		
		visionEffect = new StatusTurnEffect(Stat.VISION, 1.0, 0, "Vision Enhancer: Temproary Increased Vision", 
				"Icons/Effects/visionenhancegreen", 2, true);
	}

	public override void Activate(Tile targetTile)
	{
		Player playerScript = Player.MyPlayer.GetComponent<Player>(); // The player's script
		int indexOfEffect; // The index of the effect if it is already attached
		List<Effect> turnEffects; // The turn effects in the player
		
		if (Amount == 0) // We are out cameras
			return;
		else {
			turnEffects = playerScript.GetTurnEffects();
			if (turnEffects.Contains(visionEffect)) { // Effect exists
				indexOfEffect = turnEffects.IndexOf(visionEffect);
				turnEffects[indexOfEffect].IncreaseTurnsRemaining(visionEffect.TurnsRemaining());
			} else // Attach the effect
				playerScript.AttachTurnEffect(visionEffect);
			if (SoundManagerScript.Singleton != null) { /* Play activated sound effect */
				// Move sound manager to this object
				SoundManagerScript.Singleton.gameObject.transform.position = gameObject.transform.position;
				SoundManagerScript.Singleton.PlaySingle3D(ActivateEfx);
			}
			Amount--;
			UpdateContextAwareBox();
			if (Amount == 0) { // Destroy this item
				Player.MyPlayer.GetComponent<Player>().RemoveItem(this, false);
				Destroy(gameObject);
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}

	/**
	 * To string function
	 */
	public override string ToString()
	{
		string toReturn = "Item Name: " + ItemName + StringMethodsScript.NEWLINE;

		toReturn += "Description: " + ItemDescription + StringMethodsScript.NEWLINE;
		toReturn += "Amount: " + Amount + StringMethodsScript.NEWLINE;
		toReturn += "Stat Affected: " + EnumsToString.ConvertStatEnum(Stat.VISION) + StringMethodsScript.NEWLINE;

		return toReturn;
	}
	
	/* Compiler pacifier */
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
	}	
}
