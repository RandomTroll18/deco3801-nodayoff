using UnityEngine;
using System.Collections;

/**
 * Base class for all objectives.
 */
public abstract class Objective : MonoBehaviour {
	protected string title;
	protected string description;
	protected Tile location;
	protected Tile location2 = null;
	protected GameManager gameManager;

	public virtual void InitializeObjective() {
		Start();
	}

	void Start() {
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

	public Tile Location2 {
		get {
			return location2;
		}
		set {
			location2 = value;
		}
	}

	[PunRPC]
	public abstract void OnComplete();
}
