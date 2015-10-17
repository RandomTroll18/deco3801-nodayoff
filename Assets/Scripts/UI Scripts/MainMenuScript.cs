﻿using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	public static string LevelToLoad; // The level user wants to load

	/*
	 * Simply load the level selected
	 * 
	 * Arguments
	 * - string level - The level to load
	 */
	public void LoadLevel(string level) {
		if (level.Equals("MainMenu"))
			Player.ChosenClass = null; // Player has not chosen a class
		Application.LoadLevel(level);
	}

	/*
	 * Load class selection level
	 */
	public void LoadClassSelect() {
		Application.LoadLevel("ClassSelect");
	}

	/*
	 * Load the level the player wants to load with the given class
	 * 
	 * Arguments
	 * - string chosenClass - The class the player has chosen
	 */
	public void LoadLevelWithClass(string chosenClass) {
		Player.ChosenClass = chosenClass;
		if (LevelToLoad == null) {
			Debug.Log("No level to load");
			Application.LoadLevel("Main Level");
		} else { 
			Debug.Log("There is a level to load");
			Application.LoadLevel(LevelToLoad);
		}
	}


	/*
	 * Remember what the player wants to load
	 * 
	 * Arguments
	 * - The level the player wants to load
	 */
	public void RememberLevel(string level) {
		LevelToLoad = level;
	}

	/*
	 * Function used to quit the application
	 */
	public void Quit() {
		Application.Quit();
	}
}
