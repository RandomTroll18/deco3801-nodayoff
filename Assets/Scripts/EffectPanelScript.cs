using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectPanelScript : MonoBehaviour {

	public GameObject Panel; // The effect card panel
	public GameObject CardPrefab; // The prefab for a card
	List<GameObject> cards; // List of cards
	RectTransform panelTransform;  // The transform component of the panel
	float cardWidth; // The width of the card
	float cardHeight; // The height of the card

	/**
	 * Start function. Things to do
	 * - Initialize list of effects
	 * - Get the RectTransform component of the panel
	 * - Set the card's height and width (relative measurement)
	 */
	void Start() {
		cards = new List<GameObject>();
		panelTransform = Panel.GetComponent<RectTransform>();
		cardWidth = (float)47.8333;
		cardHeight = (float)45.8333;
	}

	/**
	 * Update the UI when list of turn effects is updated
	 */
	void updateUI() {
		int rowCount = 0; // The number of cards in the row
		int heightCount = 0; // The number of rows
		foreach (GameObject card in cards) {
			// Set the card position
			card.GetComponent<RectTransform>().anchoredPosition3D = 
				new Vector3((cardWidth * rowCount), (cardHeight * heightCount));
			card.GetComponent<Image>().sprite = card.GetComponent<EffectCardScript>().GetEffect().GetIcon();
			rowCount++;
			if (rowCount == 12) { // New row
				rowCount = 0;
				heightCount++;
			}
		}
	}

	/**
	 * Add the turn effects to this effect panel and 
	 * update the UI
	 * 
	 * Arguments
	 * - List<TurnEffect> newEffects - New effects to add
	 */
	public void AddTurnEffects(List<TurnEffect> newEffects) {
		GameObject card; // Card to instantiate
		EffectCardScript cardScript; // The script attached to the card
		foreach (TurnEffect effect in newEffects) {
			card = Instantiate<GameObject>(CardPrefab); // Instantiate UI element
			card.GetComponent<RectTransform>().SetParent(panelTransform); // Set the parent
			card.GetComponent<Image>().sprite = effect.GetIcon(); // Set the icon
			cards.Add(card); // Add this game object to the list of cards
			cardScript = card.GetComponent<EffectCardScript>(); // Get Card Script
			cardScript.AddEffect(effect); // Add the turn effect to the card
		}
		updateUI();
	}

	/**
	 * Remove a turn effect from this effect panel and update the UI
	 * 
	 * Arguments
	 * - TurnEffect toRemove - The effect to be removed
	 */
	public void RemoveTurnEffect(TurnEffect toRemove) {
		EffectCardScript cardScript; // The script attached to a card
		foreach (GameObject card in cards) {
			cardScript = card.GetComponent<EffectCardScript>();
			if (cardScript.GetEffect().Equals(toRemove)) {
				cards.Remove(card); // Remove this game object from list of cards
				Destroy(card); // Destroy this from existence
				break;
			}
		}
		updateUI();
	}
}
