using UnityEngine;

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
	void Sync(){
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
	void Kill(int toKill){
		Player.MyPlayer.GetComponentInChildren<SecurityRoomObjective>().OnComplete();
		SetInactive();
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.GetComponent<PhotonView>().ownerId == toKill) { // Found the player
				if (Player.MyPlayer.GetComponent<PhotonView>().ownerId == toKill) { // This is the player to kill
					Object.FindObjectOfType<ConnectionManager>().DisconnectClient();
					if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() 
							== Classes.BETRAYER) { // Alien was killed. Alien lost
						Application.LoadLevel("AlienLoseScreen");
					} else // Human lost
						Application.LoadLevel("GameOver");
				}
				Destroy(player);
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
