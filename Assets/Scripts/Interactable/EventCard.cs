using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventCard : MonoBehaviour {

	//public GameObject prefab;

	private GameObject card;

	// Use this for initialization
	void Start () {

	}
	

	// Update is called once per frame
	void Update () {
		
	}

	public GameObject CreateCard(){
		card = Instantiate (Resources.Load ("EventCard")) as GameObject;
		//GameObject card2 = Instantiate (Resources.Load ("EventCard"), Vector3(transform.position.x,transform.position.y, transform.position.z) , Quaternion.identity);
		GameObject UI = GameObject.Find("Main_Canvas");
		card.transform.SetParent(UI.transform, false);
		Debug.Log ("Hey");
		this.ChangeButton (1, "hw");
		this.ChangeText ("LOREM IPSUM");
		return card;
	}

	public void ChangeButton(int bNum, string input){
		GameObject b = card.transform.GetChild (bNum).gameObject;
		b.GetComponentInChildren<Text>().text = input;
		//Debug.Log (Button.GetComponent<Text>().text);//.gameObject.GetComponent<Text> ().text = text;
		Debug.Log ("Hello?" );

		Button target = b.GetComponent<Button> ();

		target.onClick.RemoveAllListeners();
		target.onClick.AddListener (() => EventCardDestroy(b));
	}
	
	public void ChangeText(string input){
		GameObject  Label = card.transform.GetChild (4).gameObject;
		Label.GetComponentInChildren<Text> ().text = input;
	}
	

	void EventCardDestroy(GameObject go) {
//		childObject.transform.parent.gameObject
		Destroy(go.transform.parent.gameObject);
	}

	public void Close(){
		Destroy(transform.gameObject);
	}
	
}
