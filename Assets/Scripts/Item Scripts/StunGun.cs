using UnityEngine;
using System.Collections;

/**
 * Class for a Stun Gun - Short Range Weapon
 */
public class StunGun : ShortRangeWeapon {

	/**
	 * Stun Gun is simply a weapon that inflicts stun on the target.
	 * This weapon does no damage, has a range of 1 square and has 
	 * unlimited rounds. 
	 * 
	 * This item can only be used once per 2 turns
	 * 
	 * This is an Offensive type weapon whose range will take the 
	 * form of a square
	 */
	void Start () {
		ItemDescription = "Stun Gun to keep people/aliens in line";

		Effects = null; // No turn based effects

		Rounds = -1.0;
		Damage = 0.0;
		Range = 1.0;
		CoolDown = 0; // No cool down initially
		CoolDownSetting = 3; // Item can only be used once per 3 turns

		ItemRangeType = RangeType.SQUARERANGE;
		ItemActivationType = ActivationType.OFFENSIVE;
	}

	/**
	 * Override the StartAfterInstantiate function
	 */
	public override void StartAfterInstantiate() {
		base.StartAfterInstantiate();
		Start();
	}

	/**
	 * Override the Activate function
	 */
	public override void Activate() {
		base.Activate();
		if (CoolDown != 0) {
			Debug.Log("Item still cooling down");
			return;
		}

		Debug.Log("Stun Gun Done Activating");
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
		
		// Next, add the amount of damage this weapon does
		toReturn += "Damage: " + Damage + StringMethodsScript.NEWLINE;
		
		// Next, add the range of this weapon
		toReturn += "Range: " + Range + StringMethodsScript.NEWLINE;
		
		// Next, add the number of rounds left and return the final string
		toReturn += "Rounds Left: " + Rounds + StringMethodsScript.NEWLINE;
		
		// Next, add the number of turns this item can be used at a time
		toReturn += "Cool Down Turns: " + CoolDownSetting + StringMethodsScript.NEWLINE;

		// Next, add range type and activation type
		toReturn += "Range Type: " + EnumsToString.ConvertRangeTypeEnum(ItemRangeType) + StringMethodsScript.NEWLINE;
		toReturn += "Activation Type: " + EnumsToString.ConvertActivationTypeEnum(ItemActivationType) 
				+ StringMethodsScript.NEWLINE;
		
		return toReturn;
	}
}
