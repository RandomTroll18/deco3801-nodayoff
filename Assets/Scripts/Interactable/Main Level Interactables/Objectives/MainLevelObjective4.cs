using UnityEngine;
using System.Collections;

public class MainLevelObjective4 : InteractiveObject {

	bool escape = false; // Record if this player escaped or not
	
	public override void TakeAction(int input){
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}

		if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() == Classes.BETRAYER) {
			ChatTest.Instance.AllChat(true, "ALIEN CAN'T ESCAPE");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<FourthObjectiveMain>().Title)) {
			ChatTest.Instance.AllChat(true, "NO POWER");
			Debug.Log("Wrong part of the story");
			return;
		}

		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
	}

	public void InteractablSync() {
		escape = true;

		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	/**
	 * Invoke the alien's game over screen
	 */
	[PunRPC]
	public void AlienGameOver() {
		ChatTest.Instance.Big("ALIEN GAME OVER");
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) // Alien lost
			Application.LoadLevel("AlienLoseScreen");
	}

	[PunRPC]
	public void Sync() {
		Debug.LogError("Syncing escape pod");
		ChatTest.Instance.AllChat(false, "ONE ESCAPE POD LOST");
		Object.FindObjectOfType<GameManager>().GetComponent<PhotonView>().RPC("PlayerEscaped", PhotonTargets.All, null);

		if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() == Classes.BETRAYER
		    	&& GameObject.FindGameObjectsWithTag("Player").Length <= 1) {
			Application.LoadLevel("AlienLoseScreen"); // No more humans. Alien lost
		}

		if (escape) { // Human won
			if (PhotonNetwork.playerList.Length <= 2) // No more humans. Alien lost
				GetComponent<PhotonView>().RPC("AlienGameOver", PhotonTargets.All, null);
			Object.FindObjectOfType<ConnectionManager>().DisconnectClient();
			if (Object.FindObjectOfType<GameManager>().HasPlayerDied()) // A human has died 
				Application.LoadLevel("PartialWinScreen");
			else // No player has died yet
				Application.LoadLevel("WinScreen");
		}
		PhotonNetwork.Destroy(gameObject);
		ChatTest.Instance.AllChat(true, "REMAINING PODS: " + getPodsRemaining());
	}

	/**
	 * Get the number of escape pods remaining
	 * 
	 * Returns
	 * - The number of pods left to activate
	 */
	int getPodsRemaining() {
		/* The escape pods */
		GameObject pod1 = GameObject.Find("EscapePod 1");
		GameObject pod2 = GameObject.Find("EscapePod 2");
		GameObject pod3 = GameObject.Find("EscapePod 3");
		GameObject pod4 = GameObject.Find("EscapePod 4");
		int podCount = 0; // The number of pods

		if (pod1 != null)
			podCount++;
		if (pod2 != null)
			podCount++;
		if (pod3 != null)
			podCount++;
		if (pod4 != null)
			podCount++;

		return podCount;
	}

}
