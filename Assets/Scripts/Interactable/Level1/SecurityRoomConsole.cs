using UnityEngine;
using System.Collections;

public class SecurityRoomConsole : InteractiveObject {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log ("Inactive");
			return;
		}

		InteractableSync();

	}

	public void InteractableSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync(){
		SecurityRoomCard SRC = gameObject.AddComponent<SecurityRoomCard>();
		GameObject SRCGO = SRC.CreateCard();
		this.CloseEvent();
	}

	public void DoKill(int toKill) {
		GetComponent<PhotonView>().RPC("Kill", PhotonTargets.All, toKill);
	}
	
	[PunRPC]
	void Kill(int toKill){
		Player.MyPlayer.GetComponentInChildren<SecurityRoomObjective>().OnComplete();
		GameManager GameManagerScript = Object.FindObjectOfType<GameManager>();
		this.SetInactive();
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Debug.Log("Finding disconnected player model");
			Debug.Log("Player object id: " + player.GetComponent<PhotonView>().ownerId);
			if (player.GetComponent<PhotonView>().ownerId == toKill) { // Found the player
				Debug.Log("Found player model");
				//PhotonNetwork.Destroy(player);
				if (Player.MyPlayer.GetComponent<PhotonView>().ownerId == toKill) {
					Object.FindObjectOfType<ConnectionManager>().DisconnectClient();
					if (Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject().GetClassTypeEnum() 
							== Classes.BETRAYER) {
						Application.LoadLevel("AlienLoseScreen");
					} else {
						Application.LoadLevel("GameOver");
					}
				}
				Destroy(player);
			}
		}

		Debug.Log("Killed " + toKill);

		try { 
			Debug.Log("kill check"); // Not working... Checks are put else where
			Player p = Player.MyPlayer.GetComponent<Player>();
		} 
		catch (MissingReferenceException e) { // Handle Security System Kill state
			Application.LoadLevel("GameOver");
		}
	}

	
}
