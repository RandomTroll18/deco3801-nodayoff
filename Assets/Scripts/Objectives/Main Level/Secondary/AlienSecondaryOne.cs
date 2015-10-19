using UnityEngine;
using System.Collections;

public class AlienSecondaryOne : SecondaryObjective {

	Effect apEffect; // The vision effect
	
	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}


	// Use this for initialization
	void Start () {
		ObjectiveName = "AlienSecondaryOne";
		Title = "Steal Food Supply";
		Description = "REWARD: extra AP." + StringMethodsScript.NEWLINE;
		GameObject objective = GameObject.Find("Alien Secondary One");
		Location = Tile.TilePosition(objective.transform.position);
		
		AlienSecondaryOneInteractable i = 
			GameObject.Find("Alien Secondary One").AddComponent<AlienSecondaryOneInteractable>();
		i.InstantInteract = true;
		i.StartMe();
		
		apEffect = new StatusTurnEffect(Stat.AP, 10.0, 0, 
		                                    "Extra Food: 10 Extra AP", "Icons/Effects/DefaultEffect", -1, true);
	}
	
	public override void OnComplete()
	{
		Player playerScript; // The player script
		PlayerClass playerClass; // The player's class
		AlienClass alienClassContainer; // Container for the alien's class
		AlienPrimaryAbility alienPrimaryAbilityContainer; // Container for the alien's primary ability

		playerScript = Player.MyPlayer.GetComponent<Player>();
		playerClass = playerScript.GetPlayerClassObject();

		if (playerClass.GetClassTypeEnum() != Classes.BETRAYER)
			throw new System.NotSupportedException("Alien secondary activated by non-alien player");

		alienClassContainer = (AlienClass)playerClass;
		alienPrimaryAbilityContainer = (AlienPrimaryAbility)alienClassContainer.GetPrimaryAbility();

		// Add this effect to the player
		alienPrimaryAbilityContainer.AddBonusEffect(apEffect);
		if (alienPrimaryAbilityContainer.AbilityIsActive()) { // Ability has been activated. Apply effect now
			playerScript.AttachTurnEffect(apEffect);
			playerScript.IncreaseStatValue(Stat.AP, 10.0);
		}
		Destroy(this);
	}
}
