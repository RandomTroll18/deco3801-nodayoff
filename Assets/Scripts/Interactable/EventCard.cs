using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class EventCard : MonoBehaviour
{

	//public GameObject prefab;
	
	protected GameObject card;
	protected delegate void Function(int highestVote);
	protected Function Resolve;
	protected int VoteCap;
	protected bool TeamEvent = false;
	protected int ListNumber;

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
		this.ChangeButton(1, "hw");
		this.ChangeText("LOREM IPSUM");
		return;
	}

	public void ChangeButton(int bNum, string input)
	{
		GameObject b = card.transform.GetChild(bNum-1).gameObject;
		b.SetActive(true);
		b.GetComponentInChildren<Text>().text = input;
		Button target = b.GetComponent<Button>();
		
		target.onClick.RemoveAllListeners();
		target.onClick.AddListener(() => Method(bNum));
	}

	/*
	public void ChangeButton(int bNum, string input, UnityAction function)
	{
		GameObject b = card.transform.GetChild(bNum).gameObject;
		b.SetActive(true);
		b.GetComponentInChildren<Text>().text = input;
		Button target = b.GetComponent<Button>();

		target.onClick.RemoveAllListeners();
		if (function == null) {
			target.onClick.AddListener(() => EventCardDestroy(b));
		} else {
			target.onClick.AddListener(function);
		}
	} */
	
	public void ChangeText(string input)
	{
		GameObject Label = card.transform.GetChild(5).gameObject;
		Label.GetComponentInChildren<Text>().text = input;
	}

	public void ChangeImage(string input)
	{
		GameObject Label = card.transform.GetChild(4).gameObject;
		Label.GetComponentInChildren<Image>().overrideSprite = Resources.Load<Sprite>(input);
	}

	void EventCardDestroy(GameObject go)
	{
//		childObject.transform.parent.gameObject
		Destroy(go.transform.parent.gameObject);
		GameObject Label = card.transform.GetChild(3).gameObject;
		//Debug.Log(Label.GetComponentInChildren<Image>().sprite.ToString());
	}

	public void Close()
	{
		Destroy(transform.gameObject);
	}

	public void SetCap() {
		if (TeamEvent) {
			VoteCap = 4; // TODO: Set current players in game
		} else {
			VoteCap = 1;
		}
	}

	private void Vote(int playerNumber) {
		Poll Counter = GameObject.FindGameObjectWithTag("GameController").GetComponent<Poll>();
		Counter.AddToPoll(ListNumber, playerNumber);
		ResolveCard(Counter, playerNumber);
		Destroy(card);
	}

	void Method(int number){
		Vote(number);
		Destroy(card);
	}

	private void ResolveCard(Poll counter, int playerNumber) {
		if (counter.CheckPoll(ListNumber, VoteCap)) {
			//Resolve();
			CardEffect(counter.ReturnHighestCount(ListNumber));
		}
	}

	public virtual void CardEffect(int highestVote){
		Debug.Log("Effect not set");
	}
	
	/*
	private void CheckKill(Poll counter, int player){
		Debug.Log("Count now " + counter.CheckCount(ListNumber, player));
		if (counter.CheckCount(ListNumber, player) == 2) {
			Debug.Log("Kill player " + player);
		}
	}
	*/

}
