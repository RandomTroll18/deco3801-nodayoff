using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

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

}
