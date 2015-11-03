using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 2nd part to the objective that lets the alien destroy escape pods.
 */
public class AlienSecondarySix : SecondaryObjective {
	List<AlienSecondarySixInteractable> interactables = new List<AlienSecondarySixInteractable>();

	// Use this for initialization
	void Start () {
		Log();
		
		GameObject[] objectives = GameObject.FindGameObjectsWithTag("Escape Pod");
		Title = "Destroy Pods: Part 2";
		Description = "You have the explosives. Destroy one of the escape pod pairs.";

		foreach (GameObject objective in objectives) {
			AlienSecondarySixInteractable interactable 
					= objective.AddComponent<AlienSecondarySixInteractable>();
			interactable.InstantInteract = true;
			interactable.StartMe();
			interactables.Add(interactable);
		}
	}
	
	public override void OnComplete() {
		Destroy(this);
		
		string message = "Alien has destroyed two escape pods. There are no longer enough escape pods" +
			" for all survivors to escape";
		string messageTitle = "Alien Activity";
		string image = "ui/events/explosion2";
		Object.FindObjectOfType<GameManager>()
				.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message, messageTitle, image);

		/*
		 * Figure out which pair of pods needs to blow up
		 */
		GameObject pod1;
		GameObject pod2;
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
