using UnityEngine;

public class MapCollisionDetector : MonoBehaviour {

	Player player; // The current player we are looking at
	bool colliding; // Record if we are colliding with something

	void Start() {
		GameObject playerObject = GameObject.FindWithTag("Player");
		if (player == null) // Found a player
			player = playerObject.GetComponent<Player>();
	}

	/**
	 * Handle collision with other objects
	 * 
	 * Arguments
	 * - Collider collidedObject - The collider of the object we collided with
	 */
	void OnTriggerEnter(Collider collidedObject) {
		if (collidedObject.CompareTag("Blocker")) // Collided with a blocking object.
			colliding = true;
	}

	/**
	 * Handle exiting out of a collision with other objects
	 * 
	 * Arguments
	 * - Collider collidedObject - The collider of the object we recently collided with
	 */
	void OnTriggerExit (Collider collidedObject) {
		if (collidedObject.CompareTag("Blocker")) // Collided with a blocking object
			colliding = false;
	}

	/**
	 * Return whether or not we are colliding with an object
	 * 
	 * Returns
	 * - true if we are colliding with a blocking object. false otherwise
	 */
	public bool IsColliding() {
		return colliding;
	}
}
