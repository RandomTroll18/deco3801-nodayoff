using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public GameObject GameManagerObject; // The game manager object
	public GameObject ContextAwareBox; // The context aware box
	public GameObject TileHighlight; // Highlighted Tile to generate
	public float RotationSpeed; // Camera rotation speed
	public Transform Target; // The character that this camera controller belongs to
	public float ZoomSpeed; // Speed of zooming in and out
	public Locations Location; // Location of the player in the map
	public float CamSpeed; // The speed of panning the camera
	/* 
	 * The distance the mouse pointer needs to be from the edge before the 
	 * screen moves.
	 */
	public float GUISize;
	public LayerMask LayerMask; // Layer mask to filter certain game objects

	/*
	 * Images for the different map locations
	 */
	Image leftWing;
	Image rightWing;
	Image leftGun;
	Image rightGun;
	Image power;
	Image quarters;
	Image cargo;
	Image bridge;

	/*
	 * Boundaries
	 */
	const float MAX_Z = 120f;
	const float MIN_Z = -55f;
	const float MAX_X = 86f;
	const float MIN_X = -75f;
	const float MAX_Y = 25f;
	const float MIN_Y = 12.25f;
	Vector3 offset;
	MovementController movController;
	ActivationTileController actController; // Activation Controller script
	/* Whether the player can control the camera */
	bool locked = false; // Lock the camera from moving
	bool IsTargetConfirmation = false; // Record if we are just confirming our target
	Quaternion initialRotation; // Initial rotation of camera
	Tile destination = null; // The destination to snap the camera to
	Vector3 start; // Starting position of the camera
	float speed = 25.0F; // The speed of snapping the camera to a location
	float startTime; // The starting time of the camera's movement
	float journeyLength; // The straight-line distance to the destination

	public void StartMe() {
		SetPublicVariables();
		movController = GetComponentInParent<MovementController>();
		actController = ContextAwareBox.GetComponent<ActivationTileController>();
		ResetOffset();
		initialRotation = transform.rotation;

	}

	void SetPublicVariables() {
		leftWing = GameObject.FindGameObjectWithTag("Left Wing").GetComponent<Image>();
		rightWing = GameObject.FindGameObjectWithTag("Right Wing").GetComponent<Image>();
		leftGun = GameObject.FindGameObjectWithTag("Left Gun").GetComponent<Image>();
		rightGun = GameObject.FindGameObjectWithTag("Right Gun").GetComponent<Image>();
		power = GameObject.FindGameObjectWithTag("Power").GetComponent<Image>();
		quarters = GameObject.FindGameObjectWithTag("Quarters").GetComponent<Image>();
		cargo = GameObject.FindGameObjectWithTag("Cargo").GetComponent<Image>();
		bridge = GameObject.FindGameObjectWithTag("Bridge").GetComponent<Image>();

		ZoomSpeed = 500f;
		RotationSpeed = 180f;
		GameManagerObject = Object.FindObjectOfType<GameManager>().gameObject;
		ContextAwareBox = Object.FindObjectOfType<ContextAwareBox>().gameObject;
		TileHighlight = Resources.Load("Highlighted Tile 2") as GameObject;
	}

	void Update() {
		if (destination != null) { // We are snapping the camera to a location
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			Vector3 dest = Tile.TileMiddle(destination);
			dest.y = transform.position.y;
			transform.position = Vector3.Lerp(start, dest, fracJourney);
			if (Vector3.Distance(transform.position, dest) < 2f) {
				locked = false;
				destination = null;
			}
		}

		/* We are just moving the camera normally */

		/* Rectangles for camera movement */
		Rect recdown = new Rect(0, 0, Screen.width, GUISize);
		Rect recup = new Rect(0, Screen.height - GUISize, Screen.width, GUISize);
		Rect recleft = new Rect(0, 0, GUISize, Screen.height);
		Rect recright = new Rect(Screen.width - GUISize, 0, GUISize, Screen.height);

		if (locked) // Destroy the AP Counter
			Destroy(gameObject.transform.parent.GetComponentInChildren<MovementController>().Counter);

		if (!locked) { /* Player is moving the camera */
			if (Input.GetKey("space")) { // Snap the camera to the player
				Destroy(gameObject.transform.parent.GetComponentInChildren<MovementController>().Counter);
				ResetCamera();
			}
			/* Player is probably panning, rotating or zooming the camera */
			float zoom = -Input.GetAxis("Mouse ScrollWheel"); // The amount of zoom
			Vector3 pos = transform.position; // Current position of the camera
			pos.y += zoom * Time.deltaTime * ZoomSpeed;
			pos.y = Mathf.Clamp(pos.y, MIN_Y, MAX_Y + 0.01f);
			transform.position = pos;

			/*
			 * Change to map if zoomed out too far
			 */
			if (transform.position.y >= MAX_Y && !Application.loadedLevelName.Equals("Tutorial")) {
				switch (Location) {
				case Locations.BRDIGE:
					bridge.enabled = true;
					break;
				case Locations.R_GUN:
					rightGun.enabled = true;
					break;
				case Locations.L_GUN:
					leftGun.enabled = true;
					break;
				case Locations.R_WING:
					rightWing.enabled = true;
					break;
				case Locations.L_WING:
					leftWing.enabled = true;
					break;
				case Locations.QUARTERS:
					quarters.enabled = true;
					break;
				case Locations.CARGO_BAY:
					cargo.enabled = true;
					break;
				case Locations.POWER:
					power.enabled = true;
					break;
				}
			} else { // Hide the map
				if (leftWing != null)
					leftWing.enabled = false;
				if (rightWing != null)
					rightWing.enabled = false;
				if (leftGun != null)
					leftGun.enabled = false;
				if (rightGun != null)
					rightGun.enabled = false;
				if (power != null)
					power.enabled = false;
				if (quarters != null)
					quarters.enabled = false;
				if (cargo != null)
					cargo.enabled = false;
				if (bridge != null)
					bridge.enabled = false;
			}

			float rotation = Input.GetAxis("Rotate"); // The amount to rotate
			transform.RotateAround(Target.position, Vector3.up, 
			                       Time.deltaTime * rotation * RotationSpeed);

			if (Input.GetKey("s") || recdown.Contains(Input.mousePosition)) { // Pan the camera downwards
				Destroy(gameObject.transform.parent.GetComponentInChildren<MovementController>().Counter);
				Vector3 direction = (transform.position - Target.position);
				direction.y = 0;
				direction.Normalize();
				transform.Translate(direction * CamSpeed, Space.World);
			}
			
			if (Input.GetKey("w") || recup.Contains(Input.mousePosition)) { // Pan the camera up
				Destroy(gameObject.transform.parent.GetComponentInChildren<MovementController>().Counter);
				Vector3 direction = -(transform.position - Target.position);
				direction.y = 0;
				direction.Normalize();
				transform.Translate(direction * CamSpeed, Space.World);
			}
			
			if (Input.GetKey("a") || recleft.Contains(Input.mousePosition)) { // Pan the camera left
				Destroy(gameObject.transform.parent.GetComponentInChildren<MovementController>().Counter);
				Vector3 direction = (transform.position - Target.position);
				float z = direction.z;
				direction.z = direction.x;
				direction.x = -z;
				direction = -direction;
				direction.y = 0;
				direction.Normalize();
				transform.Translate(direction * CamSpeed, Space.World);
			}
			
			if (Input.GetKey("d") || recright.Contains(Input.mousePosition)) { // Pan the camera right
				Destroy(gameObject.transform.parent.GetComponentInChildren<MovementController>().Counter);
				Vector3 direction = (transform.position - Target.position);
				float z = direction.z;
				direction.z = direction.x;
				direction.x = -z;
				direction.y = 0;
				direction.Normalize();
				transform.Translate(direction * CamSpeed, Space.World);
			}

			Clamp();
		}

		/* The user clicked on something */
		if (Input.GetMouseButtonUp(0) && !GetComponentInParent<Player>().IsPlayerNoLongerActive()) {
			if (!EventSystem.current.IsPointerOverGameObject()) { // Mouse is not over a game object. Attempt to move
				Tile goal = Tile.MouseToTile((LayerMask));
				if (actController.ActivationTiles().Contains(goal)) {
					movController.ClearPath();

					if (!IsTargetConfirmation) { // The user is activating an item/ability. Need to confirm target
						actController.InitiateTargetConfirmation(goal); // Confirm target
						IsTargetConfirmation = true;
					} else { // Activate the item/ability
						actController.Activate(goal); // Activate the item
						IsTargetConfirmation = false;
					}

				} else if (goal != null) { // Stop item/ability activation
					actController.DestroyActivationTiles(); // Stop targetting
					IsTargetConfirmation = false;
					movController.RequestMovement(goal);
				}
			}
		}
	}

	/**
	 * Set the camera's position in between two values
	 */
	void Clamp() {
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
		                                 transform.position.y,
		                                 Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
	}

	/*
	 * Change the offset for the camera
	 */ 
	void ResetOffset() {
		offset = transform.position - transform.parent.position;
		offset = new Vector3(0, offset.y, offset.z);
	}

	/**
	 * Lock the camera
	 */
	public void LockCamera() {
		locked = true;
	}

	/**
	 * Unlock the camera
	 */
	public void UnlockCamera() {
		locked = false;
	}

	/*
	 * Reset the camera
	 */
	public void ResetCamera() {
		transform.position = transform.parent.position + offset;
		transform.rotation = initialRotation;
	}

	/**
	 * Moves the camera to the given location.
	 * 
	 * Arguments
	 * - Tile location - The location to move the camera to
	 */
	public void MoveCamera(Tile location) {
		if (!locked) {
			Vector3 dest = Tile.TileMiddle(location);
			dest.y = transform.position.y;
			destination = location;
			locked = true;
			start = transform.position;
			startTime = Time.time;
			journeyLength = Vector3.Distance(start, Tile.TileMiddle(destination));
		}
	}

	/**
	 * Generate a highlighted tile at the given position
	 * 
	 * Arguments
	 * - Tile pos - The position to generate the higlighted tile on
	 */
	public void HighlightTile(Tile pos) {
		Vector3 tilePos = Tile.TileMiddle(pos);
		tilePos.y = tilePos.y - 0.49f;
		Quaternion tileRot = Quaternion.identity;
		Instantiate(TileHighlight, tilePos, tileRot);
	}
}
