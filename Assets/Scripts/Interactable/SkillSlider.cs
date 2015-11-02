using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillSlider : MonoBehaviour {

	public Text SliderText; // The text for the slider
	public Slider APSlider; // The AP slider

	SkillCheck skillCheck;

	/**
	 * Startup this object
	 */
	void Start() {
		skillCheck = Object.FindObjectOfType<SkillCheck>();
	}

	/**
	 * Update the text of the slider
	 */
	public void TextUpdate(){
		GameObject player = Player.MyPlayer; // The controlling player
		Player playerScript = player.GetComponent<Player>(); // The player's script
		double multiplier; // The multiplier to apply to the AP
		int actualAP, multipliedAP; // The AP before and after being multiplied, respectively
		float percentage; // The percentage to display

		if (skillCheck.ClassMultiplier != Stat.NOMULTIPLIER) // Multiplier exists
			multiplier = playerScript.GetPlayerClassObject().GetStat(skillCheck.ClassMultiplier);
		else // Multiplier does not exist
			multiplier = 1;

		/* Calculate AP and percentage */
		actualAP = (int)Mathf.Floor((float)(playerScript.GetStatValue(Stat.AP) * (float)APSlider.value / 100f));
		multipliedAP = (int)Mathf.Floor((float)actualAP * (float)multiplier);
		percentage = ((float)multipliedAP - skillCheck.Cost) / multipliedAP * 100;

		SliderText.text = multipliedAP.ToString() + " (" + percentage.ToString("F2") + "% chance)"; // Update text
	}
}