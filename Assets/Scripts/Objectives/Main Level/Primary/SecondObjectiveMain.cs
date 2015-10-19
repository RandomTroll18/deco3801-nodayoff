using UnityEngine;
using System.Collections;

public class SecondObjectiveMain : PrimaryObjective {
	GameObject spawnPoints;

	public void StartMe() {
		Title = "Find the key card";
		Description = "Somewhere on the SS DECO is the key card";
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
		Application.LoadLevel("WinScreen");
	}
}
