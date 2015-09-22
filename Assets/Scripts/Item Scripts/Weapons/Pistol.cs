
/**
 * Class for an ordinary pistol (may be changed if 
 * variety needed for pistols
 */
public class Pistol : ShortRangeWeapon {

	/**
	 * Things to do on start
	 * - Set item description
	 * - Set damage
	 * - Set range
	 * - Set number of rounds in pistol 
	 * - Set Cool Down values
	 * before it can no longer be used
	 */
	void Start() {
		ItemDescription = "Your standard issue pistol. Seven 9mm bullets effective " +
			"against humans, but we can't say that it's tested on aliens";

		InstantEffects = null; // No stats being affected

		Damage = 10.0; // Let's say we do 10.0 damage per successful hit
		Range = 4.0; // Let's say range is 4.0 - Not confirmed
		Rounds = 7.0; // Only 7 rounds
		CoolDown = 0; // No Cool down initially
		CoolDownSetting = 1; // This item can only be used once per turn
	}

	/* Override abstract functions so that compiler doesn't whine */
	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(Tile targetTile)
	{
		throw new System.NotImplementedException();
	}

	/**
	 * Override toString function
	 * 
	 * Returns
	 * - A string providing info on this item
	 */
	public override string ToString() {
		// The string to return. Start with the name
		string toReturn = "Item Name: " + ItemName + StringMethodsScript.NEWLINE;
		
		// Next, add item description
		toReturn += "Description: " + ItemDescription + StringMethodsScript.NEWLINE;
		
		// Next, add the amount of damage this weapon does
		toReturn += "Damage: " + Damage + StringMethodsScript.NEWLINE;

		// Next, add the range of this weapon
		toReturn += "Range: " + Range + StringMethodsScript.NEWLINE;

		// Next, add the number of rounds left and return the final string
		toReturn += "Rounds Left: " + Rounds + StringMethodsScript.NEWLINE;

		// Next, add the number of turns this item can be used at a time
		toReturn += "Cool Down Turns: " + CoolDownSetting + StringMethodsScript.NEWLINE;

		return toReturn;
	}
}
