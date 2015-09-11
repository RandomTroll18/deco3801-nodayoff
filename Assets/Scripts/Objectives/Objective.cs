using UnityEngine;
using System.Collections;

/**
 * Base class for all objectives.
 * 
 * Primary objectives are just Objectives.
 */
public abstract class Objective : MonoBehaviour {
	protected string title;
	protected string description;
	protected Tile location;
	protected GameManager gameManager;

	protected Objective() {
		gameManager = GameObject.FindGameObjectWithTag("GameController")
			.GetComponent<GameManager>();
	}

	public string Title {
		get {
			return title;
		}
		set {
			title = value;
		}
	}

	public string Description {
		get {
			return description;
		}
		set {
			description = value;
		}
	}

	public Tile Location {
		get {
			return location;
		}
		set {
			location = value;
		}
	}

	public abstract void OnComplete();
}
