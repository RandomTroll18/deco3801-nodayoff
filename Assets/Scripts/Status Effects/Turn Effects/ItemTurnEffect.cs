using UnityEngine;
using System.Collections;

public class ItemTurnEffect : Effect {

	ItemTurnEffectType itemEffectType; // The kind of effect applied to items
	System.Type affectedItemType; // The item type to affect
	int itemEffectValue; // The value to effect an item by

	/**
	 * The constructor for an item effect
	 * 
	 * Arguments
	 * - System.Type itemToEffect - The item to effect
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon (with the Resources folder as the root)
	 * - int turnsActive - The number of turns active
	 * - ItemTurnEffectType newItemEffectType - The type of effect to apply to items
	 * - double newItemEffectValue - The value to affect the item by
	 * - bool applyPerTurnFlag - set this effect to be applied per turn or not
	 */
	public ItemTurnEffect(System.Type itemToEffect, string newDescription, string iconPath, int turnsActive, 
	                  ItemTurnEffectType newItemEffectType, int newItemEffectValue, bool applyPerTurnFlag) {
		SetBasicValues(newDescription, iconPath, turnsActive, applyPerTurnFlag);
		affectedItemType = itemToEffect;
		itemEffectType = newItemEffectType;
		itemEffectValue = newItemEffectValue;
		Type = TurnEffectType.ITEMEFFECT;
		Debug.Log("Item turn effect construcsted");
	}

	/**
	 * Override toString function
	 * 
	 * Returns
	 * - The string form of this class
	 */
	public override string ToString()
	{
		string toReturn = base.ToString();
		toReturn += "Type of item effect: " + itemEffectType.ToString() + StringMethodsScript.NEWLINE;
		toReturn += "Affected item type: " + affectedItemType.ToString() + StringMethodsScript.NEWLINE;
		return toReturn;
	}

	/**
	 * Get the type of item this turn effect affects
	 * 
	 * Returns
	 * - The type of item this effect affects
	 */
	public override System.Type GetAffectedItemType() {
		return affectedItemType;
	}

	/**
	 * Apply effect to given item
	 * 
	 * Arguments
	 * - Item item - The item to affect
	 */
	public override void ApplyEffectToItem(Item item)
	{
		Debug.Log("Item type to be applied to: " + item.GetType().ToString());
		if (!item.GetType().Equals(affectedItemType)) return; // Don't affect the item
		Debug.Log("Applying");
		switch (itemEffectType) { // Change item stats based on the effect type
		case ItemTurnEffectType.COOLDOWN: // Change cool down
			Debug.Log("Cooldown setting reduced to: " + itemEffectValue);
			item.SetCoolDownSetting(itemEffectValue);
			item.ResetCoolDown();
			break;
		case ItemTurnEffectType.EXTRAUSE: // Change number of uses
			Debug.Log("Use count changed to: " + itemEffectValue);
			item.SetUsePerTurn(itemEffectValue);
			item.SetCurrentNumberOfUses(itemEffectValue);
			break;
		default: break; // Unknown
		}
	}

	/* Override abstract stuff so that compiler doesn't whine */

	public override void SetValue(double newValue)
	{
		throw new System.NotImplementedException();
	}

	public override Material GetMaterial()
	{
		throw new System.NotImplementedException();
	}

	public override int GetMode()
	{
		throw new System.NotImplementedException();
	}

	public override Stat GetStatAffected()
	{
		throw new System.NotImplementedException();
	}

	public override double GetValue()
	{
		throw new System.NotImplementedException();
	}

	public override void SetMode(int newMode)
	{
		throw new System.NotImplementedException();
	}

	public override void SetStatAffected(Stat newStat)
	{
		throw new System.NotImplementedException();
	}
}
