using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Poll : MonoBehaviour {

	public int NumberOfPolls;
	private List<List<int>> BigList = new List<List<int>>();

	// Use this for initialization
	public void StartMe() {

		for (int v = 0; v < NumberOfPolls; v++)
		{
			BigList.Add(new List<int>());
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddToPoll(int number, int vote) {
		GetComponent<PhotonView>().RPC("AddToPollRPC", PhotonTargets.All, number, vote);
	}
	
	[PunRPC]
	void AddToPollRPC(int number, int vote) {
		Debug.Log("Add to poll " + number + "|" + vote);
		BigList[number].Add(vote);
	}

	public bool CheckPoll(int number, int cap) {
		if (BigList[number].Count >= cap) {
			return true;
		}
		return false;
	}

	public int ReturnHighestCount(int number, int cap){
		List<int> a = BigList[number];
		int tempCount = 0;
		int temp = 0;

		for (int x = 1; x < 5; x++)
		{
			temp = x;
			tempCount = 0;
			for (int i = 0; i < (a.Count - 1); i++)
			{
				if (a[i] == x) {
					tempCount++;
				}
			}
			if (tempCount == cap - 1) {
				return x;
			}
		}
		return -1;
	}

	public int CheckCount(int number, int check){
		int Count = 0;
		List<int> a = BigList[number];
		for (int i = 0; i < (a.Count); i++)
		{
			if (a[i] == check){
				Count++;
			}
		}
		return Count;
	}

	public void ClearCount(int number){
		GetComponent<PhotonView>().RPC("ClearCountRPC", PhotonTargets.All, number);

	}

	public List<int> GetData(int x) {
		return BigList[x];
	} 
	
	[PunRPC]
	void ClearCountRPC(int number) {
		//Debug.Log("Clear count");
		BigList[number] = new List<int>();
	}


}
