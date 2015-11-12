using UnityEngine;

/*
 * Security Room Console
 * Interactable used for players to vote kill a particular player
 */
public class SecurityRoomConsole : InteractiveObject {

	
	public override void TakeAction(int input){
		
		if (IsInactivated) { // Don't activate this object. It's inactive
			Debug.Log ("Inactive");
			return;
		}

		InteractableSync();

	}

	/**
	 * Sync this object with other players
	 */
	public void InteractableSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	/**
	 * RPC call for syncing with other players
	 */
	[PunRPC]
	public void Sync(){
		SecurityRoomCard SRC = gameObject.AddComponent<SecurityRoomCard>(); // The security room event card
		SRC.CreateCard();
		CloseEvent();
	}

	/**
	 * Kill the player that was voted to be killed
	 */
	public void DoKill(int toKill) {
		GetComponent<PhotonView>().RPC("Kill", PhotonTargets.All, toKill);
	}

	/**
	 * RPC call for killing the player
	 */
	[PunRPC]
	public void Kill(int toKill){
		GameObject foundPlayer = null; // The player model

		Debug.LogError("Killing a player");
		Player.MyPlayer.GetComponentInChildren<SecurityRoomObjective>().OnComplete();
		SetInactive();
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.GetComponent<PhotonView>().ownerId == toKill) { // Found the player model
				foundPlayer = player;
				break;
			}
		}
		if (foundPlayer != null) { // Found our player
			foreach (PhotonPlayer networkPlayer in PhotonNetwork.playerList) {
				if (networkPlayer.ID == foundPlayer.GetComponent<PhotonView>().owner.ID) { // Found the network player
					if (networkPlayer.GetTeam() == PunTeams.Team.blue) { // Human was killed
						Debug.LogError("A human was killed");
						if (PhotonNetwork.player.ID == networkPlayer.ID) { // This is the killed player
							Object.FindObjectOfType<GameManager>().GetComponent<PhotonView>().RPC("PlayerDied", 
							                                                                      PhotonTargets.All, 
							                                                                      null);
							Object.FindObjectOfType<ConnectionManager>().DisconnectClient();
							Application.LoadLevel("GameOver");
						} else // This is a surviving player
							Object.FindObjectOfType<GameManager>().PlayerDied();
					} else { // Alien was killed
						Debug.LogError("Alien was killed. Alien lost");
						if (PhotonNetwork.player.ID == networkPlayer.ID) { // This is the killed player
							Object.FindObjectOfType<ConnectionManager>().DisconnectClient();
							Application.LoadLevel("AlienDeathScreen");
						}
					}
					break;
				}
			}
		}
		Debug.Log("Killed " + toKill);

		try { 
			Debug.Log("kill check"); // Not working... Checks are put else where
		} 
		catch (MissingReferenceException) { // Handle Security System Kill state
			Application.LoadLevel("GameOver");
		}
	}

	
}
