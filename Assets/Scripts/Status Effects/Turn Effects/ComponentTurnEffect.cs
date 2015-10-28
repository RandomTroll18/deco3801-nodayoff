using UnityEngine;
using System.Collections;

public class ComponentTurnEffect : Effect {

	ComponentEffectType componentType; // The type of this component effect
	int range = 0; // The range of this effect

	/**
	 * Constructor
	 * 
	 * - ComponentEffectType type - The type of component turn effect
	 * - string newDescription - The description
	 * - string iconPath - The path to the icon
	 * - int turnsActive - The number of turns this effect should be active
	 * - bool applyTurnFlag - set this effect to be applied per turn
	 */
	public ComponentTurnEffect(ComponentEffectType newComponentType, string newDescription, string iconPath, 
	       	int turnsActive, bool applyTurnFlag) {
		SetBasicValues(newDescription, iconPath, turnsActive, applyTurnFlag);
		Type = TurnEffectType.COMPONENTEFFECT;
		componentType = newComponentType;
	}

	/**
	 * Constructor. Can pass range
	 * 
	 * - ComponentEffectType type - The type of component turn effect
	 * - string newDescription - The description
	 * - string iconPath - The path to the icon
	 * - int turnsActive - The number of turns this effect should be active
	 * - int newRange - The range of the effect
	 * - bool applyTurnFlag - set this effect to be applied per turn
	 */
	public ComponentTurnEffect(ComponentEffectType newComponentType, string newDescription, string iconPath, 
	                           int turnsActive, int newRange, bool applyTurnFlag) {
		SetBasicValues(newDescription, iconPath, turnsActive, applyTurnFlag);
		Type = TurnEffectType.COMPONENTEFFECT;
		componentType = newComponentType;
		range = newRange;
	}

	/**
	 * We have to do a more complex attach routine
	 */
	public override void ExtraAttachActions()
	{
		switch (componentType) {
		case ComponentEffectType.STEALTH: // Stealth component
			if (Player.MyPlayer.GetComponent<Stealth>() == null)
				Player.MyPlayer.AddComponent<Stealth>(); // Add component
			Player.MyPlayer.GetComponent<PhotonView>().RPC("SetStealth", PhotonTargets.All, 
				                                               new object[] {true, true});
			break;
		case ComponentEffectType.TRAPDETECTOR: // Trap detector component
			if (Player.MyPlayer.GetComponent<TrapDetectingComponent>() == null) {
				Player.MyPlayer.AddComponent<TrapDetectingComponent>(); // Add component
				Player.MyPlayer.GetComponent<TrapDetectingComponent>().Range = range;
			}
			Player.MyPlayer.GetComponent<TrapDetectingComponent>().enabled = true;
			break;
		case ComponentEffectType.INVISIBILITYDETECTOR: // Invisibility detector component
			if (Player.MyPlayer.GetComponent<InvisibilityDetectingComponent>() == null) {
				Player.MyPlayer.AddComponent<InvisibilityDetectingComponent>(); // Add component
				Player.MyPlayer.GetComponent<InvisibilityDetectingComponent>().Range = range;
			}
			Player.MyPlayer.GetComponent<InvisibilityDetectingComponent>().enabled = true;
			break;
		default: // Unknown
			break;
		}
	}

	/**
	 * We have to do a more complex detach routine
	 */
	public override void ExtraDetachActions()
	{
		switch (componentType) {
		case ComponentEffectType.STEALTH: // Stealth component
			// Disable stealth
			if (Player.MyPlayer.GetComponent<Stealth>() != null) {
				Player.MyPlayer.GetComponent<PhotonView>().RPC("SetStealth", PhotonTargets.All, 
						new object[] {false, false});
			}
			Player.MyPlayer.GetComponentInChildren<MeshRenderer>().enabled = true;
			break;
		case ComponentEffectType.TRAPDETECTOR: // Trap detector component
			if (Player.MyPlayer.GetComponent<TrapDetectingComponent>() != null) {
				Player.MyPlayer.GetComponent<TrapDetectingComponent>().enabled = false; 
				Player.MyPlayer.GetComponent<TrapDetectingComponent>().MakeTrapsInvisible();
				Object.Destroy(Player.MyPlayer.GetComponent<TrapDetectingComponent>());
			}
			break;
		case ComponentEffectType.INVISIBILITYDETECTOR: // Invisibility detector component
			if (Player.MyPlayer.GetComponent<InvisibilityDetectingComponent>() != null) {
				Player.MyPlayer.GetComponent<InvisibilityDetectingComponent>().enabled = false;
				Player.MyPlayer.GetComponent<InvisibilityDetectingComponent>().MakeInvisible();
				Object.Destroy(Player.MyPlayer.GetComponent<InvisibilityDetectingComponent>());
			}
			break;
		default: // Unknown
			break;
		}
	}

	/* Pacify compiler */
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

	public override void SetValue(double newValue)
	{
		throw new System.NotImplementedException();
	}

	public override void SetStatAffected(Stat newStat)
	{
		throw new System.NotImplementedException();
	}

	public override void SetMode(int newMode)
	{
		throw new System.NotImplementedException();
	}
}
