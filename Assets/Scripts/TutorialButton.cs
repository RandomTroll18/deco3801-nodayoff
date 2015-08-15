﻿using UnityEngine;
using System.Collections;

public class TutorialButton : MonoBehaviour {
	
	public GameObject text1;
	public GameObject text2;
	public GameObject text3;
	public GameObject text4;
	public GameObject text5;
	public GameObject text6;
	/* public GameObject text7;
	public GameObject text8;
	public GameObject testText; */


	// Use this for initialization
	void Start () {
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
}
