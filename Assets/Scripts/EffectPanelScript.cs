using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectPanelScript : MonoBehaviour {

	public GameObject Panel; // The effect box panel
	public GameObject BoxPrefab; // The prefab for a box
	List<GameObject> boxes; // List of boxes
	RectTransform panelTransform;  // The transform component of the panel
	float boxWidth; // The width of the box
	float boxHeight; // The height of the box

	/**
	 * Start function. Things to do
	 * - Initialize list of effects
	 * - Get the RectTransform component of the panel
	 * - Set the box's height and width (relative measurement)
	 */
	void Start() {
		boxes = new List<GameObject>();
		panelTransform = Panel.GetComponent<RectTransform>();
		boxWidth = (float)-47.8333;
		boxHeight = (float)-45.8333;
	}

	/**
	 * Update the UI when list of turn effects is updated
	 */
	void updateUI() {
		int rowCount = 1; // The number of boxes in the row
		int heightCount = 0; // The number of rows
		foreach (GameObject box in boxes) {
			// Set the box position
			box.GetComponent<RectTransform>().anchoredPosition3D = 
				new Vector3((((boxWidth - 25) * rowCount)), (boxHeight * heightCount));
			box.GetComponent<Image>().sprite = box.GetComponent<EffectBoxScript>().GetEffect().GetIcon();
			rowCount++;
			if (rowCount == 13) { // New row
				rowCount = 0;
				heightCount++;
			}
		}
	}

	/**
	 * Add a single turn effect to this effect panel and update the UI
	 * 
	 * Arguments
	 * - TurnEffect newEffect - The effect to add
	 */
	public void AddTurnEffect(TurnEffect newEffect) {
		GameObject box; // Box to instantiate
		EffectBoxScript boxScript; // The script attached to the box
		box = Instantiate<GameObject>(BoxPrefab); // Instantiate UI element
		box.GetComponent<RectTransform>().SetParent(panelTransform); // Set the parent
		box.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
		box.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
		box.GetComponent<Image>().sprite = newEffect.GetIcon(); // Set the icon
		boxes.Add(box); // Add this game object to the list of boxs
		boxScript = box.GetComponent<EffectBoxScript>(); // Get Box Script
		boxScript.AddEffect(newEffect); // Add the turn effect to the box
		updateUI();
	}

	/**
	 * Add the turn effects to this effect panel and 
	 * update the UI
	 * 
	 * Arguments
	 * - List<TurnEffect> newEffects - New effects to add
	 */
	public void AddTurnEffects(List<TurnEffect> newEffects) {
		GameObject box; // Box to instantiate
		EffectBoxScript boxScript; // The script attached to the box
		foreach (TurnEffect effect in newEffects) AddTurnEffect(effect);
	}

	/**
	 * Remove a turn effect from this effect panel and update the UI
	 * 
	 * Arguments
	 * - TurnEffect toRemove - The effect to be removed
	 */
	public void RemoveTurnEffect(TurnEffect toRemove) {
		EffectBoxScript boxScript; // The script attached to a box
		foreach (GameObject box in boxes) {
			boxScript = box.GetComponent<EffectBoxScript>();
			if (boxScript.GetEffect().Equals(toRemove)) {
				boxes.Remove(box); // Remove this game object from list of boxs
				Destroy(box); // Destroy this from existence
				break;
			}
		}
		updateUI();
	}
}
