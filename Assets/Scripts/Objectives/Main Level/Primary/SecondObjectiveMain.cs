using UnityEngine;
using System.Collections;

/*
 * A randomly positioned keycard needed to access the bridge (objective 3)
 */
public class SecondObjectiveMain : PrimaryObjective {
	GameObject spawnPoints;

	public void StartMe() {
		Title = "Find the keycard";
		Description = "Somewhere on the SS DECO is the keycard. This keycard is needed to activate" +
			" the escape pods from the bridge. The exact location of the keycard is unknown, but we" +
			" do know it's either in the Quarters, Left Wing, Right Wing or the Left Gun.";
		spawnPoints = GameObject.Find("Key Card Spawns");

		SpawnKeyCard();
	}

	void SpawnKeyCard() {
		// instantiate key card and interactable
		if (PhotonNetwork.isMasterClient) {
			// pick a random spawn point
			int spawnNum = Random.Range(0, spawnPoints.transform.childCount - 1);
			GameObject spawnPoint = spawnPoints.transform.GetChild(spawnNum).gameObject;

			Vector3 spawnPos = Tile.TileMiddle(Tile.TilePosition(spawnPoint.transform.position));

			GameObject key = PhotonNetwork.Instantiate(
				"Main Level Interactables/Key Card", 
				spawnPos, 
				Quaternion.identity, 
				0);
		}

	}

	public override void OnComplete() {
		ThirdObjectiveMain obj = gameObject.AddComponent<ThirdObjectiveMain>();
		obj.StartMe();
		NextObjective = obj;
		base.OnComplete();
	}
}
