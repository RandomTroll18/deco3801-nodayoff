using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	/*
	 * Function used to load a given level
	 * 
	 * Arguments
	 * - String level - The name of the scene to load
	 */
	public void LoadLevel(string level) {
		Application.LoadLevel(level);
	}

	/*
	 * Function used to quit the application
	 */
	public void Quit() {
		Application.Quit();
	}
}
