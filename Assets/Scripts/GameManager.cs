using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	 
	public int RoundsLeftUntilLose;
	public GameObject[] OpenedDoors;
	public Text RemainingTurnsText; // The text counter for the remaining turns

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

	int numPlayers;
	int playersLeft; // The number of players still active
	Dictionary<Tile, GameObject> doors = new Dictionary<Tile, GameObject>(); // The doors
	MovementController movController; // Movement controller script

	/*
	 * From now on, it's safer to replace Start with StartMe in all your classes and to 
	 * call StartMe in StartScripts
	 */
	public void StartMe() {
		StartPlaying();
	}

	[PunRPC]
	public void AddPlayer() {
		numPlayers++;
		this.playersLeft++;
		Debug.Log("Player list size: " + this.playersLeft);
	}

	void StartScripts() {
		Player.MyPlayer.GetComponent<MovementController>().StartMe();
		Player.MyPlayer.GetComponentInChildren<CameraController>().StartMe();
		Player.MyPlayer.GetComponentInChildren<Camera>().enabled = true;
		Player.MyPlayer.GetComponent<Player>().enabled = true;
		Object.FindObjectOfType<Poll>().StartMe();
		Object.FindObjectOfType<MainCanvasButton>().StartMe();
		Object.FindObjectOfType<ContextAwareBox>().StartMe();
		Object.FindObjectOfType<InventoryUIScript>().StartMe();
		/*
		 * This is how you call StartMe for classes that aren't singletons
		 */
		foreach (InventoryUISlotScript script in Object.FindObjectsOfType<InventoryUISlotScript>()) {
			script.StartMe();
		}
		Object.FindObjectOfType<ChatTest>().StartMe();
		Object.FindObjectOfType<ClassPanelScript>().StartMe();
		Object.FindObjectOfType<EffectPanelScript>().StartMe();
		Object.FindObjectOfType<PrimaryObjectiveController>().StartMe();
		Object.FindObjectOfType<EndTurnButtonScript>().enabled = true;
		Object.FindObjectOfType<EndTurnButtonScript>().StartMe();
		Object.FindObjectOfType<APCounterScript>().StartMe();
		Object.FindObjectOfType<APCounterScript>().enabled = true;
		Player.MyPlayer.GetComponent<MovementController>().enabled = true;
	}

	public void StartPlaying() {
		StartScripts();

		/*
		 * This is where the old Start() started
		 */
		movController = Player.MyPlayer.GetComponent<MovementController>();
		InitalizeDoors();
		TurnOnLighting();
		validTurn = false;
		//		Debug.Log("Valid turn is: " + validTurn + " by default");
		//		Debug.Log("Number of players left: " + playersLeft);
		RemainingTurnsText.text = "Rounds Remaining: " + RoundsLeftUntilLose;

		this.enabled = true; // Turns on the Update() function
	}

	void InitalizeDoors() {
		foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door")) {
			doors.Add(Tile.TilePosition(door.transform.position), door);
		}

		foreach (GameObject openDoor in OpenedDoors) {
			if (openDoor != null) {
				OpenDoor(Tile.TilePosition(openDoor.transform.position));
			}
		}
	}

	void TurnOnLighting() {
		RenderSettings.ambientLight = Color.black;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Lighting")) {
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
			if (playersLeft <= 0) { // No more active players
				validTurn = false;
			}
			return;
		}

		//Debug.Log ("Invalid turn. Game Manager doing stuff");
		playersLeft = numPlayers;

		// Initialize player stats - AP and apply player effects
		Player p = Player.MyPlayer.GetComponent<Player>();
		p.InitializeStats();
		p.ApplyTurnEffects();
		p.ReduceItemCoolDowns();
		p.ReduceAbilityTurns();
		p.SetActivity(false);

		RoundsLeftUntilLose--;
		RemainingTurnsText.text = "Rounds Remaining: " + RoundsLeftUntilLose;
		if (RoundsLeftUntilLose <= 0) {
			Application.LoadLevel("GameOver");
		}

		validTurn = true;
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
	[PunRPC]
	public void SetInactivePlayer() {
		playersLeft--;
		Debug.Log("Players left: " + playersLeft);
	}

	/**
	 * Opens the door at the given position. This means the door will start its animation and will
	 * no longer block that tile.
	 */
	public void OpenDoor(Tile position) {
		GameObject door;
		if (!doors.TryGetValue(position, out door)) {
			Debug.LogWarning("You tried to open a door that doesn't exist on that tile. BEN");
			return;
		}

		door.GetComponent<Animator>().enabled = true;
		movController.UnblockTile(position);
	}
}
