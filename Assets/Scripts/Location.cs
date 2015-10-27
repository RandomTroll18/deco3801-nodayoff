using UnityEngine;
using System.Collections;

public enum Locations {
	BRDIGE,
	CARGO_BAY,
	L_GUN,
	R_GUN,
	L_WING,
	R_WING,
	POWER,
	QUARTERS
}


public class Location : MonoBehaviour {

	public Locations MyLocation;

	public override string ToString() {
		switch (MyLocation) {
		case Locations.BRDIGE:
			return "Bridge"; 
		case Locations.L_GUN:
			return "Left Gun";
		case Locations.R_GUN:
			return "Right Gun";
		case Locations.L_WING:
			return "Left Wing";
		case Locations.R_WING:
			return "Right Wing";
		case Locations.POWER:
			return "Power";
		case Locations.QUARTERS:
			return "Quarters";
		case Locations.CARGO_BAY:
			return "Cargo Bay";
		default:
			return "Error";
		}
	}

}
