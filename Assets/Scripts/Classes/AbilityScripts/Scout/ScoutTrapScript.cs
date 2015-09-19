using UnityEngine;
using System.Collections;

public class ScoutTrapScript : Trap {

	Player owner; // The owner of this trap
	GameObject trapObject; // This script's trap object

	/**
	 * Activate function
	 * 
	 * Arguments
	 * - Player player - The player that activated the trap
	 */
	public override void Activated(Player p)
	{
		if (p == owner) Debug.Log("I'm sorry master. I won't activate");
		else {
			Debug.Log("Activated scout trap");
			Destroy(this);
		}
	}

	/**
	 * Set the object reference to the trap
	 * 
	 * Arguments
	 * - GameObject objectReference - The game object that this script is attached to
	 */
	public void SetReference(GameObject objectReference) {
		trapObject = objectReference;
	}

	/**
	 * Set the owner of this trap
	 * 
	 * Arguments
	 * - Player newOwner - The new owner of this trap
	 */
	public void SetOwner(Player newOwner) {
		owner = newOwner;
	}
}
