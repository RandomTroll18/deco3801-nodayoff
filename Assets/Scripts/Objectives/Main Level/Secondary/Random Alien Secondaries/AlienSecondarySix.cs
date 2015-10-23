using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		
		string message = "Alien has destroyed two escape pods. There is no longer enough escape pods" +
			"for all survivors to escape";
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
