using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventCard : MonoBehaviour {

	public Slider APSlider;

	private InteractiveObject Current;


	void Start(){

	}

	public void Action(GameObject x) {
		x.GetComponent<EventCard>().Action();
	}

	public void Action(){
		Current.TakeAction(APSlider.value);
	}

	public void SetCurrent(InteractiveObject i) {
		this.Current = i;
	}

}
