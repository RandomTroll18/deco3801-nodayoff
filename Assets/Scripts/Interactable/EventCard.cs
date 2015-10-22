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

	protected bool DebugOption = false;

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

	// *Depricated*
	void EventCardDestroy(GameObject go)
	{
		Destroy(go.transform.parent.gameObject);
		GameObject Label = card.transform.GetChild(3).gameObject;
	}

	public void Close()
	{
		Destroy(transform.gameObject);
	}

	public void SetCap() {

		if (TeamEvent) {
			GameManager GameManagerScript = Object.FindObjectOfType<GameManager>();
			VoteCap = GameManagerScript.GetPlayersLeft();
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
			counter.ClearCount(ListNumber);
		}
	}

	public virtual void CardEffect(int highestVote){
		if (DebugOption) Debug.Log("Effect not set");
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
