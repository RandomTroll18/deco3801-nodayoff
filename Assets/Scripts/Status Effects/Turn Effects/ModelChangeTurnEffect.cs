using UnityEngine;
using System.Collections;

public class ModelChangeTurnEffect : Effect {

	GameObject toSetActive; // The model to set active
	GameObject original; // The original model to set active
	GameObject target; // The target

	/**
	 * The constructor for a material effect
	 * 
	 * Arguments
	 * - GameObject newModel - The new model
	 * - GameObject oldModel - the old/original model
	 * - GameObject newTarget - The target
	 * - string newDescription - The description of this turn effect
	 * - string iconPath - The path to the icon (with the Resources folder as the root)
	 * - int turnsActive - Number of turns active
	 * - bool applyPerTurnFlag - set this effect to be applied per turn or not
	 */
	public ModelChangeTurnEffect(GameObject newModel, GameObject oldModel, GameObject newTarget, string newDescription, 
			string iconPath, int turnsActive, bool applyPerTurnFlag) {
		SetBasicValues(newDescription, iconPath, turnsActive, applyPerTurnFlag);
		toSetActive = newModel;
		original = oldModel;
		target = newTarget;
		Type = TurnEffectType.MODELCHANGEEFFECT;
		Debug.Log("Material turn effect constructed");
	}

	public override void ExtraAttachActions()
	{
		switch (target.tag) { // Need to determine what type of object this is
		case "Player": // Player
			target.GetComponent<PhotonView>().RPC("SetAttachedObjects", 
					PhotonTargets.All, 
			        new object[] {toSetActive.name, original.name}
			);
			break;
		}
	}

	public override void ExtraDetachActions()
	{
		switch (target.tag) { // Need to determine what type of object this is
		case "Player": // Player
			target.GetComponent<PhotonView>().RPC("SetAttachedObjects", 
					PhotonTargets.All, 
					new object[] {original.name, toSetActive.name}
			);
			break;
		}
	}

	/* Abstract functions so that compiler doesn't whine */
	public override void ApplyEffectToItem(Item item)
	{
		throw new System.NotImplementedException();
	}

	public override System.Type GetAffectedItemType()
	{
		throw new System.NotImplementedException();
	}

	public override Material GetMaterial()
	{
		throw new System.NotImplementedException();
	}

	public override string GetMaterialPath()
	{
		return base.GetMaterialPath();
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

	public override void SetValue(double newValue)
	{
		throw new System.NotImplementedException();
	}
}
