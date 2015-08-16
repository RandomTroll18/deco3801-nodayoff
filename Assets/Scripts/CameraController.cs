using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public GameObject player;
	public float camSpeed;
	// The distance the mouse pointer needs to be from the edge before the screen moves.
	public float GUISize;
	public LayerMask layerMask;
	
	private Vector3 offset;
	
	void Start () {
		offset = transform.position - player.transform.position;
	}

	void Update() {
		// Camera panning with mouse
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

		// Mouse click detection
		if (Input.GetMouseButtonUp (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
				Debug.Log (hit.point);
				float x = Mathf.Ceil ((hit.point.x - 1) / 2);
				float z = Mathf.Ceil ((hit.point.z - 1) / 2);
				Debug.Log (x + " " + z);
			}
		}
	}

	/*
	 * Call this when the player moves.
	 */
	public void ResetCamera() {
		transform.position = player.transform.position + offset;
	}
}
