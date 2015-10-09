using UnityEngine;
using System.Collections;

public class DoorLight : MonoBehaviour {

	Light doorLight;
	Animator animator;

	void Start () {
		doorLight = GetComponentInChildren<Light>();
		animator = GetComponent<Animator>();
		if (doorLight == null)
			Debug.LogError(gameObject.name + " has no door light!");
	}
	
	void Update () {
		if (doorLight == null)
			return; // No door light

		if (!animator.enabled) {
			doorLight.color = Color.red;
		} else {
			doorLight.color = Color.green;
		}
	}
}
