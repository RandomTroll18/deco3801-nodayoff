using UnityEngine;
using System.Collections;

public class MarinePrimaryAbility : Ability {

	Player master; // Owning player
	int extraCharge; // The number of extra charges in this turn

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The player that spawned this robot
	 */
	public MarinePrimaryAbility(Player player) {
		AbilityName = "Stimulus Debris";
		Range = 0.0; // No range
		AbilityRangeType = RangeType.GLOBALTARGETRANGE;
		AbilityActivationType = ActivationType.SUPPORTIVE;
		RemainingTurns = 3; // Only 3 remaining turns
		master = player;
		extraCharge = 3; // 3 extra charges in this turn
	}

	/**
	 * Apply class ability to this item
	 * 
	 * Arguments
	 * - Item item - The item to apply the class ability to
	 */
	public void ApplyAbilityToItem(Item item) {
		StunGun stunGun; // Stun gun object
		if (item.GetType() != typeof(StunGun)) return; // Not stun gun
		else if (extraCharge == 0) return; // No more extra charges this turn
		stunGun = (StunGun)item;
		stunGun.ResetCoolDown();
		extraCharge--;
	}

	/**
	 * Activate function
	 */
	public override void Activate()
	{
		base.Activate();
		master.SetStunImmunity(true); // Player is now stunned
	}

	/**
	 * Reduce number of turns left
	 */
	public override void ReduceNumberOfTurns()
	{
		base.ReduceNumberOfTurns();
		extraCharge = 3;
		if (RemainingTurns == 0) {
			master.SetStunImmunity(false); // No longer immune
		}
	}
}
