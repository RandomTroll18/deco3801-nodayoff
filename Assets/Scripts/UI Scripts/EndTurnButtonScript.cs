using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndTurnButtonScript : MonoBehaviour {

	public GameObject PlayerOwner; // The player that owns this button
	public GameObject EndTurnButton; // The button that this script is attached to
	public GameObject GameManagerObject; // The game manager's object

	/**
	 * Update function of the end turn button
	 */
	void Update() {
		Player playerScript = PlayerOwner.GetComponent<Player>();
		if (playerScript.IsPlayerNoLongerActive()) EndTurnButton.GetComponent<Button>().interactable = false;
		else EndTurnButton.GetComponent<Button>().interactable = true;
	}

	/**
	 * End player's turn
	 */
	public void EndTurn() {
		Player playerScript = PlayerOwner.GetComponent<Player>(); // The script of the player
		GameManager gameManager = GameManagerObject.GetComponent<GameManager>(); // Game manager script
		if (!playerScript.IsPlayerNoLongerActive()) {
			playerScript.SetActivity(true);
			gameManager.SetInactivePlayer();
		}
	}
}
