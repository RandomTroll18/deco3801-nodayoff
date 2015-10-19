using UnityEngine;
using System.Collections;

/*
 * Getting to one escape pod. There are 4 escape pods and each can only hold one player. So if an
 * escape pod is lost, one human must lose.
 */
public class FourthObjectiveMain : PrimaryObjective {

	// Use this for initialization
	public void StartMe() {
		Title = "Escape";
		Description = "The escape pods are ready. Get to them before our ship is destroyed." 
			+ StringMethodsScript.NEWLINE +
			"A Technician is needed.";
	}

	public override void OnComplete() {
		Application.LoadLevel("Win Screen");
	}

}
