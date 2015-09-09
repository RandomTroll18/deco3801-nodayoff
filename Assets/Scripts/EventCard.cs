using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventCard : MonoBehaviour {

	//public GameObject prefab;

	// Use this for initialization

	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}

	public void Close(){
		Destroy(transform.gameObject);
	}
	
}
