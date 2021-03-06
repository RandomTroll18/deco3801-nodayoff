﻿using UnityEngine;
using System.Collections;

/*
 * Players need to activate console in the power room.
 */
public class FirstObjectiveMain : PrimaryObjective {

	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}

	void Start() {
		Title = "Activate Power";
		Description = "Main power sources have taken heavy damage." + StringMethodsScript.NEWLINE +
			"Get to the power room and activate our emergency power reserves. Our Technician can do" +
			" this quicker than everyone else.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 1")
				Location = Tile.TilePosition(objective.transform.position);
		}

		NextObjective = Object.FindObjectOfType<SecondObjectiveMain>();
	}

	public override void OnComplete() {
		SecondObjectiveMain obj = gameObject.AddComponent<SecondObjectiveMain>();
		obj.StartMe();
		NextObjective = obj;
		base.OnComplete();
	}
}
