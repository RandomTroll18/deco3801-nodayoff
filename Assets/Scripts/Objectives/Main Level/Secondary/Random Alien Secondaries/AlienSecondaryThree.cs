using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienSecondaryThree : SecondaryObjective {

	Effect[] skillCheckBonusEffect; // The skill check bonus to add
	AlienSecondaryThreeInteractable interactable;
	
	public override void InitializeObjective()
	{
		base.InitializeObjective();
//		Start();
	}
	
	
	// Use this for initialization
	public void StartMe() {
		Log();
		ObjectiveName = "AlienSecondaryThree";
		Title = "Obtain Enhancements";
		Description = "One of the boarding parties has dropped enhancements for you.\n" +
			"REWARD: +50% to skill check of your human class.";
		GameObject objective = PickAlienObjective();
		Location = Tile.TilePosition(objective.transform.position);
		
		interactable = 
			objective.AddComponent<AlienSecondaryThreeInteractable>();
		interactable.InstantInteract = true;
		interactable.StartMe();

		// Initialize list of skill check bonus effects
		skillCheckBonusEffect = new Effect[4];
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

		skillCheckBonusEffect[0] = new StatusMultiplierTurnEffect(Stat.MARINEMULTIPLIER, 0.5, 0, 
				"+50% Marine Skill Check", "Icons/Effects/skillchkmarpurple", -1, true);
		skillCheckBonusEffect[1] = new StatusMultiplierTurnEffect(Stat.ENGMULTIPLIER, 0.5, 0, 
				"+50% Engineer Skill Check", "Icons/Effects/skillchkengpurple", -1, true);
		skillCheckBonusEffect[2] = new StatusMultiplierTurnEffect(Stat.SCOUTMULTIPLIER, 0.5, 0, 
				"+50% Scout Skill Check", "Icons/Effects/skillchkscoutpurple", -1, true);
		skillCheckBonusEffect[3] = new StatusMultiplierTurnEffect(Stat.TECHMULTIPLIER, 0.5, 0, 
		        "+50% Technician Skill Check", "Icons/Effects/skillchktechpurple", -1, true);

		foreach (Effect bonusEffect in skillCheckBonusEffect) {
			// Add this effect to the player
			alienPrimaryAbilityContainer.AddBonusEffect(bonusEffect);
			if (alienPrimaryAbilityContainer.AbilityIsActive()) { // Ability has been activated. Apply effect now
				playerScript.AttachTurnEffect(bonusEffect);
				alienClassContainer.IncreaseStatMultiplierValue(skillCheckBonusEffect[0].GetStatAffected(), 0.5);
			}
		}

		Destroy(this);
		PickNewAlienObjective();

		string message = "Alien has obtained enhancements from " + 
			interactable.GetComponent<Location>().ToString() + "." +
				" They now have higher stats.";
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message);

		Destroy(interactable);
	}
}
