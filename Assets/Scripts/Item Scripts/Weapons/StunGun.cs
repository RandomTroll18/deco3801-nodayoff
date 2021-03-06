using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Class for a Stun Gun - Short Range Weapon
 */
public class StunGun : ShortRangeWeapon {
	const int STUN_DURATION = 2; // The duration for stunning someone

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
	void Start() {
		ItemDescription = "Stun Gun to keep people/aliens in line";

		InstantEffects = null; // No turn based effects

		Rounds = -1.0;
		Damage = 0.0;
		Range = 3.0;
		CoolDown = 0; // No cool down initially
		CoolDownSetting = 3; // Item can only be used once per 3 turns
		DefaultCoolDownSetting = 3; // Default cool down is 3
		CurrentNumberOfUses = 1; // Item hasn't been used yet

		ItemRangeType = RangeType.SQUARERANGE;
		ItemActivationType = ActivationType.OFFENSIVE;

		Activatable = true; // Set this item to be activatable
		Droppable = false; // Set this item to be droppable
	}

	/* Override abstract functions so that compiler doesn't whine */
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	/**
	 * Override the StartAfterInstantiate function
	 */
	public override void StartAfterInstantiate() {
		Start();
	}

	/**
	 * Override the Activate function
	 */
	public override void Activate(Tile targetTile) {
		PhotonPlayer targetNetworkPlayer; // The targetted network player
		Player target; // The target

		if (CurrentNumberOfUses == 0) {
			Debug.Log("Stun Gun has no more uses");
			return; // No more uses
		} else if (CoolDown != 0) {
			Debug.Log("Stun Gun is cooling down");
			return; // Still cooling down
		}

		target = Player.PlayerAtTile(targetTile);

		if (target == null || target == Player.MyPlayer.GetComponent<Player>()) { // No target
			Debug.Log("StunGun: No Target Found");
			return;
		} 

		targetNetworkPlayer = target.GetComponent<PhotonView>().owner;
		target.GetComponent<PhotonView>().RPC("Stun", targetNetworkPlayer, STUN_DURATION); // Found a valid target
		target.GetComponent<PhotonView>().RPC("DisplayStunAnim", PhotonTargets.All, null);
		CurrentNumberOfUses--;
		if (CurrentNumberOfUses == 0) 
			CoolDown = CoolDownSetting; // Set Cool Down
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

		// Next, add range type and activation type
		toReturn += "Range Type: " + EnumsToString.ConvertRangeTypeEnum(ItemRangeType) + StringMethodsScript.NEWLINE;
		toReturn += "Activation Type: " + EnumsToString.ConvertActivationTypeEnum(ItemActivationType) 
				+ StringMethodsScript.NEWLINE;
		
		return toReturn;
	}
}
