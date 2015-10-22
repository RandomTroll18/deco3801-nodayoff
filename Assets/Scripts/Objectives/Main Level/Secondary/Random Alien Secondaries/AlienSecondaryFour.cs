﻿using UnityEngine;
using System.Collections;

/*
 * Random boarding party added to map
 */
public class AlienSecondaryFour : SecondaryObjective {
	AlienSecondaryFourInteractable interactable;

	GameObject PickBoardingParty() {
		GameObject[] spawns = GameObject.FindGameObjectsWithTag("Boarding Party");
		int spawn = Random.Range(0, spawns.Length - 1);
		return spawns[spawn];
	}

	// Use this for initialization
	void Start () {
		Log();
		GameObject objective = PickAlienObjective();
		Title = "Summon Boarding Party";
		Description = "A Versipellis boarding party is at the ready. Shutdown the ship's defenses" +
			"so they can attack.";
		Location = Tile.TilePosition(objective.transform.position);
		
		interactable = objective.AddComponent<AlienSecondaryFourInteractable>();
		interactable.InstantInteract = true;
		interactable.StartMe();
	}

	public override void OnComplete() {
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("SpawnBoardingParty", PhotonTargets.All, PickBoardingParty().transform.position);
		Destroy(this);

		string message = "Alien has stolen secret documents from " + 
			interactable.GetComponent<Location>().MyLocation.ToString();
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message);
		
		Destroy(interactable);
	}
}
