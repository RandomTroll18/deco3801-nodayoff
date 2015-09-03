
public class TurnEffect {

	Stat statAffected; // The stat affected
	string description;
	/*
	 * The value that the stat is affected by.
	 * The value can be negative
	 */
	double value;

	/**
	 * The constructor
	 * 
	 * Arguments
	 * - Stat statAffected - the stat to be affected
	 * - double value - The value that the stat will be affected by
	 */
	public TurnEffect(Stat newStatAffected, double newValue) {
		statAffected = newStatAffected;
		value = newValue;
	}

	/**
	 * Get the stat affected by this turn effect
	 * 
	 * Returns
	 * - The stat this effect is affecting
	 */
	public int GetStatAffected() {
		return (int)statAffected;
	}

	/**
	 * Get the value by which the stat is to be affected
	 * 
	 * Returns
	 * - The value that the stat will be affected by
	 */
	public double GetValue() {	
		return value;
	}

	/**
	 * Sets the stat affected
	 * 
	 * Arguments
	 * - Stat newStat - The stat being affected
	 */
	public void SetStatAffected(Stat newStat) {
		statAffected = newStat;
	}

	/**
	 * Sets the value by which the stat is affected by
	 * 
	 * Arguments
	 * - double newValue - the value by which the stat is affected
	 */
	public void SetValue(double newValue) {
		value = newValue;
	}

	/**
	 * Tostring function
	 * 
	 * Return
	 * - The string form of this function
	 */
	public override string ToString() {
		string finalString = "Turn Effect: " + StringMethodsScript.NEWLINE;
		finalString += "Stat: " + EnumsToString.ConvertStatEnum(statAffected) + StringMethodsScript.NEWLINE;
		finalString += "Value: " + value;
		return finalString;
	}
}
