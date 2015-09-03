using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

	public GameObject GameManagerObject; // The game manager object
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

	void Start() {
		//playerScript = Player.GetComponent<Player>();
		movController = Player.GetComponent<MovementController>();

		offset = transform.position - Player.transform.position;
		//this.gameManagerScript = this.gameManagerObject.GetComponent<GameManager>();
	}

	void Update() {
		// Camera panning with mouse
		Rect recdown = new Rect(0, 0, Screen.width, GUISize);
		Rect recup = new Rect(0, Screen.height - GUISize, Screen.width, GUISize);
		Rect recleft = new Rect(0, 0, GUISize, Screen.height);
		Rect recright = new Rect(Screen.width - GUISize, 0, GUISize, Screen.height);
		
		if (recdown.Contains(Input.mousePosition)) {
			transform.Translate(0, 0, -CamSpeed, Space.World);
		}
		
		if (recup.Contains(Input.mousePosition)) {
			transform.Translate(0, 0, CamSpeed, Space.World);
		}
		
		if (recleft.Contains(Input.mousePosition)) {
			transform.Translate(-CamSpeed, 0, 0, Space.World);
		}
		
		if (recright.Contains(Input.mousePosition)) {
			transform.Translate(CamSpeed, 0, 0, Space.World);
		}


		// Mouse click detection
		if (Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask) && !EventSystem.current.IsPointerOverGameObject()) {
				Tile goal;
				goal = new Tile();
				goal.X = Tile.TilePosition(hit.point.x);
				goal.Z = Tile.TilePosition(hit.point.z);
				Debug.Log(goal.X + " " + goal.Z);

				movController.RequestMovement(goal);
			}
		}
	}

	/*
	 * Call this when the player moves.
	 */
	public void ResetCamera() {
		transform.position = Player.transform.position + offset;
	}
}
