using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

	public int x;
	public int y;
	public int width;
	public int height;

	public GameObject MainPanel;
	public int CurrentPanel;
	public GameObject[] Panels;

	protected Popup NextPopup;
	//panels = new GameObject[totalPanel];


	public GameObject Create() {

		GameObject PopupGameObject = Instantiate(Resources.Load("EventCard")) as GameObject;
		GameObject Canvas = GameObject.Find("Main_Canvas");
		PopupGameObject.transform.SetParent(Canvas.transform, false);
		ChangeContent();
		return PopupGameObject;

	}

	public void ClosePopup() {
		Destroy(this);
		if (!NextPopup.Equals(null)) {
			NextPopup.Create();
		}

	}

	public virtual void ChangeContent() {

	}
	
}
