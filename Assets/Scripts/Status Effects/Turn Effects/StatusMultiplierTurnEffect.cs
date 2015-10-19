using UnityEngine;
using System.Collections;

public class StatusMultiplierTurnEffect : StatusTurnEffect {

	/**
	 * The constructor for a stat effect
	 * 
	 * Arguments
	 * - Stat statAffected - the stat to be affected
	 * - double value - The value that the stat will be affected by
	 * - int mode - The way the stat would be applied
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon (with the Resources folder as the root)
	 * - int turnsActive - Number of turns active
	 * - bool applyPerTurnFlag - set this effect to be applied per turn or not
	 */
	public StatusMultiplierTurnEffect(Stat newStatAffected, double newValue, int newMode, 
	                        string newDescription, string iconPath, int turnsActive, bool applyPerTurnFlag) 
			: base(newStatAffected, newValue, newMode, newDescription, iconPath, turnsActive, applyPerTurnFlag) {
		switch (newStatAffected) { // Can only affect multiplier
		case Stat.AP: goto case Stat.VISION;
		case Stat.VISION: /* Invalid */
			throw new System.ArgumentException("Invalid multiplier stat");
		}
		Type = TurnEffectType.STATMULTIPLIEREFFECT;
	}
}
