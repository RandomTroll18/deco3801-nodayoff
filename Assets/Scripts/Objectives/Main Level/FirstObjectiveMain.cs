using UnityEngine;
using System.Collections;

public class FirstObjectiveMain : PrimaryObjective {

	public override void InitializeObjective()
	{
		base.InitializeObjective();
		Start();
	}

	void Start() {
		Title = "Activate Auxillary Power";
		Description = "Main power sources have taken heavy damage." + StringMethodsScript.NEWLINE +
			"A Technician is needed.";

		foreach (GameObject objective in GameObject.FindGameObjectsWithTag("Objective")) {
			if (objective.name == "Objective 1")
				Location = Tile.TilePosition(objective.transform.position);
		}

		NextObjective = Object.FindObjectOfType<SecondObjectiveMain>();
//		Door = Tile.TilePosition(-10f, -4.3f);
	}

	public override void OnComplete() {
		SecondObjectiveMain obj = gameObject.AddComponent<SecondObjectiveMain>();
		obj.StartMe();
		NextObjective = obj;
		base.OnComplete();
	}
}
