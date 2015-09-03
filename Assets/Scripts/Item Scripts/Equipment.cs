
/**
 * This is an abstract class representing items 	
 * that can be equipped by players. These items 
 * can be one of the following:
 * 
 * - Weapon
 * - Armour
 */
public abstract class Equipment : Item {

	/*
	 * The percentage at which the corresponding is 
	 * affected by. These are in percentages, so 
	 * a value of 1.00 => 100%, 0.90 => 90%, etc.
	 * 
	 * e.g. if statsAffected[0] == Stat.HP and 
	 * percentEffect[0] == 1.1, then 
	 * Stat.HP = Stat.HP * 1.1;
	 * 
	 * The indices of these values correspond to 
	 * the indices of the stats in statsAffected
	 * (like valueEffect)
	 */
	protected double[] PercentEffect;
}
