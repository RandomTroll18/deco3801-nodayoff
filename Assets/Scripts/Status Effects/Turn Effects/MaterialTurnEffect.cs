using UnityEngine;
using System.Collections;

public class MaterialTurnEffect : Effect {

	Material effectMaterial; // The material to be set from this effect
	string materialPath; // Material path

	/**
	 * The constructor for a material effect
	 * 
	 * Arguments
	 * - string newMaterialPath - The path to the material of this effect
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon (with the Resources folder as the root)
	 * - int turnsActive - Number of turns active
	 * - bool applyPerTurnFlag - set this effect to be applied per turn or not
	 */
	public MaterialTurnEffect(string newMaterialPath, string newDescription, string iconPath, int turnsActive, 
			bool applyPerTurnFlag) {
		SetBasicValues(newDescription, iconPath, turnsActive, applyPerTurnFlag);
		effectMaterial = Resources.Load<Material>(newMaterialPath);
		materialPath = newMaterialPath;
		Type = TurnEffectType.MATERIALEFFECT;
		Debug.Log("Material turn effect constructed");
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
		toReturn += "Material: " + effectMaterial.ToString() + StringMethodsScript.NEWLINE;
		return toReturn;
	}

	/**
	 * Get material for this effect
	 * 
	 * Returns
	 * - The material for this effect
	 */
	public override Material GetMaterial()
	{
		return effectMaterial;
	}

	public override string GetMaterialPath()
	{
		return materialPath;
	}

	/* Override abstruct stuff so that compiler doesn't whine */
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

	public override void SetValue(double newValue)
	{
		throw new System.NotImplementedException();
	}

	public override System.Type GetAffectedItemType()
	{
		throw new System.NotImplementedException();
	}

	public override void ApplyEffectToItem(Item item)
	{
		throw new System.NotImplementedException();
	}
}
