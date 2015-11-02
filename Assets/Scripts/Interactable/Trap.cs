using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Trap interactables
 */
public class Trap : MonoBehaviour {

	GameObject player; // The player we are tracking

	public bool OnDebug; // The debug option

	protected Player PlayerScript; // The script of the player we are tracking
	protected PrimaryObjectiveController PrimaryO; // The primary objective controller
	protected bool DebugOption = false;

	/**
	 * On collision callback
	 */
	void OnCollisionEnter(Collision col) {
		if (DebugOption) 
			Debug.Log("Collided");
		TrapToDo(col);
	}

	/**
	 * Initialize this object
	 * 
	 * Arguments
	 * - GameManager g - The game manager
	 */
	public void StartMe(GameManager g) {

		DebugOption = OnDebug;
		player = GameObject.Find("Player");
		PrimaryO = GameObject.FindGameObjectWithTag("Objective UI")
			.GetComponent<PrimaryObjectiveController>();
		player = Player.MyPlayer; 
		PlayerScript = player.GetComponent<Player>();

	}

	public virtual void TrapToDo(Collision col) {
		if (col.gameObject.name == "Player(Clone)") { // Found a player model
			Activate();
			TrapSync();
		}
	}

	/**
	 * Activate function with a target
	 * 
	 * Arguments
	 * - Player p - The player that activated this trap
	 * 
	 * *Depricated*
	 */
	public virtual void Activated(Player p) {

		EventCard test = gameObject.AddComponent<EventCard>(); // Test event card
		test.CreateCard();
		if (DebugOption) 
			Debug.Log("Sorry Yugi, but you've triggered my trap card!");
	}

	/**
	 * Activate function without a target
	 */
	public virtual void Activate() {
		return;
	}

	/**
	 * Sync this trap with other players
	 */
	public virtual void TrapSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}

	/**
	 * The RPC call for syncing
	 */
	[PunRPC]
	void Sync() {
		Destroy(gameObject);
	}



}
