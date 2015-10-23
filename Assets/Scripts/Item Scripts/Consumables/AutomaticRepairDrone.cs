using UnityEngine;
using System.Collections;

public class AutomaticRepairDrone : SupportConsumables {

	public int AdditionalTurns = 3; // Additional number of turns

	/**
	 * On start, do the following:
	 * - Set item description
	 * - Set amount recovered
	 * - The stat affected
	 */
	void Start() {
		ItemDescription = "Repair Drone that can delay your death by a few turns";

		Range = 0.0; // No range. Instant use
		Activatable = true; // Can be activated
		Droppable = true;
	}

	/**
	 * Activate function
	 */
	public override void Activate()
	{
		GameObject gameManager = Object.FindObjectOfType<GameManager>().gameObject; // The game manager object
		Debug.Log("Automatic Repair Drone Activated");

		if (Amount == 0)
			return;
		else {
			gameManager.GetComponent<PhotonView>().RPC("AddTurns", PhotonTargets.All, new object[] {AdditionalTurns});
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
	 * Tostring function
	 */
	public override string ToString()
	{
		string toReturn = "Item Name: " + ItemName + StringMethodsScript.NEWLINE; // The string to return

		toReturn += ItemDescription + StringMethodsScript.NEWLINE;
		toReturn += "Amount: " + Amount + StringMethodsScript.NEWLINE;

		return toReturn;
	}

	/* Compiler pacifying */
	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(Tile targetTile)
	{
		throw new System.NotImplementedException();
	}
}
