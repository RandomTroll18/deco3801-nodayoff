using UnityEngine;
using System.Collections;

public class AlienSecondarySeven : SecondaryObjective {
	public static bool completed = false;

	AlienSecondarySevenInteractable interactable;
	ComponentTurnEffect stealthEffect; // Stealth effect

	// Use this for initialization
	void Start () {
		Log();
		
		GameObject objective = PickAlienObjective();
		Title = "Invisibility";
		Description = "An invisibility enhancement has been deployed for you. Find it to give" +
			" yourself an advantage over the humans.";
		Location = Tile.TilePosition(objective.transform.position);

		
		interactable 
			= objective.AddComponent<AlienSecondarySevenInteractable>();
		interactable.InstantInteract = true;
		interactable.StartMe();

		stealthEffect = new ComponentTurnEffect(
				ComponentEffectType.STEALTH, 
				"Alien Secondary Objective 7: Stealth Reward", 
				"Icons/Effects/stealthapppurple", -1, false
		);
	}
	
	public override void OnComplete() {
		Player playerScript; // The player script
		AlienClass alienClass; // Alien class container
		AlienPrimaryAbility alienAbility; // Alien ability container

		completed = true;
		playerScript = Player.MyPlayer.GetComponent<Player>();

		if (playerScript.GetPlayerClassObject().GetClassTypeEnum() != Classes.BETRAYER)
			throw new System.ArgumentException("Invalid class who completed alien class");

		alienClass = (AlienClass)playerScript.GetPlayerClassObject();
		alienAbility = (AlienPrimaryAbility)alienClass.GetPrimaryAbility();
		alienAbility.AddBonusEffect(stealthEffect);
		if (alienAbility.AbilityIsActive()) {
			playerScript.AttachTurnEffect(stealthEffect);
			Player.MyPlayer.GetComponent<Stealth>().Permanent = true;
		}

		Destroy(this);
		completed = true;
		
		string message = "Alien has obtained an invisibility enhancement from " + 
			interactable.GetComponent<Location>().ToString() + ". They are now invisible" +
			" when in alien mode.";
		string messageTitle = "Alien Activity";
		string image = "ui/events/body";
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message, messageTitle, image);

		GameObject pod1;
		GameObject pod2;
		// TODO: confirm this works
		if (gameObject.name.Equals("EscapePod 3") || gameObject.name.Equals("EscapePod 4")) {
			pod1 = GameObject.Find("EscapePod 3");
			pod2 = GameObject.Find("EscapePod 4");
		} else {
			pod1 = GameObject.Find("EscapePod 1");
			pod2 = GameObject.Find("EscapePod 2");
		}
		
		pod1.GetComponent<PhotonView>().RPC("PhotonDestroy", PhotonTargets.All);
		pod2.GetComponent<PhotonView>().RPC("PhotonDestroy", PhotonTargets.All);
	}
}
