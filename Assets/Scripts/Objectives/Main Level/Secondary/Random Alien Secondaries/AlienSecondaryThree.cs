using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienSecondaryThree : SecondaryObjective {

	Effect skillCheckBonusEffect; // The skill check bonus to add
	
	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}
	
	
	// Use this for initialization
	void Start () {
		ObjectiveName = "AlienSecondaryThree";
		Title = "Access Secret Documents";
		Description = "REWARD: +50% to skill check of your human class." + StringMethodsScript.NEWLINE;
		GameObject objective = PickAlienObjective();
		Location = Tile.TilePosition(objective.transform.position);
		
		AlienSecondaryThreeInteractable i = 
			objective.AddComponent<AlienSecondaryThreeInteractable>();
		i.InstantInteract = true;
		i.StartMe();

	}
	
	public override void OnComplete()
	{
		Player playerScript; // The player script
		PlayerClass playerClass; // The player's class
		AlienClass alienClassContainer; // Container for the alien's class
		AlienPrimaryAbility alienPrimaryAbilityContainer; // Container for the alien's primary ability
		Stat statMultiplier; // The stat multiplier to modify
		
		playerScript = Player.MyPlayer.GetComponent<Player>();
		playerClass = playerScript.GetPlayerClassObject();
		
		if (playerClass.GetClassTypeEnum() != Classes.BETRAYER)
			throw new System.NotSupportedException("Alien secondary activated by non-alien player");
		
		alienClassContainer = (AlienClass)playerClass;
		alienPrimaryAbilityContainer = (AlienPrimaryAbility)alienClassContainer.GetPrimaryAbility();

		switch(alienClassContainer.GetHumanClassType()) { // Determine what multiplier to modify
		case Classes.MARINE:
			skillCheckBonusEffect = new StatusMultiplierTurnEffect(Stat.MARINEMULTIPLIER, 0.5, 0, 
					"+50% Marine Skill Check", "Icons/Effects/skillchkmarpurple", -1, true);
			statMultiplier = Stat.MARINEMULTIPLIER;
			break;
		case Classes.ENGINEER:
			skillCheckBonusEffect = new StatusMultiplierTurnEffect(Stat.ENGMULTIPLIER, 0.5, 0, 
					"+50% Engineer Skill Check", "Icons/Effects/skillchkengpurple", -1, true);
			statMultiplier = Stat.ENGMULTIPLIER;
			break;
		case Classes.SCOUT:
			skillCheckBonusEffect = new StatusMultiplierTurnEffect(Stat.SCOUTMULTIPLIER, 0.5, 0, 
					"+50% Scout Skill Check", "Icons/Effects/skillchkscoutpurple", -1, true);
			statMultiplier = Stat.SCOUTMULTIPLIER;
			break;
		case Classes.TECHNICIAN:
			skillCheckBonusEffect = new StatusMultiplierTurnEffect(Stat.TECHMULTIPLIER, 0.5, 0, 
					"+50% Technician Skill Check", "Icons/Effects/skillchktechpurple", -1, true);
			statMultiplier = Stat.TECHMULTIPLIER;
			break;
		default: // Invalid class
			throw new System.NotSupportedException("Invalid human class");
		}
		
		// Add this effect to the player
		alienPrimaryAbilityContainer.AddBonusEffect(skillCheckBonusEffect);
		if (alienPrimaryAbilityContainer.AbilityIsActive()) { // Ability has been activated. Apply effect now
			playerScript.AttachTurnEffect(skillCheckBonusEffect);
			alienClassContainer.IncreaseStatMultiplierValue(statMultiplier, 0.5);
		}
		Destroy(this);
		PickNewAlienObjective();
	}
}
