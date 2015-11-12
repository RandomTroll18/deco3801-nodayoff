using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

/**
 * A base class that other Event Card will extend. Used to create Event Card prefab into Player UI.
 */
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
	}

	/**
	 * Change the settings for the event card's button
	 * 
	 * Arguments
	 * - int bNum - The identifier for the button to change
	 * - string input - The text to be displayed on this button
	 */
	public void ChangeButton(int bNum, string input)
	{
		GameObject b = Card.transform.GetChild(bNum-1).gameObject; // The button we are changing
		Button target = b.GetComponent<Button>(); // The button's script

		/* Make the button visible and change its text */
		b.SetActive(true);
		b.GetComponentInChildren<Text>().text = input;

		/* Add an onclick listener to this button */
		target.onClick.RemoveAllListeners();
		target.onClick.AddListener(() => Method(bNum));
	}

	/**
	 * Change the title of this event card
	 * 
	 * Arguments
	 * - string input -  The new title fo the event card
	 */
	public void ChangeTitle(string input)
	{
		GameObject Label = GameObject.FindGameObjectWithTag("ECTitle").gameObject; // Event Card Title Object
		Label.GetComponentInChildren<Text>().text = input;
	}

	/**
	 * Change the contents of the event card
	 * 
	 * Arguments
	 * - string input - The new string content for the event card
	 */
	public void ChangeText(string input)
	{
		GameObject Label = GameObject.FindGameObjectWithTag("ECDesc").gameObject; // Event Card Content Object
		Label.GetComponentInChildren<Text>().text = input;
	}

	/**
	 * Change the image to be displayed by this event card
	 * 
	 * Arguments
	 * - string input - The path to the new icon image for the event card
	 */
	public void ChangeImage(string input)
	{
		GameObject Label = GameObject.FindGameObjectWithTag("ECImg").gameObject; // Event Card Image Object
		Label.GetComponentInChildren<Image>().overrideSprite = Resources.Load<Sprite>(input);
	}

	/**
	 * Delete this event card
	 */
	public void Close()
	{
		Destroy(transform.gameObject);
	}

	/**
	 * Set the vote cap. For use with security system and any other event 
	 * card that needs voting
	 */
	public void SetCap() {

		if (TeamEvent) // Team event. Need to set the maximum number of votes to be the number of players
			VoteCap = PhotonNetwork.playerList.Length;
		else // Only need the vote of one player. Probably a deision card
			VoteCap = 1;
	}

	/**
	 * Make a vote
	 * 
	 * Arguments
	 * - int playerNumber - The player that made the vote
	 */
	void Vote(int playerNumber) {
		Counter.AddToPoll(ListNumber, playerNumber);
		ResolveCard(Counter, playerNumber);
		Destroy(Card);
	}

	/**
	 * A method to add for the button's onclick listener. For now, 
	 * used for voting
	 * 
	 * Arguments
	 * - int number - The player that made the vote
	 */
	void Method(int number){
		Vote(number);
		Destroy(Card);
	}

	/**
	 * Resolve the voting initiated by this event card
	 * 
	 * Arguments
	 * - Poll counter - The vote counter
	 * - int playerNumber - The player that made the last vote
	 */
	void ResolveCard(Poll counter, int playerNumber) {
		int placeholder; // Placeholder for the highest vote count
		if (counter.CheckPoll(ListNumber, VoteCap)) { // All players have voted
			Debug.Log("Hit vote count");
			placeholder = counter.ReturnHighestCount(ListNumber, VoteCap);
			if (TeamEvent) {
				if (placeholder != -1) { // Enough votes. Execute the effect of this event card
					CardEffect(placeholder);
					ChatTest.Instance.GetComponent<PhotonView>().RPC("Big", PhotonTargets.All, new object[] 
					                                                 {"Event Successful"});
				} else // Not enough votes
					ChatTest.Instance.GetComponent<PhotonView>().RPC("Big", PhotonTargets.All, new object[] 
					                                                 {"Event Failed"});
			}
			counter.ClearCount(ListNumber); // Need to clear the vote counter
		}
	}

	/**
	 * Activate the effect of this event card
	 * 
	 * Arguments
	 * - int highestVote - The number of votes for the desired event
	 */
	public virtual void CardEffect(int highestVote){
		if (DebugOption) 
			Debug.Log("Effect not set");
	}
}
