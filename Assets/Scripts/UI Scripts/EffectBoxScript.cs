using UnityEngine;
using System.Collections;

public class EffectBoxScript : MonoBehaviour {

	Effect effect; // The turn effect attached to this box

	/**
	 * Add a turn effect to this box
	 * 
	 * Arguments
	 * - TurnEffect newEffect - The effect to be added
	 */
	public void AddEffect(Effect newEffect) {
		effect = newEffect;
	}

	/**
	 * Get the turn effect attached to this box
	 * 
	 * Returns
	 * - The turn effect attached to this box
	 */
	public Effect GetEffect() {
		return effect;
	}
}
