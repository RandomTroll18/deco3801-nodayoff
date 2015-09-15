
/**
 * This is the weapon class inherited by equipment that 
 * are to be weapons
 */
public abstract class Weapon : Equipment {

	protected double Damage; // The amount of damage done by this weapon
	/*
	 * Number of rounds per use. Needs to be double because you can have something 
	 * like a flamethrower. If this is equal to -1, then it's infinite
	 */
	protected double Rounds; // The number of rounds per use. Needs to be done
}
