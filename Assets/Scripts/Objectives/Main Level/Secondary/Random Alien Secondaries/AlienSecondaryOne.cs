using UnityEngine;
using System.Collections;

public class AlienSecondaryOne : SecondaryObjective {

	Effect apEffect; // The vision effect
	AlienSecondaryOneInteractable interactable;
	
	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}


	// Use this for initialization
	void Start () {
		Log();
		ObjectiveName = "AlienSecondaryOne";
		Title = "Steal Food Supply";
		Description = "REWARD: extra AP." + StringMethodsScript.NEWLINE;
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
//		PickNewAlienObjective();

		string message = "Alien has stolen secret documents from " + 
			interactable.GetComponent<Location>().MyLocation.ToString();
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message);
		
		Destroy(interactable);
	}
}
