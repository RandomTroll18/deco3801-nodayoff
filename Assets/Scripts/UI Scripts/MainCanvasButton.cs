﻿using UnityEngine;

public class MainCanvasButton : MonoBehaviour {

	/* Game objects for various buttons and texts */
	public GameObject Text1;
	public GameObject Text2;
	public GameObject Text3;
	public GameObject Text4;
	public GameObject Text5;
	public GameObject Text6;
	public GameObject Text7;
	public GameObject TutorialMenu;
	public GameObject EscMenu;
	public GameObject SettingMenu;

	/* The states for camera controllers */
	bool playerCameraControllerState;
	bool spawnedCameraControllerState;
	bool stateSaved; // Record if state was saved


	/**
	 * Initialize this object
	 */
	public void StartMe() {
		hideAll();
		ChangeTab(1);
	}

	/**
	 * Hide all game objects
	 */
	void hideAll() {
		Text1.SetActive(false);
		Text2.SetActive(false);
		Text3.SetActive(false);
		Text4.SetActive(false);
		Text5.SetActive(false);
		Text6.SetActive(false);
		Text7.SetActive(false);
	}
	

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) // Escape was pressed. Determine what action to do
			Back();
	}

	/**
	 * Enable/disable camera controller of controllable characters
	 * 
	 * Arguments
	 * - bool enable - The flag for enabling/disabling camera controllers
	 */
	void toggleCameraControllers(bool enable) {
		GameObject playerObject = Player.MyPlayer; // The game object of the player
		GameObject spawnedCharacter = null; // The spawned character
		Player playerScript = Player.MyPlayer.GetComponent<Player>(); // The player script

		/* Get the player's spawned character, if it exists */
		switch (playerScript.GetPlayerClassObject().GetClassTypeEnum()) {
		case Classes.ENGINEER: // Engineer robot
			spawnedCharacter = GameObject.FindGameObjectWithTag("EngineerPrimAbilitySpawn");
			break;
		default: // No spawn
			break;
		}

		if (!enable && !stateSaved) { // Pausing. Save state of camera controllers and set them to inactive
			playerCameraControllerState = playerObject.GetComponentInChildren<CameraController>().enabled;
			playerObject.GetComponentInChildren<CameraController>().enabled = enable;
			if (spawnedCharacter != null) { // There is a spawned character
				spawnedCameraControllerState = spawnedCharacter.GetComponentInChildren<CameraController>().enabled;
				spawnedCharacter.GetComponentInChildren<CameraController>().enabled = enable;
			}
			stateSaved = true;	
		} else { // Resuming. Restore state
			playerObject.GetComponentInChildren<CameraController>().enabled = playerCameraControllerState;
			if (spawnedCharacter != null) { // There is a spawned character
				spawnedCharacter.GetComponentInChildren<CameraController>().enabled = spawnedCameraControllerState;
			}
			stateSaved = false;
		}
	}

	/**
	 * Toggle menus depending on state
	 */
	public void Back() {
		if (EscMenu.activeInHierarchy){ // Esc Menu is active. Toggle it
			Toggle(EscMenu);
			toggleCameraControllers(true);
		} else { // It's not active, need to determine what menu it is
			toggleCameraControllers(false);
			if (TutorialMenu.activeInHierarchy) { // Tutorial menu is currently open
				Toggle(TutorialMenu);
				ChangeTab(1);
				Toggle(EscMenu);  // Re-open Esc
			} else if(SettingMenu.activeInHierarchy) { // Sound settings menu is currently open
				Toggle(SettingMenu);
				Toggle(EscMenu); // Re-open Esc
			} else // Just re-open esc menu
				Toggle(EscMenu);
		}
	}

	public void ChangeTab(int tabNumber) {
		hideAll ();
		if (tabNumber == 1) {
			Text1.SetActive (true);
		} 
		else if (tabNumber == 2) {
			Text2.SetActive (true);
		} 
		else if (tabNumber == 3) {
			Text3.SetActive (true);
		} 
		else if (tabNumber == 4) {
			Text4.SetActive (true);
		} 
		else if (tabNumber == 5) {
			Text5.SetActive (true);
		} 
		else if (tabNumber == 6) {
			Text6.SetActive (true);
		} 
		else if (tabNumber == 7) {
			Text7.SetActive (true);
		} 


		/*testText.text = currentTab.ToString; */

	}

	/*
	public void ToggleTutorialMenu(){
		if (TutorialMenu.activeInHierarchy) {
		TutorialMenu.SetActive (false);
			EscMenu.SetActive(true);
		} else {
			ChangeTab(1);
			TutorialMenu.SetActive(true);
			EscMenu.SetActive(false);
		}
	}

	public void ToggleEscMenu(){
		if (EscMenu.activeInHierarchy) {
			EscMenu.SetActive(false);
		} else {
			EscMenu.SetActive(true);
		}
	}
	 */

	public void Toggle(GameObject target) {
		if (target != EscMenu) {
			EscMenu.SetActive(false);
		}

		if (target.activeInHierarchy) {
			target.SetActive(false);
		} else {
			target.SetActive(true);
		}

	}

	public void DefaultSettingButton(GameObject target1, GameObject target2, GameObject target3) {
		
	}

	public void FullGameQuit(){
		Application.Quit ();
	}

	public void LoadLevel(string level) {
		Application.LoadLevel(level);
	}
	

}
