using UnityEngine;

public class MapCollisionDetector : MonoBehaviour {

	Player player;
	bool colliding;

	void Start() {
		GameObject playeObject = GameObject.FindWithTag ("Player");
		if (player == null) {
			player = playeObject.GetComponent <Player>();
		}
		if (player == null) {
			Debug.Log ("Cannot find Player script");
		}
	}
	
	void OnTriggerEnter (Collider collider) {
		if (collider.CompareTag ("Blocker"))
			colliding = true;
	}

	void OnTriggerExit (Collider collider) {
		if (collider.CompareTag ("Blocker"))
			colliding = false;
	}

	public bool IsColliding() {
		return colliding;
	}
}
