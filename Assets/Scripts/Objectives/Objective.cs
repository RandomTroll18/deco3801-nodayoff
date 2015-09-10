using UnityEngine;
using System.Collections;

/**
 * Base class for all objectives.
 * 
 * Primary objectives are just Objectives.
 */
public abstract class Objective {
	string title;
	string description;
	Tile location;

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

	// TODO: OnComplete()
}
