using UnityEngine;
using System.Collections;

public class MainCanvasButton : MonoBehaviour {
	
	public GameObject text1;
	public GameObject text2;
	public GameObject text3;
	public GameObject text4;
	public GameObject text5;
	public GameObject text6;
	public GameObject TutorialMenu;
	public GameObject EscMenu;
	public GameObject SettingMenu;
	/* public GameObject text7;
	public GameObject text8;
	public GameObject testText; */


	// Use this for initialization
	void Start () {
		//EscMenu.SetActive (false);
		hideAll ();
		ChangeTab (1);
	}

	void hideAll() {

		text1.SetActive (false);
		text2.SetActive (false);
		text3.SetActive (false);
		text4.SetActive (false);
		text5.SetActive (false);
		text6.SetActive (false);

		/*
		text7.SetActive (false);
		text8.SetActive (false);
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Back ();
		}
	}

	public void Back(){
		if (EscMenu.activeInHierarchy){
			Toggle(EscMenu);
		} else {
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

	public void ChangeTab (int tabNumber) {
		hideAll ();
		if (tabNumber == 1) {
			text1.SetActive (true);
		} 
		else if (tabNumber == 2) {
			text2.SetActive (true);
		} 
		else if (tabNumber == 3) {
			text3.SetActive (true);
		} 
		else if (tabNumber == 4) {
			text4.SetActive (true);
		} 
		else if (tabNumber == 5) {
			text5.SetActive (true);
		} 
		else if (tabNumber == 6) {
			text6.SetActive (true);
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

	public void LoadLevel (string level) {
		Application.LoadLevel(level);
	}
	

}
