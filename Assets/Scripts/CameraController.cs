using System.Collections.Generic;
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
	bool IsTargetConfirmation = false; // Record if we are just confirming our target

	void Start() {
		//playerScript = Player.GetComponent<Player>();
		movController = GameObject.FindGameObjectWithTag("GameController")
			.GetComponent<MovementController>();
		actController = ContextAwareBox.GetComponent<ActivationTileController>();

		ResetOffset();
		//this.gameManagerScript = this.gameManagerObject.GetComponent<GameManager>();

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
			if (!EventSystem.current.IsPointerOverGameObject()) {
				Tile goal = Tile.MouseToTile((LayerMask));
				//Debug.Log("Clicked: " + goal.ToString());
				if (actController.ActivationTiles().Contains(goal)) {
					if (!IsTargetConfirmation) {
						actController.InitiateTargetConfirmation(goal); // Confirm target
						IsTargetConfirmation = true;
					}
					else {
						actController.Activate(goal); // Activate the item
						IsTargetConfirmation = false;
					}
				} else if (goal != null) {
					actController.DestroyActivationTiles(); // Stop targetting
					IsTargetConfirmation = false;
					movController.RequestMovement(goal);
				}
			}
		}
	}

	/*
	 * This needs to be called since Player can change in the middle of a 
	 * game
	 */ 
	void ResetOffset() {
		offset = transform.position - Player.transform.position;
		offset = new Vector3(0, offset.y, 0);
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
		Vector3 dest = Tile.TileMiddle(location);
		dest.y = transform.position.y;
		transform.position = dest;
	}
}
