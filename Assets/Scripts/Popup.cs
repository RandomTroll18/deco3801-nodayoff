using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

	public int currentPanel;
	public GameObject[] panels;
	//panels = new GameObject[totalPanel];


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NextPanel(){
		panels [currentPanel].SetActive (false);
		currentPanel++;
		if (currentPanel < panels.Length) {
			panels [currentPanel].SetActive (true);
		}
	}
}
