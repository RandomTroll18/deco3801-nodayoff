using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrimaryObjectiveController : MonoBehaviour {
	Objective objective;
	CameraController cam;

	// Use this for initialization
	public void StartMe() {
		Objective firstObjective = null;
		switch (Application.loadedLevelName) {
		case "Main Level":
			firstObjective = new FirstObjectiveMain();
			break;
		case "Level":
			firstObjective = new FirstObjective();
			break;
		default:
			Debug.LogError("Scene name is unknown");
			break;
		}
		ChangeObjective(firstObjective);
		cam = Camera.main.GetComponent<CameraController>();
	}

	/**
	 * Call this when the objective needs to change
	 */
	public void ChangeObjective(Objective newObjective) {
		this.objective = newObjective;
		transform.Find("Title").GetComponent<Text>().text = objective.Title;
		transform.Find("Description").GetComponent<Text>().text = objective.Description;

	}

	public void OnComplete() {
		GetComponent<PhotonView>().RPC("OnCompleteNetwork", PhotonTargets.All, null);
	}

	[PunRPC]
	void OnCompleteNetwork() {
		objective.OnComplete();
	}

	public Objective GetObjective() {
		return objective;
		
	}

	public void GoToLocation() {
		cam.MoveCamera(objective.Location);
		cam.HighlightTile(objective.Location);
	}
}
