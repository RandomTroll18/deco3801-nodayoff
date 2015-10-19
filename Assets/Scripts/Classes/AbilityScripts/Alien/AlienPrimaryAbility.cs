using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienPrimaryAbility : Ability {

	Player owner; // The owner of this ability
	Effect changeColour; // The material effect for this alien
	Effect addAP; // The status effect for the alien - giving bonus AP
	List<Effect> secondaryEffectRewards; // Effects that were awarded for completing secondary objectives
	int initialNumberOfTurns; // The initial number of turns

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The owner of this ability
	 */
	public AlienPrimaryAbility(Player player) {
		AbilityName = "Alien Mode";
		Range = 0.0; // No range
		AbilityRangeType = RangeType.GLOBALTARGETRANGE;
		AbilityActivationType = ActivationType.SUPPORTIVE;

		owner = player;

		// Create turn effects
		changeColour = new MaterialTurnEffect("AbilityMaterials/Alien/AlienModeMaterial", 
				"Alien Mode: Turn Into An Alien", "Icons/Effects/DefaultEffect", -1, false);

		secondaryEffectRewards = new List<Effect>();
	}

	/**
	 * Add a bonus secondary effect
	 * 
	 * Arguments
	 * - Effect bonusEffect - The bonus effect to add
	 */
	public void AddBonusEffect(Effect bonusEffect) {
		secondaryEffectRewards.Add(bonusEffect);
	}

	/**
	 * Activate function
	 */
	public override void Activate()
	{
		double extraAP; // The amount of AP to give to the alien

		base.Activate();
		extraAP = Effect.CalculateAP();
		addAP = new StatusTurnEffect(Stat.AP, extraAP, 0, "Alien: Increase AP", "Icons/Effects/APup100px", -1, true);
		addAP.TurnModificationDelegates = new Effect.TurnModifications(NewAPForEffect);
		owner.AttachTurnEffect(changeColour);
		owner.AttachTurnEffect(addAP);
		foreach (Effect bonus in secondaryEffectRewards)
			owner.AttachTurnEffect(bonus);
	}
	/**
	 * Deactivate function
	 */
	public override void Deactivate()
	{
		base.Deactivate();
		owner.DetachTurnEffect(changeColour);
		owner.DetachTurnEffect(addAP);
			foreach (Effect bonus in secondaryEffectRewards)
				owner.DetachTurnEffect(bonus);
	}

	/**
	 * Change the amount of AP added to the player via the status turn effect
	 */
	public void NewAPForEffect() {
		double newAP; // The new amount of AP to give to the alien

		newAP = Effect.CalculateAP();
		addAP.SetValue(newAP);
	}
}
