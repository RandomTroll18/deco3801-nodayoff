using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventCard : MonoBehaviour
{

	//public GameObject prefab;

	protected GameObject card;

	// Use this for initialization
	void Start()
	{

	}
	

	// Update is called once per frame
	void Update()
	{
		
	}

	public GameObject CreateCard()
	{
		card = Instantiate(Resources.Load("EventCard")) as GameObject;
		GameObject UI = GameObject.Find("Main_Canvas");
		card.transform.SetParent(UI.transform, false);
		ChangeCard();
		return card;
	}

	public virtual void ChangeCard()
	{
		Debug.Log("Hey");
		this.ChangeButton(1, "hw", null);
		this.ChangeText("LOREM IPSUM");
		return;
	}

	public void ChangeButton(int bNum, string input, Event function)
	{
		GameObject b = card.transform.GetChild(bNum).gameObject;
		b.SetActive(true);
		b.GetComponentInChildren<Text>().text = input;
		Button target = b.GetComponent<Button>();

		target.onClick.RemoveAllListeners();
		target.onClick.AddListener(() => EventCardDestroy(b));
	}
	
	public void ChangeText(string input)
	{
		GameObject Label = card.transform.GetChild(4).gameObject;
		Label.GetComponentInChildren<Text>().text = input;
	}

	public void ChangeImage(string input)
	{
		GameObject Label = card.transform.GetChild(3).gameObject;
		Label.GetComponentInChildren<Image>().overrideSprite = Resources.Load<Sprite>(input);
	}

	void EventCardDestroy(GameObject go)
	{
//		childObject.transform.parent.gameObject
		Destroy(go.transform.parent.gameObject);
		GameObject Label = card.transform.GetChild(3).gameObject;
		Debug.Log(Label.GetComponentInChildren<Image>().sprite.ToString());
	}

	public void Close()
	{
		Destroy(transform.gameObject);
	}
	
}
