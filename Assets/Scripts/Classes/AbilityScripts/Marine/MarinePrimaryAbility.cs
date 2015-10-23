using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MarinePrimaryAbility : Ability {

	Player master; // Owning player
	Material defaultPlayerMaterial; // The player's default material
	Material abilityActivePlayerMaterial; // Material used when ability is active
	Effect materialEffect; // The material effect to add to the player
	Effect stunGunEffect; // The stun gun effect to add to the player
	Effect noCoolDownEffect; // Removal of stun gun cooldown effect
	int coolDown; // The number of rounds this ability will be cooling down

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
		coolDown = 3;
		master = player;
	}

	/**
	 * Activate function
	 */
	public override void Activate()
	{
		base.Activate();
		// Create turn effects
		materialEffect = new MaterialTurnEffect("AbilityMaterials/Marine/MarinePrimAbilityMaterial", 
				"Stimulus Debris: Immune To Stun", "Icons/Effects/stunimmunitygreen", 2, false);
		stunGunEffect = new ItemTurnEffect(typeof(StunGun), "Stimulus Debris: Extra Stun Gun Charges", 
		        "Icons/Effects/extrachargesgreen", 2, ItemTurnEffectType.EXTRAUSE, 3, false);
		noCoolDownEffect = new ItemTurnEffect(typeof(StunGun), "Stimulus Debris: No Stun Gun Cool Down", 
		        "Icons/Effects/stuninstantCDgreen", 2, ItemTurnEffectType.COOLDOWN, 1, false);
		master.SetStunImmunity(true); // Player is now stunned
		master.AttachTurnEffect(materialEffect);
		master.AttachTurnEffect(stunGunEffect);
		master.AttachTurnEffect(noCoolDownEffect);
		RemainingTurns = 3;
	}

	/**
	 * Reduce number of turns left
	 */
	public override void ReduceNumberOfTurns()
	{
		ClassPanelScript classPanelScript = ClassPanel.GetComponent<ClassPanelScript>(); // The class panel script

		if (RemainingTurns != 0)
			RemainingTurns--;
		Debug.Log("Marine ability remaining turns: " + RemainingTurns);
		if (RemainingTurns == 0) {
			if (IsActive) { // Reset ability to go to cooldown
				master.SetStunImmunity(false); // No longer immune
				RemainingTurns = coolDown;
				IsActive = false;
				classPanelScript.PrimaryAbilityText.text = "Cooling Down";
			} else {  // No longer in cool down
				classPanelScript.PrimaryAbilityButton.GetComponent<Button>().interactable = true;
				classPanelScript.PrimaryAbilityText.text = "Stimulus Debris";
			}

		}
	}
}
