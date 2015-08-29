using UnityEngine;
using System.Collections;

public class TurnEffect : MonoBehaviour {

	private Stat statAffected; // The stat affected
	/*
	 * The value that the stat is affected by.
	 * The value can be negative
	 */
	private double value;

	/**
	 * The constructor
	 * 
	 * Arguments
	 * - Stat statAffected - the stat to be affected
	 * - double value - The value that the stat will be affected by
	 */
	public TurnEffect (Stat statAffected, double value) {
		this.statAffected = statAffected;
		this.value = value;
	}

	/**
	 * Get the stat affected by this turn effect
	 * 
	 * Returns
	 * - The stat this effect is affecting
	 */
	public int getStatAffected () {
		return (int)this.statAffected;
	}

	/**
	 * Get the value by which the stat is to be affected
	 * 
	 * Returns
	 * - The value that the stat will be affected by
	 */
	public double getValue () {
		return this.value;
	}
}
