using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlienPrimaryAbility : Ability {

	Player owner; // The owner of this ability
	Effect changeColour; // The material effect for this alien
	Effect addAP; // The status effect for the alien - giving bonus AP
	List<Effect> secondaryEffectRewards; // Effects that were awarded for completing secondary objectives
	int initialNumberOfTurns; // The initial number of turns
	const string materialPath = "AbilityMaterials/Alien/AlienModeMaterial"; // The alien mode material
	List<AudioClip> transformEfx; // Transform sound effects
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
				"Alien Mode: Turn Into An Alien", "Icons/Effects/alienmodepurple", -1, false);

		secondaryEffectRewards = new List<Effect>();

		transformEfx = new List<AudioClip>();
		transformEfx.Add(Resources.Load<AudioClip>("Audio/Sound Effects/Alien_transform"));
		if (transformEfx.Count != 1) {
			Debug.LogError("Invalid sound effect path for alien transforming");
		}
	}

	/**
	 * Add a bonus secondary effect
	 * 
	 * Arguments
	 * - Effect bonusEffect - The bonus effect to add
	 */
	public void AddBonusEffect(Effect bonusEffect) {
		int existingIndex; // The index of the existing effect

		if (secondaryEffectRewards.Contains(bonusEffect)) { // Need to handle this
			switch (bonusEffect.GetTurnEffectType()) {
			case TurnEffectType.STATEFFECT:
				existingIndex = secondaryEffectRewards.IndexOf(bonusEffect);
				if (existingIndex < 0 || existingIndex > (secondaryEffectRewards.Count - 1))
					throw new System.Exception("Unknown error with alien mode rewards");
				secondaryEffectRewards[existingIndex].SetValue(secondaryEffectRewards[existingIndex].GetValue() 
				                                               + bonusEffect.GetValue());
				break;
			case TurnEffectType.COMPONENTEFFECT: // Don't stack
				break; 
			}

		} else
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
		addAP = new StatusTurnEffect(Stat.AP, extraAP, 0, "Alien: Increase AP", "Icons/Effects/bonusAPALIENpurple", 
				-1, true);
		addAP.TurnModificationDelegates = new Effect.TurnModifications(NewAPForEffect);
		owner.AttachTurnEffect(changeColour);
		owner.AttachTurnEffect(addAP);
		foreach (Effect bonus in secondaryEffectRewards)
			owner.AttachTurnEffect(bonus);
		RemainingTurns = 2;

		SoundManagerScript.Singleton.PlaySingle3D(transformEfx);
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

	public override void ReduceNumberOfTurns()
	{
		ClassPanelScript classPanelScript = ClassPanel.GetComponent<ClassPanelScript>(); // Class panel script

		base.ReduceNumberOfTurns();
		if (RemainingTurns == 0 && IsActive) { // Allow button to be pressed
			classPanelScript.AlienPrimaryAbilityButton.GetComponent<Button>().interactable = true;
		}
	}
}
