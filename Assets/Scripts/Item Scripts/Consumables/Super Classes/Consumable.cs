
/**
 * This is the class inherited by items that can 
 * be consumed (but not equipped)
 */
public abstract class Consumable : Item {

	/*
	 * The amount of this consumable. Needs to be public 
	 * because some objects that are designated to be 
	 * consumables may have more than 1
	 */
	public int Amount;

	/**
	 * Update the text of the context aware box
	 */
	protected void UpdateContextAwareBox() {
		if (Amount > 0) // Update data
			FindObjectOfType<ContextAwareBox>().UpdateAttachedObjectText();
	}
}
