using UnityEngine;
using System.Collections;

/**
 * Likely won't be used for the MVP but I'm keeping secondary objectives in mind.
 */
using System.Collections.Generic;


public class SecondaryObjective : Objective {
	public string ObjectiveName; // Used to ID objectives
	protected bool teamObjective;
	static List<GameObject> takenObjectives = new List<GameObject>();

	const int NUM_HUMAN_OBJECTIVES = 1;
	const int NUM_ALIEN_OBJECTIVES = 4;

	protected void Log() {
		ChatTest.Instance.AllChat(true, "NEW SECONDARY");
		Debug.Log("a");
	}

	public override void OnComplete() {
		if (teamObjective) {
			Debug.Log("Team objective completed");
		}
	}

	public static void PickNewHumanObjective() {
		/*
		 * Randomly pick the next secondary for this player
		 */
		int nextObjective = Random.Range(0, NUM_HUMAN_OBJECTIVES - 1);
		GameObject secondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;
		switch (nextObjective) {
		case 0:
			if (secondaries.GetComponentInChildren<SaveRounds>() != null)
				break;

			secondaries.AddComponent<SaveRounds>();
			break;
		}
	}

	public static void PickNewAlienObjective() {
		/*
		 * Randomly pick the next secondary for this player
		 */
		int nextObjective = Random.Range(0, NUM_ALIEN_OBJECTIVES + 2);
		GameObject secondaries = Player.MyPlayer.transform.FindChild("SecondaryObjectives").gameObject;
		switch (nextObjective) {
		case 0:
			if (secondaries.GetComponentInChildren<AlienSecondaryThree>() != null)
				break;

			secondaries.AddComponent<AlienSecondaryThree>().StartMe();
			break;
		case 1:
			if (secondaries.GetComponentInChildren<AlienSecondaryOne>() != null)
				break;

			secondaries.AddComponent<AlienSecondaryOne>().StartMe();
			break;
		case 2:
			if (secondaries.GetComponentInChildren<AlienSecondarySeven>() != null || AlienSecondarySeven.completed) {
				return;
			}

			secondaries.AddComponent<AlienSecondarySeven>();
			break;
		case 3:
			if (secondaries.GetComponentInChildren<AlienSecondaryFour>() != null) {
				return;
			}
			
			secondaries.AddComponent<AlienSecondaryFour>().StartMe();
			break;
		case 4:
			if (secondaries.GetComponentInChildren<AlienSecondaryFour>() != null) {
				return;
			}
			
			secondaries.AddComponent<AlienSecondaryFour>().StartMe();
			break;
		case 5:
			if (secondaries.GetComponentInChildren<AlienSecondaryFour>() != null) {
				return;
			}
			
			secondaries.AddComponent<AlienSecondaryFour>().StartMe();
			break;
		}
	}

	protected GameObject PickHumanObjective() {
		GameObject[] objects = GameObject.FindGameObjectsWithTag("HumanRandomSecondary");
		int spawn = Random.Range(0, objects.Length - 1);
		return objects[spawn];
	}

	protected GameObject PickAlienObjective() {
		GameObject[] objects = GameObject.FindGameObjectsWithTag("AlienRandomSecondary");
		int spawn;
		while (true) {
			spawn = Random.Range(0, objects.Length - 1);
			if (!takenObjectives.Contains(objects[spawn]))
			    break;
		}
		takenObjectives.Add(objects[spawn]);
		return objects[spawn];
	}

}
