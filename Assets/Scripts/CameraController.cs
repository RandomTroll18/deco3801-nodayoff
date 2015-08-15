using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public GameObject player;
	public float camSpeed;
	// The distance the mouse pointer needs to be from the edge before the screen moves.
	public float GUISize;
	
	private Vector3 offset;
	
	void Start () {
		offset = transform.position - player.transform.position;
	}

	void Update() {
		Rect recdown = new Rect (0, 0, Screen.width, GUISize);
		Rect recup = new Rect (0, Screen.height - GUISize, Screen.width, GUISize);
		Rect recleft = new Rect (0, 0, GUISize, Screen.height);
		Rect recright = new Rect (Screen.width - GUISize, 0, GUISize, Screen.height);
		
		if (recdown.Contains(Input.mousePosition))
			transform.Translate(0, 0, -camSpeed, Space.World);
		
		if (recup.Contains(Input.mousePosition))
			transform.Translate(0, 0, camSpeed, Space.World);
		
		if (recleft.Contains(Input.mousePosition))
			transform.Translate(-camSpeed, 0, 0, Space.World);
		
		if (recright.Contains(Input.mousePosition))
			transform.Translate(camSpeed, 0, 0, Space.World);
	}

	/*
	 * Call this when the player moves.
	 */
	public void ResetCamera() {
		transform.position = player.transform.position + offset;
	}
}
