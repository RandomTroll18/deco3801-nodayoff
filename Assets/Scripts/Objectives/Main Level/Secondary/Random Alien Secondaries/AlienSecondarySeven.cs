using UnityEngine;
using System.Collections;

public class AlienSecondarySeven : SecondaryObjective {
	public static bool completed = false;

	AlienSecondarySevenInteractable interactable;

	// Use this for initialization
	void Start () {
		Log();
		
		GameObject objective = PickAlienObjective();
		Title = "Invisibility";
		Description = "An invisibility enhancement has been deployed for you. Find it to give" +
			" yourself an advantage over the humans.";
		
		interactable 
			= objective.AddComponent<AlienSecondarySevenInteractable>();
		interactable.InstantInteract = true;
		interactable.StartMe();
	}
	
	public override void OnComplete() {
		completed = true;
		Destroy(this);
		completed = true;
		
		string message = "Alien has obtained an invisibility enhancement from " + 
			interactable.GetComponent<Location>().ToString() + ". They are now invisible" +
			" when in alien mode.";
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message);
		
		int num = 0;
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
