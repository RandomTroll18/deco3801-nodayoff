using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class EventCard : MonoBehaviour
{
	protected GameObject Card; // The reference to this card
	protected delegate void Function(int highestVote); // Delegate for effects
	protected Function Resolve; // The function to be added as a delegate
	protected int VoteCap; // The maximum amount of votes
	protected bool TeamEvent = false; // Record if this event should be for the entire team or not
	protected int ListNumber; // The current location in the poll
	Poll Counter; // The Vote Counter
	protected bool DebugOption = false; // Debug Option Mode


	/**
	 * Create the card
	 */
	public GameObject CreateCard()
	{
		Counter = GameObject.FindGameObjectWithTag("GameController").GetComponent<Poll>();
		Counter.ClearCount(ListNumber);
		bool placeholder = true;
		foreach (Transform objectTransform in GameObject.Find("Main_Canvas").transform) {
			if (objectTransform.name.Equals("EventCard(Clone)")) {
				placeholder = false;
				break;
			}
		}

		if (placeholder) { // An event card already exists
			Card = Instantiate(Resources.Load("EventCard")) as GameObject;
			GameObject UI = GameObject.Find("Main_Canvas");
			Card.transform.SetParent(UI.transform, false);
			ChangeCard();
		}

		return Card;
	}

	/**
	 * Change the card to its default values
	 */
	public virtual void ChangeCard()
	{
		ChangeButton(1, "hw");
		ChangeText("LOREM IPSUM");
		return;
	}

	public void ChangeButton(int bNum, string input)
	{
		GameObject b = Card.transform.GetChild(bNum-1).gameObject;
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

	public void ChangeTitle(string input)
	{
		GameObject Label = GameObject.FindGameObjectWithTag("ECTitle").gameObject;
		Label.GetComponentInChildren<Text>().text = input;
	}

	public void ChangeText(string input)
	{
		GameObject Label = GameObject.FindGameObjectWithTag("ECDesc").gameObject;
		Label.GetComponentInChildren<Text>().text = input;
	}

	public void ChangeImage(string input)
	{
		GameObject Label = GameObject.FindGameObjectWithTag("ECImg").gameObject;
		Label.GetComponentInChildren<Image>().overrideSprite = Resources.Load<Sprite>(input);
	}

	// *Depricated*
	void EventCardDestroy(GameObject go)
	{
		Destroy(go.transform.parent.gameObject);
	}

	public void Close()
	{
		Destroy(transform.gameObject);
	}

	public void SetCap() {

		if (TeamEvent) {
			//GameManager GameManagerScript = Object.FindObjectOfType<GameManager>();
			VoteCap = PhotonNetwork.playerList.Length;
		} else {
			VoteCap = 1;
		}
	}

	private void Vote(int playerNumber) {
		Counter.AddToPoll(ListNumber, playerNumber);
		ResolveCard(Counter, playerNumber);
		Destroy(Card);
	}

	void Method(int number){
		Vote(number);
		Destroy(Card);
	}

	private void ResolveCard(Poll counter, int playerNumber) {
		if (counter.CheckPoll(ListNumber, VoteCap)) {
			Debug.Log("Hit vote count");
			int placeholder = counter.ReturnHighestCount(ListNumber, VoteCap);

			if (placeholder != -1) {
				CardEffect(placeholder);
				ChatTest.Instance.GetComponent<PhotonView>().RPC("Big", PhotonTargets.All, new object[] 
				                                                 {"Event Successful"});
			} else {
				ChatTest.Instance.GetComponent<PhotonView>().RPC("Big", PhotonTargets.All, new object[] 
				                                                 {"Event Failed"});
			}
			counter.ClearCount(ListNumber);
		}
	}

	public virtual void CardEffect(int highestVote){
		if (DebugOption) Debug.Log("Effect not set");
	}
}
