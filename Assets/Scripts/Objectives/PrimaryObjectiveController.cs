using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrimaryObjectiveController : MonoBehaviour {
	public GameObject HumanObjectivePanel; // Human objective panel for the alien
	Objective objective;
	CameraController cam;

	// Use this for initialization
	public void StartMe() {
		Objective firstObjective = null;
		switch (Application.loadedLevelName) {
		case "Main Level":
			Debug.Log("Load Main Level 1st Objective");
			firstObjective = Object.FindObjectOfType<FirstObjectiveMain>();
			break;
		case "Level":
			Debug.Log("Load Test Level 1st Objective");
			firstObjective = new FirstObjective();
			break;
		case "Tutorial":
			Debug.Log("Load Tutorial 1st Objective");
			firstObjective = Object.FindObjectOfType<TutorialFirstObjective>();
			break;
		case "TestScene2": goto case "Level";
		default:
			Debug.LogError("Scene name is unknown");
			break;
		}
		firstObjective.InitializeObjective();
		ChangeObjective(firstObjective);
		cam = Camera.main.GetComponent<CameraController>();
	}

	/**
	 * Call this when the objective needs to change
	 */
	public void ChangeObjective(Objective newObjective) {
		objective = newObjective;

		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) { // Alien
			Debug.Log("Setting text of alien objectives");
			transform.Find("Title").GetComponent<Text>().text = "Destroy The Ship";
			transform.Find("Description").GetComponent<Text>().text = "No one must escape. Your " +
				"sacrifice will not be in vain";
		} else { // Human
			Debug.Log("Setting text of human objectives");
			Debug.Log("Human objective title: " + objective.Title);
			Debug.Log("Human objective desc.: " + objective.Description);
			transform.Find("Title").GetComponent<Text>().text = objective.Title;
			transform.Find("Description").GetComponent<Text>().text = objective.Description;
		}

		Debug.Log("Objective title: " + transform.Find("Title").GetComponent<Text>().text);
		Debug.Log("Objective desc: " + transform.Find("Description").GetComponent<Text>().text);

		if (objective.Location == null)
			DeactivateLocationButton();

		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) { // Alien. Indicate primary human objective
			HumanObjectivePanel.SetActive(true);
			HumanObjectivePanel.transform.Find("Title").GetComponent<Text>().text = "Human Objective: " + objective.Title;
			HumanObjectivePanel.transform.Find("Description").GetComponent<Text>().text = objective.Description;
		}
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
