using UnityEngine;
using System.Collections;

public class DoorLight : MonoBehaviour {

	Light doorLight;
	Animator animator;
	LockDoor doorLock;

	void Start () {
		doorLock = GetComponent<LockDoor>();
		doorLight = GetComponentInChildren<Light>();
		animator = GetComponent<Animator>();
		if (doorLight == null)
			Debug.LogError(gameObject.name + " has no door light!");
	}
	
	void Update () {
		if (doorLight == null)
			return; // No door light

		if (!animator.enabled) {
			if (doorLock == null) {
				doorLight.color = Color.red;
				return;
			}

			switch (doorLock.ClassMultiplier) {
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
			case Stat.SCOUTMULTIPLIER:
				doorLight.color = Color.yellow;
				break;
			default:
				Debug.LogWarning("Door doesn't have lock component and animation is disabled OR " +
					"multiplier is wrong");
				doorLight.color = Color.red;
				break;
			}
		} else {
			doorLight.color = Color.green;
		}
	}
}
