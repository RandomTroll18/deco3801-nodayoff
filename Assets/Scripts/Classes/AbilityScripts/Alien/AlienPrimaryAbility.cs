using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlienPrimaryAbility : Ability {

	Player owner; // The owner of this ability
	Effect changeModel; // The Change model effect for this alien
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
		GameObject alienModel = null, humanModel = null; // The models of the player

		AbilityName = "Alien Mode";
		Range = 0.0; // No range
		AbilityRangeType = RangeType.GLOBALTARGETRANGE;
		AbilityActivationType = ActivationType.SUPPORTIVE;

		owner = player;

		/*
		 * For the model, need to do many things, such as getting the game object of 
		 * the player's model
		 */
		foreach (Transform objectTransform in player.gameObject.transform) {
			if (objectTransform.gameObject.name.Equals("HybridHuman")) // Found human model
				humanModel = objectTransform.gameObject;
			else if (objectTransform.gameObject.name.Equals("alien")) // Found alien model
				alienModel = objectTransform.gameObject;

			if (alienModel != null && humanModel != null)
				break;
		}
		changeModel = new ModelChangeTurnEffect(alienModel, humanModel, owner.gameObject, "Alien Mode: Shape Change", 
				"Icons/Effects/alienmodepurple", -1, false);

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
			case TurnEffectType.STATEFFECT: // We are adding a stat effect bonus. Stacking it
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
		base.Activate(); // Set this ability to be active

		/* Need to create a new instance of the added AP status effect */
		addAP = new StatusTurnEffect(Stat.AP, 10.0, 0, "Alien: Increase AP", "Icons/Effects/bonusAPALIENpurple", 
				-1, true);

		/* Attach turn effects*/
		owner.AttachTurnEffect(changeModel);
		owner.AttachTurnEffect(addAP);

		/* Attach bonus effects */
		foreach (Effect bonus in secondaryEffectRewards)
			owner.AttachTurnEffect(bonus);

		RemainingTurns = 2; // Need to wait for 2 turns before deactivating alien mode

		SoundManagerScript.Singleton.PlaySingle3D(transformEfx);
	}
	/**
	 * Deactivate function
	 */
	public override void Deactivate()
	{
		base.Deactivate(); // Set this ability to be inactive

		/* Detaching turn effects */
		owner.DetachTurnEffect(changeModel);
		owner.DetachTurnEffect(addAP);
			foreach (Effect bonus in secondaryEffectRewards)
				owner.DetachTurnEffect(bonus);
	}

	/**
	 * Change the amount of AP added to the player via the status turn effect
	 */
	public void NewAPForEffect() {
		double newAP; // The new amount of AP to give to the alien

		newAP = Effect.CalculateAP(); // Need to calculate the new amount of AP to give
		addAP.SetValue(newAP);
	}

	public override void ReduceNumberOfTurns()
	{
		ClassPanelScript classPanelScript = ClassPanel.GetComponent<ClassPanelScript>(); // Class panel script

		base.ReduceNumberOfTurns(); // Just reduce number of turns first
		if (RemainingTurns == 0 && IsActive) // Allow button to be pressed
			classPanelScript.AlienPrimaryAbilityButton.GetComponent<Button>().interactable = true;
	}
}
