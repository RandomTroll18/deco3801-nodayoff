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
			break;
		case Locations.L_GUN:
			return "Left Gun";
			break;
		case Locations.R_GUN:
			return "Right Gun";
			break;
		case Locations.L_WING:
			return "Left Wing";
			break;
		case Locations.R_WING:
			return "Right Wing";
			break;
		case Locations.POWER:
			return "Power";
			break;
		case Locations.QUARTERS:
			return "Quarters";
			break;
		case Locations.CARGO_BAY:
			return "Cargo Bay";
			break;
		default:
			return "Error";
			break;
		}
	}

}
