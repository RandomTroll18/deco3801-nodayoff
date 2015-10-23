using UnityEngine;
using System.Collections;

public class AlienSecondaryTwo : SecondaryObjective {

	Effect visionEffect; // The vision effect
	AlienSecondaryTwoInteractable interactable;
	
	public override void InitializeObjective()
	{
		base.InitializeObjective();
//		Start();
	}
	
	
	// Use this for initialization
	public void StartMe() {
		Log();
		ObjectiveName = "AlienSecondaryTwo";
		Title = "Disrupt Generator";
		Description = "REWARD: extra Vision due to dimmed lights." + StringMethodsScript.NEWLINE;
		GameObject objective = PickAlienObjective();
		Location = Tile.TilePosition(objective.transform.position);
		
		interactable = 
			objective.AddComponent<AlienSecondaryTwoInteractable>();
		interactable.InstantInteract = true;
		interactable.StartMe();
		
		visionEffect = new StatusTurnEffect(Stat.VISION, 3.0, 1, 
		                                "Dimmed Lights: Vision set to 3", "Icons/Effects/alienvisionpurple", -1, true);
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
		alienPrimaryAbilityContainer.AddBonusEffect(visionEffect);
		if (alienPrimaryAbilityContainer.AbilityIsActive()) { // Ability has been activated. Apply effect now
			playerScript.AttachTurnEffect(visionEffect);
			playerScript.SetStatValue(Stat.VISION, 3.0);
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
