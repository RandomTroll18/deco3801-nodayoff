using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillSlider : MonoBehaviour {

	string sliderTextString;
	Text sliderText;
	
	void start(){
		sliderText = GetComponent<Text> ();
	}
	
	public void textUpdate (float textUpdateNumber){
		textUpdateNumber.ToString (sliderTextString);
		sliderText.text = sliderTextString;
	}
	// Update is called once per frame
	void Update () {
		
	}
}