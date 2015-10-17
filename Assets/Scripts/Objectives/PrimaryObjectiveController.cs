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
			firstObjective = Object.FindObjectOfType<FirstObjectiveMain>();
			break;
		case "Level":
			firstObjective = new FirstObjective();
			break;
		case "Tutorial":
			firstObjective = Object.FindObjectOfType<TutorialFirstObjective>();
			break;
		case "TestScene2": goto case "Level";
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
		objective = newObjective;

		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) { // Alien
			transform.Find("Title").GetComponent<Text>().text = "Destroy The Ship";
			transform.Find("Description").GetComponent<Text>().text = "No one must escape. Your " +
				"sacrifice will not be in vain";
		} else { // Human
			transform.Find("Title").GetComponent<Text>().text = objective.Title;
			transform.Find("Description").GetComponent<Text>().text = objective.Description;
		}

		if (objective.Location == null || PhotonNetwork.player.GetTeam() == PunTeams.Team.red)
			DeactivateLocationButton();

	}

	public void OnComplete() {
		GetComponent<PhotonView>().RPC("OnCompleteNetwork", PhotonTargets.All, null);
	}

	void DeactivateLocationButton() {
		GetComponentInChildren<Button>().gameObject.SetActive(false);
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
