using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//class representing Interactive Objects
public class Trap : MonoBehaviour {
	
	//check if player is interacting with object
	//public bool IsInteracting = false;
	
	//the UICanvas
	//NOTE: Will there be problems with Blocking and Unblocking a tile with multiple players since each player uses their own MoveController???
	GameObject player;

	protected Player PlayerScript;
	protected PrimaryObjectiveController PrimaryO;

	void OnCollisionEnter(Collision col) {
		Debug.Log("Collided");
		Activate();
		TrapSync();
	}

	public void StartMe(GameManager g) {

		player = GameObject.Find ("Player");
		PrimaryO = GameObject.FindGameObjectWithTag ("Objective UI")
			.GetComponent<PrimaryObjectiveController> ();
		player = Player.MyPlayer; 
		PlayerScript = player.GetComponent<Player>();

	}

	/**
	 * Activate function 
	 * 
	 * Arguments
	 * - Player p - The player that activated this trap
	 */
	public virtual void Activated(Player p) {

		EventCard test = gameObject.AddComponent<EventCard>();
		GameObject UI = test.CreateCard();
		Debug.Log("Sorry Yugi, but you've triggered my trap card!");
		Destroy(this.gameObject);

	}

	public virtual void Activate() {
		
	}

	public void TrapSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		Destroy(this.gameObject);
	}



}
