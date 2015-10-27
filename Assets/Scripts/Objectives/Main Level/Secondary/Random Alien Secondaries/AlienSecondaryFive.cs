using UnityEngine;
using System.Collections;

/*
 * Part 1 of escape pod loss.
 */
public class AlienSecondaryFive : SecondaryObjective {
	AlienSecondaryFiveInteractable interactable;


	// Use this for initialization
	public void StartMe() {
		Log();

		GameObject objective = PickAlienObjective();
		Title = "Destroy Pods: Part 1";
		Description = "Obtain the explosives. These can then be used to destroy a pair of escape" +
			"pods.";
		Location = Tile.TilePosition(objective.transform.position);

		interactable = objective.AddComponent<AlienSecondaryFiveInteractable>();
		interactable.InstantInteract = true;
		interactable.StartMe();
	}
	
	public override void OnComplete() {
		Destroy(this);
		
		string message = "Alien has obtained explosives that can be used on the escape pods. Don't" +
			" let them get to either of the escape pods.";
		string title = "Alien Activity";
		string location = "ui/events/bombing";
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message, title);
		
		Destroy(interactable);

		GameObject secondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;
		secondaries.AddComponent<AlienSecondarySix>();
	}
}
