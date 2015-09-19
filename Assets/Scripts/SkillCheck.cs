using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCheck : MonoBehaviour {

	public Slider APSlider;
	public Text APText;
	private InteractiveObject Current;
	private GameObject Player = GameObject.Find ("Player");
	//private Player PlayerScript = Player.GetComponent<Player>();

	void Start(){

		//GameObject.FindGameObjectWithTag ("Interactable").GetComponent<InteractiveObject>().CloseEvent();
	}

	void Update(){
		/*double Multiplier = 1;
		Multiplier = PlayerScript.GetPlayerClassObject().GetDefaultStat(x);
		int actualAP = (int)Mathf.Floor((float)(PlayerScript.GetStatValue(Stat.AP) * (float)APSlider.value / 100f));
		int multipliedAP = (int)Mathf.Floor((float)actualAP * (float)Multiplier);
		APText.text = multipliedAP;*/
	}

	public void Action(GameObject x) {
		x.GetComponent<SkillCheck>().Action();
	}

	public void Action(){
		Current.TakeAction((int)APSlider.value);
	}

	public void SetCurrent(InteractiveObject i) {
		this.Current = i;
	}

}
