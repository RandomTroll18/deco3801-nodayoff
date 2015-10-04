using UnityEngine;
using System.Collections.Generic;

public class AlienClass : BaseClass {

	PlayerClass humanClass; // The contained human class

	/**
	 * Constructor for Alien Class
	 * 
	 * Arguments
	 * - Player player - The player that owns this class
	 */
	public AlienClass(Player player) : base(){
		DefaultStats[Stat.ENGMULTIPLIER] = 1.5;
		DefaultStats[Stat.MARINEMULTIPLIER] = 1.5;
		DefaultStats[Stat.SCOUTMULTIPLIER] = 1.5;
		DefaultStats[Stat.TECHMULTIPLIER] = 1.5;

		PrimaryAbility = new AlienPrimaryAbility(player);
		ClassTypeEnum = Classes.BETRAYER;
	}

	/**
	 * Get the attached human class
	 * 
	 * Returns
	 * - The human class of this alien
	 */
	public PlayerClass GetHumanClass() {
		return humanClass;
	}

	/**
	 * Set the human class of the alien
	 * 
	 * Arguments
	 * - Classes humanClassType - The human class to assign
	 */
	public void SetHumanClass(Classes humanClassType) {
		Player playerScript = Player.MyPlayer.GetComponent<Player>();
		switch (humanClassType) {
		case Classes.ENGINEER:
			humanClass = new EngineerClass(playerScript);
			break;
		case Classes.MARINE:
			humanClass = new MarineClass(playerScript);
			break;
		case Classes.SCOUT:
			humanClass = new ScoutClass(playerScript);
			break;
		case Classes.TECHNICIAN:
			humanClass = new TechnicianClass(playerScript);
			break;
		default:
			throw new System.NotSupportedException("Need to assign a valid human class to alien");
		}
	}

	/**
	 * Return the name of this class in human readable format
	 * 
	 * Returns
	 * - The string form of this type of player class
	 */
	public override string GetPlayerClassType() {
		return "Alien Class";
	}
}
