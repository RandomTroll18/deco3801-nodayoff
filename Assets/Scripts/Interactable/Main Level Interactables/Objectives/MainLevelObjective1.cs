﻿using UnityEngine;

public class MainLevelObjective1 : InteractiveObject {
	public GameObject[] DoorsToUnlock; // The doors to unlock

	public override void TakeAction(int input){
		
		if (IsInactivated) {
			Debug.Log("Inactive");
			return;
		}
		
		if (!PrimaryO.GetObjective().Title.Equals(Object.FindObjectOfType<FirstObjectiveMain>().Title)) {
			Debug.Log("Wrong part of the story");
			return;
		}
		
		if (SpendAP(input, MinCost)) {
			InteractablSync();
			IsInactivated = true;
			PrimaryO.OnComplete();
			Debug.Log("Opened");
			CloseEvent();		
		} else {
			Debug.Log("Failed with " + input);
			CloseEvent();	
		}
		
	}
	
	public void InteractablSync() {
		GetComponent<PhotonView>().RPC("Sync", PhotonTargets.All, null);
	}
	
	[PunRPC]
	void Sync(){
		GiveSecondaryObjectives();

		IsInactivated = true;

	}

	/**
	 * Give alien secondary objectives of their human class
	 * 
	 * Arguments
	 * - AlienClass alienClass - The alien class
	 * - GameObject secondaries - The container for the player's secondary objectives
	 */
	void giveAlienSecondaryHumanObj(AlienClass alienClass, GameObject secondaries) {
		switch (alienClass.GetHumanClassType()) {
		case Classes.SCOUT: // Scout
			Debug.Log("Adding scout secondary");
			secondaries.AddComponent<ScoutSecondaryOne>();
			break;
		case Classes.ENGINEER: // Engineer
			Debug.Log("Adding engineer secondary");
			secondaries.AddComponent<EngineerSecondaryOne>();
			break;
		case Classes.MARINE: // Marine
			Debug.Log("Adding marine secondary");
			secondaries.AddComponent<MarineSecondaryOne>();
			break;
		case Classes.TECHNICIAN: // Technician
			Debug.Log("Adding Technician secondary");
			secondaries.AddComponent<TechnicianSecondaryOne>();
			break;
		}
	}
	
	/**
	 * Give each player their secondary objectives
	 */
	void GiveSecondaryObjectives() {
		Player p = Player.MyPlayer.GetComponent<Player>();
		GameObject secondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;

		/*
		 * Each class gets their own, special secondary after completing the first objective.
		 */
		switch (p.GetPlayerClassObject().GetClassTypeEnum()) {
		case Classes.SCOUT: // Scout
			Debug.Log("Adding scout secondary");
			secondaries.AddComponent<ScoutSecondaryOne>();
			break;
		case Classes.ENGINEER: // Engineer
			Debug.Log("Adding engineer secondary");
			secondaries.AddComponent<EngineerSecondaryOne>();
			break;
		case Classes.MARINE: // Marine
			Debug.Log("Adding marine secondary");
			secondaries.AddComponent<MarineSecondaryOne>();
			break;
		case Classes.TECHNICIAN: // Technician
			Debug.Log("Adding Technician secondary");
			secondaries.AddComponent<TechnicianSecondaryOne>();
			break;
		case Classes.BETRAYER: // Alien
			Debug.Log("Adding alien secondary");
			secondaries.AddComponent<AlienSecondaryTwo>().StartMe();
			// Need to add human class secondaries as well
			giveAlienSecondaryHumanObj((AlienClass)p.GetPlayerClassObject(), secondaries);
			break;
		default: // Unknown
			throw new System.NotSupportedException("Unknown secondary class objects");
		}
		secondaries.AddComponent<TeamExtraRounds>(); // Nasty event for players to resolve
	}
}
