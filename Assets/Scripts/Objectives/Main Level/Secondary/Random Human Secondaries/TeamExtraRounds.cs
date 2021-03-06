﻿using UnityEngine;
using System.Collections;

/* 
 * Team objective.
 * When enough terminals have been activated, a bunch of rounds are added.
 */
public class TeamExtraRounds : SecondaryObjective {
	int step = 0;
	const int EXTRA_ROUNDS = 10;

	// Use this for initialization
	void Start () {
		Log();
		
		Title = "Repair Core";
		Description = "TEAM OBJECTIVE\n" +
			"There are several terminals around the energy core we can use to repair it." +
			"Repairing the energy core will give us more time to escape (" + EXTRA_ROUNDS + " rounds). " +
			"This is a difficult task, but we can complete it as a group.";
		Location = Tile.TilePosition(GameObject.Find("Engine Core").transform.position);

	}

	public void Progress() {
		step++;

		if (step == 4) {
			/*
			 * Enough terminals have been activated
			 */

			OnComplete();
		}
	}
	
	public override void OnComplete() {
		Destroy(this);

		string message = "Repair core has been repaired by survivors.";
		string title = "Repair Core";
		string image = "ui/events/consoleimg";
		Object.FindObjectOfType<GameManager>()
			.GetComponent<PhotonView>().RPC("EventCardMessage", PhotonTargets.All, message, title, image);
		
		Object.FindObjectOfType<GameManager>().IncreaseRounds(EXTRA_ROUNDS);
	}
}
