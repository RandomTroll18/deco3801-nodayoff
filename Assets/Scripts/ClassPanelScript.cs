using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Script for the class panel
 */
public class ClassPanelScript : MonoBehaviour {

	public Text ClassTitle; // The Text Box for the Class Title
	public Text PrimaryAbilityText; // The text for the primary ability button
	public GameObject ContextAwareBox; // The context aware box
	public GameObject PrimaryAbilityButton; // The button for activating the primary ability

	ContextAwareBoxScript contextAwareBoxScript; // The context aware box script
	ActivationTileController activationTileController; // Controller for generating activation tiles

	/**
	 * Start function
	 */
	void Start() {
		contextAwareBoxScript = ContextAwareBox.GetComponent<ContextAwareBoxScript>();
		activationTileController = ContextAwareBox.GetComponent<ActivationTileController>();
	}

	/**
	 * Activate primary ability
	 * 
	 * Arguments
	 * - GameObject player - The playerwhose ability is being activated
	 */
	public void ActivatePrimaryAbility(GameObject player) {
		Player playerScript = player.GetComponent<Player>(); // Player script
		Ability primaryAbility = playerScript.GetPlayerClassObject().GetPrimaryAbility(); // Primary ability

		if (!primaryAbility.AbilityIsActive()) { // Only activate if ability hasn't been activated before
			Debug.Log("Class Panel: ability is not active. Activate it");
			Debug.Log("Primary ability: " + primaryAbility);
			activationTileController.GeneratorInterface(playerScript, primaryAbility);
		}
	}

	/**
	 * Initialize the class panel
	 * 
	 * Arguments
	 * - string className - The class name
	 * - string primaryAbilityName-  The class' primary ability
	 */
	public void InitializeClassPanel(string className, string primaryAbilityName) {
		ClassTitle.text = className;
		PrimaryAbilityText.text = primaryAbilityName;
	}
}
