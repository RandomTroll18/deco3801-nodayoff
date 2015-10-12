using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SecondaryObjectiveController : MonoBehaviour {
	int page;
	Button nextButton;
	Button prevButton;
	List<SecondaryObjective> objectives = new List<SecondaryObjective>();
	const int START_PAGE = 0;

	void Start() {
		nextButton = GameObject.Find("Next Objectives Button").GetComponent<Button>();
		prevButton = GameObject.Find("Previous Objectives Button").GetComponent<Button>();
	}

	/*
	 * You can think of this as being the Start function
	 */
	void OnEnable() {
		Transform playerSecondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives");
		objectives.Clear();
		page = START_PAGE;
		prevButton.enabled = false;
		foreach (SecondaryObjective obj in playerSecondaries.GetComponents<SecondaryObjective>()) {
			objectives.Add(obj);
		}

		PopulatePanels();
	}

	void PopulatePanels() {
		for (int i = page * 2; i <= page * 2 + 1 && objectives.Count <= i; i++) {
			// figure out which panel this is
			// set panel title, description and goto button using objectives[i]
		}
	}

	/*
	 * Preconditions:
	 * - direction is either +1 or -1
	 */
	public void ChangePage(int direction) {
		page += direction;

		PopulatePanels();

		if (page > START_PAGE) {
			prevButton.enabled = true;
		} else {
			prevButton.enabled = false;
		}
		if (Mathf.Ceil(objectives.Count / 2.0f) - 1 == page) {
			nextButton.enabled = false;
		} else {
			nextButton.enabled = true;
		}
	}

	// TODO: close button
}
