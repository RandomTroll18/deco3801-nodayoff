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
	private bool validTurn;
	private int playersLeft; // The number of players still active
	public Player[] playerList; // List of players

	/**
	 * Start function. Needs to be done:
	 * - Initialize list of players
	 * - Initialize valid turn. If there is some calculation required, 
	 * at the start of the game, then set validTurn to be 0. 
	 * - Initialize number of players still active. 
	 */
	void Start () {
		this.validTurn = false;
		this.playersLeft = this.playerList.Length;
		Debug.Log("Valid turn is: " + this.validTurn + " by default");
		Debug.Log("Number of players left: " + this.playersLeft);
	}

	/**
	 * Update function. Needs to be done:
	 * - if it is a valid turn and there are still active players, 
	 * don't do anything
	 * - if it is a valid turn but there are no more active players, 
	 * set the turn to be invalid
	 * - if it is an invalid turn, then do any required actions
	 */
	void Update () {
		if (this.validTurn) {
			if (this.playersLeft == 0) { // No more active players
				this.validTurn = false;
				Debug.Log("No more active players");
			}
			return;
		} else {
			Debug.Log("Invalid turn. Game Manager doing stuff");
			// Reset number of players left
			this.playersLeft = this.playerList.Length;
			// Initialize player stats - AP and apply player effects
			for (int i = 0; i < this.playersLeft; ++i) {
				this.playerList[i].initializeStats();
				this.playerList[i].applyTurnEffects();
			}
			this.validTurn = true;
		}
	}

	/**
	 * Returns whether it is a vaid turn or not
	 * 
	 * Returns
	 * - true if the current turn is still valid. 
	 * - false if otherwise
	 */
	public bool isValidTurn () {
		return this.validTurn;
	}

	/**
	 * Record that a player is no longer active
	 */
	public void setInactivePlayer () {
		if (this.playersLeft != 0) this.playersLeft--;
	}
}
