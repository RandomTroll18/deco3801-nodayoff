using UnityEngine;
using System.Collections;

public class DoorLight : MonoBehaviour {

	Light doorLight; // The light at the door
	Animator animator; // The animator for the door
	LockDoor doorLock; // The lock of the door

	void Start () {
		doorLock = GetComponent<LockDoor>();
		doorLight = GetComponentInChildren<Light>();
		animator = GetComponent<Animator>();
		if (doorLight == null) // No light at the door
			Debug.LogError(gameObject.name + " has no door light!");
	}
	
	void Update () {
		if (doorLight == null)
			return; // No door light

		if (!animator.enabled) { // The animator is not enabled. Locked
			if (doorLock == null) {
				doorLight.color = Color.red;
				return;
			}

			if (doorLock.Cost == 1000) // Just set the light to yellow for unopenable doors
				doorLight.color = Color.yellow;
			else {
				switch (doorLock.ClassMultiplier) { // Need to specify what class can efficiently open the door
				case Stat.NOMULTIPLIER:
					doorLight.color = Color.red;
					break;
				case Stat.ENGMULTIPLIER:
					doorLight.color = Color.blue;
					break;
				case Stat.MARINEMULTIPLIER:
					doorLight.color = Color.magenta;
					break;
				case Stat.TECHMULTIPLIER:
					doorLight.color = Color.gray;
					break;
				default:
					Debug.LogWarning("Door doesn't have lock component and animation is disabled OR " +
						"multiplier is wrong");
					doorLight.color = Color.red;
					break;
				}
			}
		} else // The door is open
			doorLight.color = Color.green;
	}
}
