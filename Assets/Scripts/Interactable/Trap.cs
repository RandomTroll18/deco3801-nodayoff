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

	public bool OnDebug;
	protected bool DebugOption = false;

	void OnCollisionEnter(Collision col) {
		if (DebugOption) Debug.Log("Collided");
		TrapToDo(col);
	}

	public void StartMe(GameManager g) {

		DebugOption = OnDebug;
		player = GameObject.Find ("Player");
		PrimaryO = GameObject.FindGameObjectWithTag ("Objective UI")
			.GetComponent<PrimaryObjectiveController> ();
		player = Player.MyPlayer; 
		PlayerScript = player.GetComponent<Player>();

	}

	public virtual void TrapToDo(Collision col) {
		if (col.gameObject.name == "Player(Clone)") {
			Activate();
			TrapSync();
		}
	}

	/**
	 * Activate function 
	 * 
	 * Arguments
	 * - Player p - The player that activated this trap
	 * 
	 * *Depricated*
	 */
	public virtual void Activated(Player p) {

		EventCard test = gameObject.AddComponent<EventCard>();
		test.CreateCard();
		if (DebugOption) Debug.Log("Sorry Yugi, but you've triggered my trap card!");
	}

	public virtual void Activate() {
		
	}

	public virtual void TrapSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync() {
		Destroy(this.gameObject);
	}



}
