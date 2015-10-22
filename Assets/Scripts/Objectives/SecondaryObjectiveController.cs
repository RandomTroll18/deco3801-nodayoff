using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SecondaryObjectiveController : MonoBehaviour {
	public GameObject LocationButton1;
	public GameObject LocationButton2;

	int page;
	Button nextButton;
	Button prevButton;
	List<SecondaryObjective> objectives = new List<SecondaryObjective>();
	const int START_PAGE = 0;
	GameObject panel1;
	GameObject panel2;

	void Start() {
//		nextButton = GameObject.Find("Next Objectives Button").GetComponent<Button>();
//		prevButton = GameObject.Find("Previous Objectives Button").GetComponent<Button>();
//		panel1 = GameObject.Find("Secondary 1");
//		panel2 = GameObject.Find("Secondary 2");
	}

	void Update() {

	}

	/*
	 * You can think of this as being the Start function
	 */
	void OnEnable() {

		nextButton = GameObject.Find("Next Objectives Button").GetComponent<Button>();
		prevButton = GameObject.Find("Previous Objectives Button").GetComponent<Button>();
		panel1 = GameObject.Find("Secondary 1");
		panel2 = GameObject.Find("Secondary 2");

		Transform playerSecondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives");
		objectives.Clear();
		page = START_PAGE;
		prevButton.enabled = false;
		if (objectives.Count < 2) {
			nextButton.enabled = true;
		}
		foreach (SecondaryObjective obj in playerSecondaries.GetComponents<SecondaryObjective>()) {
			objectives.Add(obj);
		}

		PopulatePanels();
	}

	void PopulatePanels() {
		for (int i = page * 2; i <= page * 2 + 1 && i < objectives.Count; i++) {
			// figure out which panel this is
			GameObject panel = i % 2 == 0 ? panel1 : panel2;

			// set panel title, description and goto button using objectives[i]
			panel.transform.Find("Title").GetComponent<Text>().text = objectives[i].Title;
			panel.transform.Find("Description").GetComponent<Text>().text = objectives[i].Description;
			GameObject button = panel == panel1 ? LocationButton1 : LocationButton2;
			if (objectives[i].Location == null) {
				button.gameObject.SetActive(false);
			} else {
				button.gameObject.SetActive(true);
			}
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

	void OnDisable() {
		page = 0;
		for (int i = page * 2; i <= page * 2 + 1 && i < objectives.Count; i++) {
			// figure out which panel this is
			GameObject panel = i % 2 == 0 ? panel1 : panel2;
			
			// set panel title, description and goto button using objectives[i]
			panel.transform.Find("Title").GetComponent<Text>().text = "";
			panel.transform.Find("Description").GetComponent<Text>().text = "";
			GameObject button = panel == panel1 ? LocationButton1 : LocationButton2;
			button.gameObject.SetActive(false);
		}
	}

	public void GoToLocation(int panel) {
		CameraController cam = Camera.main.GetComponent<CameraController>();
		cam.MoveCamera(objectives[panel + page * 2].Location);
		cam.HighlightTile(objectives[panel + page * 2].Location);
	}
}
