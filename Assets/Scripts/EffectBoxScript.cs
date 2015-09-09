using UnityEngine;
using System.Collections;

public class EffectBoxScript : MonoBehaviour {

	TurnEffect effect; // The turn effect attached to this box

	/**
	 * Add a turn effect to this box
	 * 
	 * Arguments
	 * - TurnEffect newEffect - The effect to be added
	 */
	public void AddEffect(TurnEffect newEffect) {
		effect = newEffect;
	}

	/**
	 * Get the turn effect attached to this box
	 * 
	 * Returns
	 * - The turn effect attached to this box
	 */
	public TurnEffect GetEffect() {
		return effect;
	}
}
