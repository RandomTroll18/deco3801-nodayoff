using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Update is called once per frame
	public void Activated (Player p) {

		Debug.Log ("Sorry Yugi, but you've triggered my trap card!");
		Destroy(this.gameObject);
		GameObject card = Instantiate (Resources.Load ("EventCard")) as GameObject;
		//GameObject card2 = Instantiate (Resources.Load ("EventCard"), Vector3(transform.position.x,transform.position.y, transform.position.z) , Quaternion.identity);
		GameObject UI = GameObject.Find("Main_Canvas");
		card.transform.SetParent(UI.transform, false);


	}

}
