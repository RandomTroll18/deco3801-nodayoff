using UnityEngine;
using System.Collections;

public class EngineerSecondaryOne : SecondaryObjective {

	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}
	
	void Start() {
		Log();
		ObjectiveName = "Engineer Secondary One";
		Title = "Improve Robot";
		Description = "Find better coolant for your robot. A Marine could open the door quicker" +
			"for you.\n" +
			"REWARD: No cooldown for your robot.";
		GameObject objective = GameObject.FindGameObjectWithTag("Eng Secondary");
		Location = Tile.TilePosition(objective.transform.position);
		
		EngineerSecondaryOneInteractable i = 
			GameObject.Find("Engineer Secondary One").AddComponent<EngineerSecondaryOneInteractable>();
		i.InstantInteract = true;
		i.StartMe();
	}
	
	public override void OnComplete() {
		PlayerClass playerClass = Player.MyPlayer.GetComponent<Player>().GetPlayerClassObject(); // Player class
		AlienClass alienClass; // Alien class container
		EngineerPrimaryAbility engAbility; // Engineer ability
		if (playerClass.GetClassTypeEnum() == Classes.BETRAYER) { // Alien
			alienClass = (AlienClass)playerClass;
			engAbility = (EngineerPrimaryAbility)alienClass.GetHumanClass().GetPrimaryAbility();
		} else if (playerClass.GetClassTypeEnum() == Classes.ENGINEER) // Human Engineer
			engAbility = (EngineerPrimaryAbility)playerClass.GetPrimaryAbility();
		else // Invalid class
			throw new System.NotSupportedException("Not an engineer");

		engAbility.SetCoolDown(0); // No cool down
		if (!engAbility.AbilityIsActive()) { // Instantly cool down this ability
			while (engAbility.RemainingNumberOfTurns() != 0)
				engAbility.ReduceNumberOfTurns();
		}
		Destroy(this);
		base.OnComplete();
	}
}
