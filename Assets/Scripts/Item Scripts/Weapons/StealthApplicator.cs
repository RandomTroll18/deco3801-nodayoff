using UnityEngine;
using System.Collections;

public class StealthApplicator : ShortRangeWeapon {
	const int STEALTH_DURATION = 3; // Duration of the stealth
	Effect stealthEffect; // The stealth effect

	void Start() {
		ItemDescription = "Stealth Applicator. Make yourself invisible";
		
		InstantEffects = null; // No turn based effects
		
		Rounds = -1.0;
		Damage = 0.0;
		Range = 0.0;
		CoolDown = 0; // No cool down initially
		CoolDownSetting = 6; // Item can only be used once per 6 turns
		DefaultCoolDownSetting = 6; // Default cool down is 6
		CurrentNumberOfUses = 1; // Item hasn't been used yet
		UsePerTurn = 1; 

		Activatable = true; // Set this item to be activatable
		Droppable = true; // Set this item to be droppable

		stealthEffect = new ComponentTurnEffect(
				ComponentEffectType.STEALTH, 
		        "Stealth Applicator: You are currently hidden", 
				"Icons/Effects/stealthappgreen", STEALTH_DURATION, false
		);
	}

	/**
	 * Activate. No targetting required
	 */
	public override void Activate()
	{
		Player playerScript = Player.MyPlayer.GetComponent<Player>(); // The player's script

		if (CoolDown != 0) {
			Debug.Log("Stealth Applicator is cooling down");
			return; // Still cooling down
		}

		if (playerScript.GetTurnEffects().Contains(stealthEffect)) // Already contains it. don't do anything
			return;

		playerScript.AttachTurnEffect(stealthEffect);
		if (SoundManagerScript.Singleton != null) { /* Play activated sound effect */
			// Move sound manager to this object
			SoundManagerScript.Singleton.gameObject.transform.position = gameObject.transform.position;
			SoundManagerScript.Singleton.PlaySingle3D(ActivateEfx);
		}

		CurrentNumberOfUses--;
		if (CurrentNumberOfUses == 0) 
			CoolDown = CoolDownSetting; // Set Cool Down
	}

	/**
	 * Need to override cool down reducer for enabling/disabling components
	 */
	public override void ReduceCoolDown()
	{
		if (CoolDown != 0) 
			CoolDown--;

		if (CoolDown == 0) // Cool down done
			CurrentNumberOfUses = UsePerTurn;
			
	}

	/* Pacify abstract class */
	public override void Activate(Tile targetTile)
	{
		throw new System.NotImplementedException();
	}

	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
	}

	/**
	 * Override toString function
	 * 
	 * Returns
	 * - A string providing info on this item
	 */
	public override string ToString() {
		// The string to return. Start with the name
		string toReturn = "Item Name: " + ItemName + StringMethodsScript.NEWLINE;
		
		// Next, add item description
		toReturn += "Description: " + ItemDescription + StringMethodsScript.NEWLINE;

		// Next, add the range of this weapon
		toReturn += "Range: " + Range + StringMethodsScript.NEWLINE;
		
		// Next, add the number of turns this item can be used at a time
		toReturn += "Cool Down Turns: " + CoolDownSetting + StringMethodsScript.NEWLINE;
		toReturn += "Rounds Left For Cooling Down: " + CoolDown + StringMethodsScript.NEWLINE;
		
		return toReturn;
	}
	
}
