using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndTurnButtonScript : MonoBehaviour {

	public GameObject EndTurnButton; // The button that this script is attached to
	public GameObject GameManagerObject; // The game manager's object

	GameObject playerOwner;
	
	public void StartMe() {
		playerOwner = Player.MyPlayer;
	}

	/**
	 * Update function of the end turn button
	 */
	void Update() {
		Player playerScript = playerOwner.GetComponent<Player>();
		if (playerScript.IsStunned) // Disable end turn button
			EndTurnButton.GetComponent<Button>().interactable = false;
		else
			EndTurnButton.GetComponent<Button>().interactable = true;
		if (playerScript.IsPlayerNoLongerActive()) 
			EndTurnButton.GetComponentInChildren<Text>().text = "Undo End Turn";
		else
			EndTurnButton.GetComponentInChildren<Text>().text = "End Turn";
	}

	/**
	 * End player's turn
	 */
	public void EndTurn() {
		Player playerScript = playerOwner.GetComponent<Player>();
		if (!playerScript.IsPlayerNoLongerActive()) {
			playerScript.SetInActivity(true);
			GameManagerObject.GetComponent<PhotonView>().RPC("SetInactivePlayer", 
			                                                 PhotonTargets.All, null);
		} else { // Player is trying to activate their turn again
			playerScript.SetInActivity(false);
			GameManagerObject.GetComponent<PhotonView>().RPC("SetActivePlayer", PhotonTargets.All, null);
		}
	}
}