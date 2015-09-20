using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillSlider : MonoBehaviour {
	
	public Text SliderText;
	public Slider APSlider;

	public void TextUpdate (){

		GameObject Player = GameObject.Find ("Player");
		Player PlayerScript = Player.GetComponent<Player>();

		double Multiplier = 1;
		//Multiplier = PlayerScript.GetPlayerClassObject().GetDefaultStat();
		int actualAP = (int)Mathf.Floor((float)(PlayerScript.GetStatValue(Stat.AP) * (float)APSlider.value / 100f));
		int multipliedAP = (int)Mathf.Floor((float)actualAP);// * (float)Multiplier);
		SliderText.text = multipliedAP.ToString();

	}
		
	// Update is called once per frame
	void Update () {
		
	}
}