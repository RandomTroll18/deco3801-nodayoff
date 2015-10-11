using UnityEngine;
using System.Collections;

public class TestSetupScript : MonoBehaviour {
	SpawnPoint[] spawnPoints;
	// Use this for initialization
	void Start() {
		GameObject[] playerCameras = GameObject.FindGameObjectsWithTag("MainCamera");
		spawnPoints = Object.FindObjectsOfType<SpawnPoint>();
		SpawnMyPlayer();
		foreach (GameObject playerCamera in playerCameras) // Make sure player cameras are disabled
			playerCamera.SetActive(false);
	}

	void Update() {
		GameObject[] playerCameras = GameObject.FindGameObjectsWithTag("MainCamera");
		if (GameObject.FindGameObjectsWithTag("Player").Length == PhotonNetwork.playerList.Length) {
			foreach (GameObject playerCamera in playerCameras)
				playerCamera.SetActive(true);
			gameObject.SetActive(false);
		}
	}

	void SpawnMyPlayer() {
		SpawnPoint spawn = spawnPoints[0]; // TODO: pick spawn point based on class
		GameObject myPlayer = PhotonNetwork.Instantiate(
			"Player", 
			spawn.transform.position, 
			spawn.transform.rotation, 
			0
			);
		Player.MyPlayer = myPlayer;
		GameObject gm =  Object.FindObjectOfType<GameManager>().gameObject;
		gm.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, null);
		Object.FindObjectOfType<GameManager>().StartMe();
		
		Classes pClass;
		switch (myPlayer.GetComponent<Player>().GetPlayerClass()) {
		case "Engineer Class":
			pClass = Classes.ENGINEER;
			break;
		case "Marine Class":
			pClass = Classes.MARINE;
			break;
		case "Technician Class":
			pClass = Classes.TECHNICIAN;
			break;
		case "Scout Class":
			pClass = Classes.SCOUT;
			break;
		default:
			pClass = Classes.MARINE;
			Debug.LogWarning("The player class isn't what it's expected to be: " + 
			                 myPlayer.GetComponent<Player>().GetPlayerClass());
			break;
		}
		foreach (SpawnPoint thisPoint in spawnPoints) {
			if (thisPoint.Class == pClass) {
				spawn = thisPoint;
			}
		}
		myPlayer.transform.position = spawn.transform.position;
		
		myPlayer.GetComponent<Player>().GenerateStunGun();
	}
}
