using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public GameObject GameManagerObject; // The game manager object
	public GameObject ContextAwareBox; // The context aware box
	public GameObject TileHighlight;
	GameManager gameManagerScript; // The game manager script
	public float RotationSpeed;
	public Transform Target;
	public float ZoomSpeed;
	public Locations Location;
	public float CamSpeed;
	/* The distance the mouse pointer needs to be from the edge before the 
	 * screen moves.
	 */
	public float GUISize;
	public LayerMask LayerMask;

	Image leftWing;
	Image rightWing;
	Image leftGun;
	Image rightGun;
	Image power;
	Image quarters;
	Image cargo;
	Image bridge;
	const float MAX_Z = 120f;
	const float MIN_Z = -55f;
	const float MAX_X = 86f;
	const float MIN_X = -75f;
	const float MAX_Y = 25f;
	const float MIN_Y = 12.25f;
	Vector3 offset;
	//Player playerScript;
	MovementController movController;
	ActivationTileController actController; // Activation Controller script
	/* Whether the player can control the camera */
	bool locked = false;
	bool IsTargetConfirmation = false; // Record if we are just confirming our target
	Quaternion initialRotation;

	Tile destination = null;
	Vector3 start;
	float speed = 25.0F;
	float startTime;
	float journeyLength;

	public void StartMe() {
		SetPublicVariables();
		//playerScript = Player.GetComponent<Player>();
		movController = GetComponentInParent<MovementController>();
		actController = ContextAwareBox.GetComponent<ActivationTileController>();

		ResetOffset();
		//this.gameManagerScript = this.gameManagerObject.GetComponent<GameManager>();

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
		if (destination != null) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			Vector3 dest = Tile.TileMiddle(destination);
			dest.y = transform.position.y;
			transform.position = Vector3.Lerp(start, dest, fracJourney);
			if (transform.position.Equals(dest)) {
				locked = false;
				destination = null;
			}
		}

		// Camera moving with mouse
		Rect recdown = new Rect(0, 0, Screen.width, GUISize);
		Rect recup = new Rect(0, Screen.height - GUISize, Screen.width, GUISize);
		Rect recleft = new Rect(0, 0, GUISize, Screen.height);
		Rect recright = new Rect(Screen.width - GUISize, 0, GUISize, Screen.height);

		/* Camera controlling input */
		if (!locked) {
			if (Input.GetKey("space")) {
				ResetCamera();
			}

			// Camera zoom
			float zoom = -Input.GetAxis("Mouse ScrollWheel");
			Vector3 pos = transform.position;
			pos.y += zoom * Time.deltaTime * ZoomSpeed;
			pos.y = Mathf.Clamp(pos.y, MIN_Y, MAX_Y + 0.01f);
			transform.position = pos;
			

			/*
			 * Change to map if zoomed out too far
			 */
			if (transform.position.y >= MAX_Y) {
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
			} else {
				leftWing.enabled = false;
				rightWing.enabled = false;
				leftGun.enabled = false;
				rightGun.enabled = false;
				power.enabled = false;
				quarters.enabled = false;
				cargo.enabled = false;
				bridge.enabled = false;
			}

			/*
			 * Don't let curious people zoom in too close
			 */

			float rotation = Input.GetAxis("Rotate");
			transform.RotateAround(Target.position, Vector3.up, 
			                       Time.deltaTime * rotation * RotationSpeed);

			if (Input.GetKey("s") || recdown.Contains(Input.mousePosition)) {
				Vector3 direction = (transform.position - Target.position);
				direction.y = 0;
				direction.Normalize();

//				Debug.Log("Direction pressing s: " + direction.ToString());
//					Debug.Log(transform.position.z);
				transform.Translate(direction * CamSpeed, Space.World);
			}
			
			if (Input.GetKey("w") || recup.Contains(Input.mousePosition)) {
				Vector3 direction = -(transform.position - Target.position);
				direction.y = 0;
				direction.Normalize();

//				Debug.Log("Direction pressing w: " + direction.ToString());
//					Debug.Log(transform.position.z);
				transform.Translate(direction * CamSpeed, Space.World);
			}
			
			if (Input.GetKey("a") || recleft.Contains(Input.mousePosition)) {
				Vector3 direction = (transform.position - Target.position);
				float z = direction.z;
				direction.z = direction.x;
				direction.x = -z;
				direction = -direction;
				direction.y = 0;
				direction.Normalize();

//				Debug.Log("Direction pressing a: " + direction.ToString());
//				Debug.Log(transform.position.x);
				transform.Translate(direction * CamSpeed, Space.World);
			}
			
			if (Input.GetKey("d") || recright.Contains(Input.mousePosition)) {
				Vector3 direction = (transform.position - Target.position);
				float z = direction.z;
				direction.z = direction.x;
				direction.x = -z;
				direction.y = 0;
				direction.Normalize();

//				Debug.Log("Direction pressing d: " + direction.ToString());
//					Debug.Log(transform.position.x);
				transform.Translate(direction * CamSpeed, Space.World);
			}

			Clamp();
		}


		// Mouse click detection
		if (Input.GetMouseButtonUp(0) && !GetComponentInParent<Player>().IsPlayerNoLongerActive()) {
			if (!EventSystem.current.IsPointerOverGameObject()) {
				Tile goal = Tile.MouseToTile((LayerMask));
//				Debug.Log("Clicked tile at: " + Tile.TileMiddle(goal).ToString());
				if (actController.ActivationTiles().Contains(goal)) {
					movController.ClearPath();

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

	void Clamp() {
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
		                                 transform.position.y,
		                                 Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
	}

	/*
	 * This needs to be called since Player can change in the middle of a 
	 * game
	 */ 
	void ResetOffset() {
		offset = transform.position - transform.parent.position;
		offset = new Vector3(0, offset.y, offset.z);
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
		transform.position = transform.parent.position + offset;
		transform.rotation = initialRotation;
	}

	/**
	 * Moves the camera to the given location.
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


	public void HighlightTile(Tile pos) {
		Vector3 tilePos = Tile.TileMiddle(pos);
		tilePos.y = tilePos.y - 0.49f;
		Quaternion tileRot = Quaternion.identity;
		Instantiate(TileHighlight, tilePos, tileRot);
	}
}
