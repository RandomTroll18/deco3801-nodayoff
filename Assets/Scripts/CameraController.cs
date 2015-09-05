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

		 movController.UnblockTile(new Tile(-1, -8));
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
			Tile goal = Tile.MouseToTile((LayerMask));
			Debug.Log("Clicked: " + goal.ToString());
			if (goal != null)
				movController.RequestMovement(goal);
		}
	}

	/*
	 * Call this when the player moves.
	 */
	public void ResetCamera() {
		transform.position = Player.transform.position + offset;
	}
}
