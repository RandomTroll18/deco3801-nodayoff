using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrimaryObjectiveController : MonoBehaviour {
	Objective objective;
	CameraController cam;

	// Use this for initialization
	public void StartMe() {
		ChangeObjective(new FirstObjective());
		cam = Camera.main.GetComponent<CameraController>();
	}

	/**
	 * Call this when the objective needs to change
	 */
	public void ChangeObjective(Objective objective) {
		this.objective = objective;
		transform.Find("Title").GetComponent<Text>().text = objective.Title;
		transform.Find("Description").GetComponent<Text>().text = objective.Description;

	}

	public void OnComplete() {
		objective.OnComplete();

	}

	public Objective GetObjective() {
		return objective;
		
	}

	public void GoToLocation() {
		cam.MoveCamera(objective.Location);
	}
}
