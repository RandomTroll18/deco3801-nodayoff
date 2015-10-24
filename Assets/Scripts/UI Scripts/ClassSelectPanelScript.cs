using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ClassSelectPanelScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public GameObject ClassTitleText; // The text for the class title
	public GameObject ClassDescriptionText; // The text for the class description
	public string SpecifiedClass; // The specified class
	string classTitle; // The title to be given
	string classDescription; // The description to be given
	/* Images for effects */
	Sprite nonGlow; 
	Sprite glow;

	// Use this for initialization
	void Start () {
		switch (SpecifiedClass) {
		case "Engineer":
			classTitle = "Engineer";
			classDescription = "The Engineer is an expert in all things mechanical" + StringMethodsScript.NEWLINE;
			nonGlow = Resources.Load<Sprite>("UI/Class Select/engpanel");
			glow = Resources.Load<Sprite>("UI/Class Select/engpanelglow");
			break;
		case "Scout":
			classTitle = "Scout";
			classDescription = "The Scout specializes in spotting objects that no one else would see" 
					+ StringMethodsScript.NEWLINE;
			nonGlow = Resources.Load<Sprite>("UI/Class Select/scoutpanel");
			glow = Resources.Load<Sprite>("UI/Class Select/scoutpanelglow");
			break;
		case "Technician":
			classTitle = "Technician";
			classDescription = "The Technician knows her way around computers"
				+ StringMethodsScript.NEWLINE;
			nonGlow = Resources.Load<Sprite>("UI/Class Select/techpanel");
			glow = Resources.Load<Sprite>("UI/Class Select/techpanelglow");
			break;
		case "Marine":
			classTitle = "Marine";
			classDescription = "The Marine does not know when to quit, using his great physical strength to " +
				"overcome his obstacles" 
				+ StringMethodsScript.NEWLINE;
			nonGlow = Resources.Load<Sprite>("UI/Class Select/marinepanel");
			glow = Resources.Load<Sprite>("UI/Class Select/marinepanelglow");
			break;
		default:
			throw new System.NotSupportedException("Invalid Class");
		}
	}
	
	/**
	 * Mouse over function
	 * 
	 * Arguments
	 * - PointerEventData eventData - The data of the on pointer enter event
	 */
	public void OnPointerEnter(PointerEventData eventData) {
		/* Set elements to be active */
		ClassTitleText.SetActive(true);
		ClassDescriptionText.SetActive(true);

		/* Set text and image */
		ClassTitleText.GetComponent<Text>().text = classTitle;
		ClassDescriptionText.GetComponent<Text>().text = classDescription;
		GetComponent<Image>().sprite = glow;
		GetComponent<RectTransform>().sizeDelta = new Vector2(278f, 625.5f);
	}
	
	/**
	 * Mouse exit function
	 * 
	 * Arguments
	 * - PointerEventData eventData - The data of the on pointer exit event
	 */
	public void OnPointerExit(PointerEventData eventData) {
		/* Set elements to be inactive */
		ClassTitleText.SetActive(false);
		ClassDescriptionText.SetActive(false);

		/* Unset text */
		ClassTitleText.GetComponent<Text>().text = "";
		ClassDescriptionText.GetComponent<Text>().text = "";
		GetComponent<Image>().sprite = nonGlow;
		GetComponent<RectTransform>().sizeDelta = new Vector2(211f, 569f);
	}
}
