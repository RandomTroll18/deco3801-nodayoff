using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EffectToolTipScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

	public GameObject EffectToolTipPanelObject; // The effect tool tip panel
	public Text EffectToolTipTextObject; // The effect tool tip text
	string attachedEffectText; // The attached effect text

	/**
	 * Set the text for this tool tip
	 * 
	 * Arguments
	 * - string newEffectText - The effect text to attach
	 */
	public void AttachEffectText(string newEffectText) {
		attachedEffectText = newEffectText;
	}

	/**
	 * Make effect tool tip panel visible and set the text
	 * 
	 * Arguments
	 * - string newText - The new text
	 */
	public void MakeEffectToolTipVisible(string newText) {
		EffectToolTipPanelObject.SetActive(true);
		EffectToolTipTextObject.text = newText;
	}

	/**
	 * Make effect tool tip panel invisible
	 */
	public void MakeEffectToolTipInvisible() {
		EffectToolTipPanelObject.SetActive(false);
	}

	/**
	 * Mouse over function
	 * 
	 * Arguments
	 * - PointerEventData eventData - The data of the on pointer enter event
	 */
	public void OnPointerEnter(PointerEventData eventData) {
		Debug.Log("Effect Box Hovered Over");
		Debug.Log("Effect Box Hover Over Event Data: " + eventData.ToString());
		EffectToolTipPanelObject.GetComponent<Transform>().position = eventData.position;
		MakeEffectToolTipVisible(attachedEffectText);
	}

	/**
	 * Mouse exit function
	 * 
	 * Arguments
	 * - PointerEventData eventData - The data of the on pointer exit event
	 */
	public void OnPointerExit(PointerEventData eventData) {
		Debug.Log("Effect Box Exited");
		Debug.Log("Effect Box Exit Event Data: " + eventData.ToString());
		MakeEffectToolTipInvisible();
	}
}
