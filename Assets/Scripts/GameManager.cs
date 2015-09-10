using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	 
	/*
	 * Remember whether the current turn is a valid turn. 
	 * 
	 * A turn is invalid only if every player has 'ended their turn'.
	 * Invalid turns are where the game manager can do all required 
	 * calculations.
	 * 
	 * A valid turn is when the game manager is done with calculations
	 * and any actions it needs to do and allows players to move
	 */
	bool validTurn;
	int playersLeft; // The number of players still active
	public Player[] PlayerList; // List of players

	/**
	 * Start function. Needs to be done:
	 * - Initialize list of players
	 * - Initialize valid turn. If there is some calculation required, 
	 * at the start of the game, then set validTurn to be 0. 
	 * - Initialize number of players still active. 
	 */
	void Start() {
		TurnOnLighting();

		validTurn = false;
		playersLeft = PlayerList.Length;
		Debug.Log("Valid turn is: " + validTurn + " by default");
		Debug.Log("Number of players left: " + playersLeft);
	}

	void TurnOnLighting() {
		RenderSettings.ambientLight = Color.black;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Lighting")) {
			Debug.Log("DSFSFDSFSDFSFFDSF");
			g.GetComponent<Light>().enabled = true;
		}
	}

	/**
	 * Update function. Needs to be done:
	 * - if it is a valid turn and there are still active players, 
	 * don't do anything
	 * - if it is a valid turn but there are no more active players, 
	 * set the turn to be invalid
	 * - if it is an invalid turn, then do any required actions
	 */
	void Update() {
		if (validTurn) {
			if (playersLeft == 0) { // No more active players
				validTurn = false;
				Debug.Log ("No more active players");
			}
			return;
		} else {
			Debug.Log ("Invalid turn. Game Manager doing stuff");
			// Reset number of players left
			playersLeft = PlayerList.Length;
			// Initialize player stats - AP and apply player effects
			for (int i = 0; i < playersLeft; ++i) {
				PlayerList[i].InitializeStats();
				PlayerList[i].ApplyTurnEffects();
				PlayerList[i].InitializeAttachedObjects();
				PlayerList[i].ReduceItemCoolDowns();
				PlayerList[i].ReduceAbilityTurns();
			}
			validTurn = true;
		}
	}

	/**
	 * Returns whether it is a vaid turn or not
	 * 
	 * Returns
	 * - true if the current turn is still valid. 
	 * - false if otherwise
	 */
	public bool IsValidTurn() {
		return validTurn;
	}

	/**
	 * Record that a player is no longer active
	 */
	public void SetInactivePlayer() {
		if (playersLeft != 0) {
			playersLeft--;
		}
	}
}
