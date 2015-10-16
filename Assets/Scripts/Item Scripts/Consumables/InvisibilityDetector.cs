using UnityEngine;
using System.Collections;

public class InvisibilityDetector : SupportConsumables {

	ComponentTurnEffect invisibilityDetectingEffect; // Trap detecting effect
	
	void Start() {
		ItemDescription = "Invisibility Detector. Makes cowards visible";
		
		InstantEffects = null;
		
		Range = 0.0; // Range of 3 tiles
		
		/* Activatable and droppable */
		Activatable = true;
		Droppable = true;
		
		invisibilityDetectingEffect = new ComponentTurnEffect(
				ComponentEffectType.INVISIBILITYDETECTOR, 
				"Invisibility Detector: Players that were stealthed are now visible", 
				"Icons/Effects/DefaultEffect", 
				3,
				3,
				false
		);
	}
	
	public override void Activate()
	{
		if (Amount == 0) // We are out cameras
			return;
		else {
			Player.MyPlayer.GetComponent<Player>().AttachTurnEffect(invisibilityDetectingEffect);
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
