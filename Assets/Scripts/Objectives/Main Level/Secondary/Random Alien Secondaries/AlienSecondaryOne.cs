using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienSecondaryOne : SecondaryObjective {

	Effect apEffect; // The vision effect
	AlienSecondaryOneInteractable interactable;
	
	public override void InitializeObjective()
	{
		base.InitializeObjective();
//		Start();
	}


	// Use this for initialization
	public void StartMe() {
		Log();
		ObjectiveName = "AlienSecondaryOne";
		Title = "Steal Food Supply";
		Description = "Steal the food supply from the store room.\n" +
			"REWARD: Extra AP per turn while in alien mode." + StringMethodsScript.NEWLINE;
		GameObject objective = PickAlienObjective();
		Location = Tile.TilePosition(objective.transform.position);
		
		interactable = 
			objective.AddComponent<AlienSecondaryOneInteractable>();
		interactable.InstantInteract = true;
		interactable.StartMe();
		
		apEffect = new StatusTurnEffect(Stat.AP, 10.0, 0, "Extra Food: 10 Extra AP", "Icons/Effects/bonusAPALIENpurple", 
				-1, true);
	}
	
	public override void OnComplete()
	{
		Player playerScript; // The player script
		PlayerClass playerClass; // The player's class
		AlienClass alienClassContainer; // Container for the alien's class
		AlienPrimaryAbility alienPrimaryAbilityContainer; // Container for the alien's primary ability
		Effect existingEffect; // The existing effect

		playerScript = Player.MyPlayer.GetComponent<Player>();
		playerClass = playerScript.GetPlayerClassObject();


		if (playerClass.GetClassTypeEnum() != Classes.BETRAYER)
			throw new System.NotSupportedException("Alien secondary activated by non-alien player");

		alienClassContainer = (AlienClass)playerClass;
		alienPrimaryAbilityContainer = (AlienPrimaryAbility)alienClassContainer.GetPrimaryAbility();

		alienPrimaryAbilityContainer.AddBonusEffect(apEffect);
		if (alienPrimaryAbilityContainer.AbilityIsActive()) { // Ability has been activated. Apply effect now
			playerScript.AttachTurnEffect(apEffect);
			playerScript.IncreaseStatValue(Stat.AP, 10.0);
		}
		Destroy(this);
//		PickNewAlienObjective();

		string message = "Alien has stolen our food supplies from " + 
			interactable.GetComponent<Location>().ToString() + ".\n" +
				"They now have extra AP per turn.";
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message);
		
		Destroy(interactable);
	}
}
