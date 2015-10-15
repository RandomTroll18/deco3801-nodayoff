using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Popup : MonoBehaviour {

	public int x;
	public int y;

	public GameObject MainPanel;
	public int CurrentPanel;
	public GameObject PopupGameObject;

	protected Popup NextPopup;
	//panels = new GameObject[totalPanel];


	public GameObject Create() {

		PopupGameObject = Instantiate(Resources.Load("PopupUI")) as GameObject;
		GameObject Canvas = GameObject.Find("Main_Canvas");
		PopupGameObject.transform.SetParent(Canvas.transform, false);
		ChangeContent();
		return PopupGameObject;

	}

	void ClosePopup() {
		Destroy(PopupGameObject);
		if (NextPopup) {
			NextPopup.Create();
		}
	}

	public virtual void ChangeContent() {
		ChangeButton();
	}

	public void ChangeButton(){

		GameObject b = PopupGameObject.transform.GetChild(1).gameObject;
		Button target = b.GetComponent<Button>();
		
		target.onClick.RemoveAllListeners();
		target.onClick.AddListener(() => ClosePopup());

	}
	
}
