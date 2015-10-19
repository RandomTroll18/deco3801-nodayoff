using UnityEngine;

public class MainCanvasButton : MonoBehaviour {
	
	public GameObject Text1;
	public GameObject Text2;
	public GameObject Text3;
	public GameObject Text4;
	public GameObject Text5;
	public GameObject Text6;
	public GameObject TutorialMenu;
	public GameObject EscMenu;
	public GameObject SettingMenu;
	/* public GameObject text7;
	public GameObject text8;
	public GameObject testText; */

	/* The states for camera controllers */
	bool playerCameraControllerState;
	bool spawnedCameraControllerState;


	// Use this for initialization
	public void StartMe() {
		//EscMenu.SetActive (false);
		hideAll();
		ChangeTab(1);
	}

	void hideAll() {

		Text1.SetActive(false);
		Text2.SetActive(false);
		Text3.SetActive(false);
		Text4.SetActive(false);
		Text5.SetActive(false);
		Text6.SetActive(false);

		/*
		text7.SetActive (false);
		text8.SetActive (false);
		*/
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Back();
		}
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

		if (!enable) { // Pausing. Save state of camera controllers and set them to inactive
			playerCameraControllerState = playerObject.GetComponentInChildren<CameraController>().enabled;
			playerObject.GetComponentInChildren<CameraController>().enabled = enable;
			if (spawnedCharacter != null) { // There is a spawned character
				spawnedCameraControllerState = spawnedCharacter.GetComponentInChildren<CameraController>().enabled;
				spawnedCharacter.GetComponentInChildren<CameraController>().enabled = enable;
			}
				
		} else { // Resuming. Restore state
			playerObject.GetComponentInChildren<CameraController>().enabled = playerCameraControllerState;
			if (spawnedCharacter != null) { // There is a spawned character
				spawnedCharacter.GetComponentInChildren<CameraController>().enabled = spawnedCameraControllerState;
			}
		}
	}

	public void Back() {
		if (EscMenu.activeInHierarchy){
			Toggle(EscMenu);
			toggleCameraControllers(true);
		} else {
			toggleCameraControllers(false);
			if (TutorialMenu.activeInHierarchy) {
				Toggle(TutorialMenu);
				ChangeTab(1);
				Toggle(EscMenu);  //Re-open Esc
			} else if(SettingMenu.activeInHierarchy) {
				Toggle(SettingMenu);
				Toggle(EscMenu); //Re-open Esc
			} else {
				Toggle(EscMenu);
			}
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
