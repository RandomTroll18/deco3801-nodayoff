using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrimaryObjectiveController : MonoBehaviour {

	// Use this for initialization
	void Start() {
		ChangeObjective(null); // Needs to be changed
	}

	/**
	 * Call this when the objective needs to change
	 */
	public void ChangeObjective(Objective objective) {
		transform.Find("Title").GetComponent<Text>().text = "ANOTHER TITLE"; // Needs to be changed
	}
}
