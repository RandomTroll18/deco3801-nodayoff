﻿using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SwitchToScene2 () {
		Application.LoadLevel("TestScene2");
	}

	public void SwitchToScene1 () {
		Application.LoadLevel("TestScene1");
	}
}