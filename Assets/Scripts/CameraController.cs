﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

	public GameObject GameManagerObject; // The game manager object
	public GameObject ContextAwareBox; // The context aware box
	GameManager gameManagerScript; // The game manager script

	public GameObject Player;
	public float CamSpeed;
	/* The distance the mouse pointer needs to be from the edge before the 
	 * screen moves.
	 */
	public float GUISize;
	public LayerMask LayerMask;

	Vector3 offset;
	//Player playerScript;
	MovementController movController;
	ActivationTileController actController; // Activation Controller script
	/* Whether the player can control the camera */
	bool locked = false;

	void Start() {
		//playerScript = Player.GetComponent<Player>();
		movController = Player.GetComponent<MovementController>();
		actController = ContextAwareBox.GetComponent<ActivationTileController>();

		offset = transform.position - Player.transform.position;
		//this.gameManagerScript = this.gameManagerObject.GetComponent<GameManager>();

		 movController.UnblockTile(new Tile(-1, -8));
	}

	void Update() {
		// Camera panning with mouse
		Rect recdown = new Rect(0, 0, Screen.width, GUISize);
		Rect recup = new Rect(0, Screen.height - GUISize, Screen.width, GUISize);
		Rect recleft = new Rect(0, 0, GUISize, Screen.height);
		Rect recright = new Rect(Screen.width - GUISize, 0, GUISize, Screen.height);

		/* Camera controlling input */
		if (!locked) {
			if (Input.GetKey("space")) {
				ResetCamera();
			}

			if (Input.GetKey("s") || recdown.Contains(Input.mousePosition)) {
				transform.Translate(0, 0, -CamSpeed, Space.World);
			}
			
			if (Input.GetKey("w") || recup.Contains(Input.mousePosition)) {
				transform.Translate(0, 0, CamSpeed, Space.World);
			}
			
			if (Input.GetKey("a") || recleft.Contains(Input.mousePosition)) {
				transform.Translate(-CamSpeed, 0, 0, Space.World);
			}
			
			if (Input.GetKey("d") || recright.Contains(Input.mousePosition)) {
				transform.Translate(CamSpeed, 0, 0, Space.World);
			}
		}


		// Mouse click detection
		if (Input.GetMouseButtonUp(0)) {
			Tile goal = Tile.MouseToTile((LayerMask));
			//Debug.Log("Clicked: " + goal.ToString());
			if (actController.ActivationTiles().Contains(goal)) {
				Debug.Log("Clicked Tile: (" + goal.X + ", " + goal.Z + ")");
				actController.Activate(goal);
			} else if (goal != null) {
				movController.RequestMovement(goal);
			}
		}
	}

	public void LockCamera() {
		locked = true;
	}

	public void UnlockCamera() {
		locked = false;
	}

	/*
	 * Call this when the player moves.
	 */
	public void ResetCamera() {
		transform.position = Player.transform.position + offset;
	}

	/**
	 * Moves the camera to the given location.
	 * 
	 * At the moment this teleports the camera but in the future we 
	 * could make it pan.
	 */
	public void MoveCamera(Tile location) {
		Vector3 dest = transform.position;
		dest.z = location.Z;
		dest.x = location.X;
		transform.position = dest;
	}
}
