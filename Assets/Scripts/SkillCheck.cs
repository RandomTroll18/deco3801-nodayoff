using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCheck : MonoBehaviour {
	public Stat ClassMultiplier = Stat.NOMULTIPLIER; // The multiplier we are currently looking at
	public int Cost = 0; // The cost of the action
	public Slider APSlider; // Reference to the AP slider
	InteractiveObject Current; // The current interactive object we are tracking

	/**
	 * Start the action of the given interactable object
	 * 
	 * Arguments
	 * - GameObject x - The interactable object
	 */
	public void Action(GameObject x) {
		x.GetComponent<SkillCheck>().Action();
	}

	/**
	 * The action to take for this interactable object
	 */
	public void Action(){
		Current.TakeAction((int)APSlider.value);
	}

	/**
	 * Set the currently tracked interactive object
	 * 
	 * Arguments
	 * - InteractiveObject i - The new interactive object to track
	 */
	public void SetCurrent(InteractiveObject i) {
		Current = i;
	}

	/**
	 * Set the mulitplier and the cost of this skill check
	 * 
	 * Arguments
	 * - Stat multiplier - The new multiplier
	 * - int cost - The cost of this skill check
	 */
	public void SetMultiplierAndCost(Stat multiplier, int cost) {
		ClassMultiplier = multiplier;
		Cost = cost;
	}

}
