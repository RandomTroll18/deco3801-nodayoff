using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Poll : MonoBehaviour {

	public int NumberOfPolls; // The number of events/objects to vote for
	List<List<int>> BigList = new List<List<int>>(); //  The list of votes

	/**
	 * Initialize this object
	 */
	public void StartMe() {

		for (int v = 0; v < NumberOfPolls; v++) // Create the list of votes for each object/event
			BigList.Add(new List<int>());
	}

	/**
	 * Add vote/s to the given event/object
	 * 
	 * Arguments
	 * - int number - The object/event to vote for
	 * - int vote - The number of votes to add
	 */
	public void AddToPoll(int number, int vote) {
		GetComponent<PhotonView>().RPC("AddToPollRPC", PhotonTargets.All, number, vote);
	}

	/**
	 * RPC call for adding votes
	 * 
	 * Arguments
	 * - int number - The object/event to vote for
	 * - int vote - The number of votes to add
	 */
	[PunRPC]
	void AddToPollRPC(int number, int vote) {
		Debug.Log("Add to poll " + number + "|" + vote);
		BigList[number].Add(vote);
	}

	/**
	 * Check if an object/event has met the specified votes
	 * 
	 * Arguments
	 * - int number - The poll for the object/event we are looking at
	 * - int cap - The number of votes needed
	 * 
	 * Returns
	 * - true if the poll has reached the needed votes. false otherwise
	 */
	public bool CheckPoll(int number, int cap) {
		if (BigList[number].Count >= cap) // Reached needed votes
			return true;
		return false;
	}

	/**
	 * Return the number of votes 
	 * 
	 * Arguments
	 * - int number - The poll for the object/event we are looking at
	 * - int cap - The minimum number of votes needed
	 * 
	 * Returns
	 * - The number of votes for the poll, if it meets the cap. -1 otherwise
	 */
	public int ReturnHighestCount(int number, int cap){
		List<int> a = BigList[number]; // The current poll
		int tempCount; // Storage for the vote count

		for (int x = 1; x < 5; x++) { // Check all the possible objects/events in the poll
			tempCount = 0;
			for (int i = 0; i < (a.Count - 1); i++) { // Check the number of votes for a given object/event
				if (a[i] == x) // Found the current object/event we are looking at
					tempCount++;
			}
			if (tempCount == cap - 1) // Object/Event meets the required number of votes
				return x;
		}
		return -1;
	}

	/**
	 * Check the number of votes for the given poll and object/event
	 * 
	 * Arguments
	 * - int number - The poll
	 * - int check - The object/event
	 * 
	 * Returns
	 * - The number of votes for the given poll and object/event
	 */
	public int CheckCount(int number, int check){
		int count = 0; // Temporary storage for the vote count
		List<int> a = BigList[number]; // The poll
		for (int i = 0; i < (a.Count); i++) {
			if (a[i] == check)
				count++;
		}
		return count;
	}

	/**
	 * Clear the current poll
	 * 
	 * Arguments
	 * - int number - The poll to clear
	 */
	public void ClearCount(int number){
		GetComponent<PhotonView>().RPC("ClearCountRPC", PhotonTargets.All, number);

	}

	/**
	 * Get the poll requested
	 * 
	 * Arguments
	 * - int x - The poll being requested
	 * 
	 * Returns
	 * - The poll, if available. Null otherwise
	 */
	public List<int> GetData(int x) {
		return BigList[x];
	} 

	/**
	 * RPC call for clearing the poll
	 * 
	 * Arguments
	 * - int number - The poll to clear
	 */
	[PunRPC]
	void ClearCountRPC(int number) {
		BigList[number] = new List<int>();
	}


}
