using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillSlider : MonoBehaviour {

	public Text SliderText;
	public Slider APSlider;

	SkillCheck skillCheck;

	void Start() {
		skillCheck = Object.FindObjectOfType<SkillCheck>();
	}

	public void TextUpdate (){

		GameObject player = Player.MyPlayer;
		Player playerScript = player.GetComponent<Player>();

		double multiplier;
		if (skillCheck.ClassMultiplier != Stat.NOMULTIPLIER) {
			multiplier = playerScript.GetPlayerClassObject().GetStat(skillCheck.ClassMultiplier);
		} else {
			multiplier = 1;
		}

		int actualAP = (int) Mathf.Floor((float)(playerScript.GetStatValue(Stat.AP) * (float)APSlider.value / 100f));
		int multipliedAP = (int) Mathf.Floor((float) actualAP * (float) multiplier);

		float percentage = ((float) multipliedAP - skillCheck.Cost) / multipliedAP * 100;

		SliderText.text = multipliedAP.ToString() + " (" + percentage.ToString("F2") + "% chance)";

	}
}