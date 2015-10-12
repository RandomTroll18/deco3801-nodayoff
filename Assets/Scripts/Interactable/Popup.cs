using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

	public GameObject MainPanel;
	public int CurrentPanel;
	public GameObject[] Panels;
	//panels = new GameObject[totalPanel];


	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void NextPanel() {
		Panels[CurrentPanel].SetActive(false);
		CurrentPanel++;
		if (CurrentPanel < Panels.Length) {
			Panels[CurrentPanel].SetActive(true);
		}
	}

	public void ToggleVisibility() {
		
	}
}
