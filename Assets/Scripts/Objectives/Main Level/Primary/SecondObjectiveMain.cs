﻿using UnityEngine;
using System.Collections;

/*
 * A randomly positioned keycard needed to access the bridge (objective 3)
 */
public class SecondObjectiveMain : PrimaryObjective {
	GameObject spawnPoints;

	public void StartMe() {
		Title = "Find the keycard";
		Description = "Somewhere on the SS DECO is the keycard";
		spawnPoints = GameObject.Find("Key Card Spawns");

		SpawnKeyCard();
	}

	void SpawnKeyCard() {
		// pick a random spawn point
		int spawnNum = Random.Range(0, spawnPoints.transform.childCount);
		GameObject spawnPoint = spawnPoints.transform.GetChild(spawnNum).gameObject;

		// instantiate key card and interactable
		Vector3 spawnPos = Tile.TileMiddle(Tile.TilePosition(spawnPoint.transform.position));
		GameObject key = PhotonNetwork.Instantiate(
			"Main Level Interactables/Key Card", 
			spawnPos, 
			Quaternion.identity, 
			0);
		key.GetComponent<MainLevelObjective2>().StartMe();

	}

	public override void OnComplete() {
		ThirdObjectiveMain obj = gameObject.AddComponent<ThirdObjectiveMain>();
		obj.StartMe();
		NextObjective = obj;
		base.OnComplete();
	}
}