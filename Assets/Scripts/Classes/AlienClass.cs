using UnityEngine;
using System.Collections.Generic;

public class AlienClass : BaseClass {

	PlayerClass humanClass; // The contained human class
	Classes humanClassType; // The human class type

	/**
	 * Constructor for Alien Class
	 * 
	 * Arguments
	 * - Player player - The player that owns this class
	 * - Classes humanClassTypeToSet - The human class to set
	 */
	public AlienClass(Player player, Classes humanClassTypeToSet) : base() {

		/*
		 * The alien needs to be proficient in every area, but not as proficient 
		 * as the human classes in their areas
		 */
		Stats[Stat.ENGMULTIPLIER] = DefaultStats[Stat.ENGMULTIPLIER] = 1.5;
		Stats[Stat.MARINEMULTIPLIER] = DefaultStats[Stat.MARINEMULTIPLIER] = 1.5;
		Stats[Stat.SCOUTMULTIPLIER] = DefaultStats[Stat.SCOUTMULTIPLIER] = 1.5;
		Stats[Stat.TECHMULTIPLIER] = DefaultStats[Stat.TECHMULTIPLIER] = 1.5;

		PrimaryAbility = new AlienPrimaryAbility(player);
		ClassTypeEnum = Classes.BETRAYER;
		humanClassType = humanClassTypeToSet;
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
	 */
	public void SetHumanClass() {
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
	 * Get human class type of the alien
	 * 
	 * Returns
	 * - The human class type of this alien
	 */
	public Classes GetHumanClassType() {
		return humanClassType;
	}

	/**
	 * Set the human class of the alien
	 * 
	 * Arguments
	 * - Classes newHumanClassType - The human class to assign
	 */
	public void SetHumanClass(Classes newHumanClassType) {
		humanClassType = newHumanClassType;
		SetHumanClass();
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
