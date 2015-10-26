using UnityEngine;
using System.Collections;

public class SecurityRoomObjective : SecondaryObjective {

	void Start() {
		ObjectiveName = "TechnicianSecondaryOne";
		Title = "Security Room";
		Description = "TEAM OBJECTIVE\n" +
			"Once you know which player is the alien, use the security room to have the security system kill them." +
			" This requires a vote of at least 3 against a single player to activate" +
			". We only have enough power to use the system once, make it count. Activating this on a human" +
			" will mean we lose that human and can no longer use the system against the alien.";
		Location = Tile.TilePosition(Object.FindObjectOfType<SecurityRoomConsole>().transform.position);
	}
	
	public override void OnComplete() {
		Destroy(this);
		base.OnComplete();
	}
}
