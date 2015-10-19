using UnityEngine;
using System.Collections;

/*
 * The last thing before escape. As far from the escape pods as possible
 */
public class ThirdObjectiveMain : PrimaryObjective {

	// Use this for initialization
	public void StartMe() {
		Title = "Direct Power";
		Description = "All power needs to be directed to the escape pods. Get to the bridge and " +
			"use the console." + StringMethodsScript.NEWLINE +
			"A Technician is needed.";
	}

	public override void OnComplete() {
		FourthObjectiveMain obj = gameObject.AddComponent<FourthObjectiveMain>();
		obj.StartMe();
		NextObjective = obj;
		base.OnComplete();
	}
}
