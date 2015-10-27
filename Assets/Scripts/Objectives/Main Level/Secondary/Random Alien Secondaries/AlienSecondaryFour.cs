using UnityEngine;
using System.Collections;

/*
 * Random boarding party added to map
 */
using System.Collections.Generic;


public class AlienSecondaryFour : SecondaryObjective {
	AlienSecondaryFourInteractable interactable;
	static List<GameObject> usedSpawns = new List<GameObject>();

	GameObject PickBoardingParty() {
		GameObject[] spawns = GameObject.FindGameObjectsWithTag("Boarding Party");
		int spawn = -1;
		for (int i = 0; i < spawns.Length; i++) {
			if (!usedSpawns.Contains(spawns[i])) {
			    spawn = i;
				usedSpawns.Add(spawns[i]);
			}
		}
		if (spawn == -1)
			return null;
		Debug.Log("boarding spawn :" + spawn);
		return spawns[spawn];
	}

	// Use this for initialization
	public void StartMe() {
		Log();
		GameObject objective = PickAlienObjective();
		Title = "Assist Allies";
		Description = "A Versipellis boarding party is at the ready. Shutdown the ship's defenses" +
			" so they can attack.";
		Location = Tile.TilePosition(objective.transform.position);
		
		interactable = objective.AddComponent<AlienSecondaryFourInteractable>();
		interactable.InstantInteract = true;
		interactable.StartMe();
	}

	public override void OnComplete() {
		GameObject spawn = PickBoardingParty();
		if (spawn == null)
			Destroy(this);
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("SpawnBoardingParty", PhotonTargets.All, spawn.transform.position);
		Destroy(this);

		string message = "Alien has shutdown the ship's defenses in " + 
			interactable.GetComponent<Location>().MyLocation.ToString() + ". A Versipellis boarding" +
				" party is lurking somewhere. Find and destroy them before they destroy the ship.\n" +
				"Whilst the boarding part is alive, an extra round is lost per turn.";
		string title = "Alien Activity";
		string image = "ui/events/alienboard";
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message, title, image);
		
		Destroy(interactable);
	}
}
