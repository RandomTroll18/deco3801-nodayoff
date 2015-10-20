using UnityEngine;
using System.Collections;

public class MarinePrimaryAbility : Ability {

	Player master; // Owning player
	Material defaultPlayerMaterial; // The player's default material
	Material abilityActivePlayerMaterial; // Material used when ability is active
	Effect materialEffect; // The material effect to add to the player
	Effect stunGunEffect; // The stun gun effect to add to the player
	Effect noCoolDownEffect; // Removal of stun gun cooldown effect

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

		// Create turn effects
		materialEffect = new MaterialTurnEffect("AbilityMaterials/Marine/MarinePrimAbilityMaterial", 
				"Stimulus Debris: Immune To Stun", "Icons/Effects/stunimmunitygreen", 2, false);
		stunGunEffect = new ItemTurnEffect(typeof(StunGun), "Stimulus Debris: Extra Stun Gun Charges", 
				"Icons/Effects/extrachargesgreen", 2, ItemTurnEffectType.EXTRAUSE, 3, false);
		noCoolDownEffect = new ItemTurnEffect(typeof(StunGun), "Stimulus Debris: No Stun Gun Cool Down", 
				"Icons/Effects/stuninstantCDgreen", 2, ItemTurnEffectType.COOLDOWN, 1, false);
	}

	/**
	 * Activate function
	 */
	public override void Activate()
	{
		base.Activate();
		master.SetStunImmunity(true); // Player is now stunned
		master.AttachTurnEffect(materialEffect);
		master.AttachTurnEffect(stunGunEffect);
		master.AttachTurnEffect(noCoolDownEffect);
	}

	/**
	 * Reduce number of turns left
	 */
	public override void ReduceNumberOfTurns()
	{
		base.ReduceNumberOfTurns();
		if (RemainingTurns == 0) {
			master.SetStunImmunity(false); // No longer immune
		}
	}
}
