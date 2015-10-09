using UnityEngine;
using System.Collections;

public class DoorLight : MonoBehaviour {

	Light doorLight;
	Animator animator;

	void Start () {
		doorLight = GetComponentInChildren<Light>();
		animator = GetComponent<Animator>();
	}
	
	void Update () {
		if (!animator.enabled) {
			doorLight.color = Color.red;
		} else {
			doorLight.color = Color.green;
		}
	}
}
