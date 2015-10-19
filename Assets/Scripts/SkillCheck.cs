using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCheck : MonoBehaviour {
	public Stat ClassMultiplier = Stat.NOMULTIPLIER;
	public int Cost = 0;

	public Slider APSlider;
	private InteractiveObject Current;

	void Start(){

		//GameObject.FindGameObjectWithTag ("Interactable").GetComponent<InteractiveObject>().CloseEvent();
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

	public void SetMultiplierAndCost(Stat multiplier, int cost) {
		ClassMultiplier = multiplier;
		Cost = cost;
	}

}
