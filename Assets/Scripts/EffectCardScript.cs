using UnityEngine;
using System.Collections;

public class EffectCardScript : MonoBehaviour {

	TurnEffect effect; // The turn effect attached to this card

	/**
	 * Add a turn effect to this card
	 * 
	 * Arguments
	 * - TurnEffect newEffect - The effect to be added
	 */
	public void AddEffect(TurnEffect newEffect) {
		effect = newEffect;
	}

	/**
	 * Get the turn effect attached to this card
	 * 
	 * Returns
	 * - The turn effect attached to this card
	 */
	public TurnEffect GetEffect() {
		return effect;
	}
}
