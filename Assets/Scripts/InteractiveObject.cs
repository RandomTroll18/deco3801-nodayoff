using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//class representing Interactive Objects
public class InteractiveObject : MonoBehaviour {
    
	//check if player is interacting with object
	//public bool IsInteracting = false;

	//the UICanvas
	public GameObject Panel;
	public Text NameLabel;
	//public Text DescriptionLabel;
	public Slider APSlider;
	public Button Button;
	//public Tile Target;


	private int ObjectID;
	private Tile Position;

	private string Name;
	private string Description;
	private int APLimit;
	private int multiplier;
	private int RNGvariable;
	private bool IsActivated;
	private bool IsBlocked;
	private bool IsClosed;


	void Start() {

		this.ObjectID = 0;

		this.Position = new Tile(
			Tile.TilePosition(this.transform.position.x), 
			Tile.TilePosition(this.transform.position.z)
			);
		
		this.Name = "Test";
		this.Description = "Lorum Ipsum";
		this.APLimit = 3;
		this.multiplier = 1;
		this.RNGvariable = 1;
		this.IsActivated = false;
		this.IsClosed = true;


		Debug.Log (this.Position.ToString());
		
		
	}
	
	public void Interact() {
		Debug.Log ("Interacted with " + this.name + " at " + this.Position.ToString());
		// TODO: Figure whats wrong with code below & add toggle Panel
		if (Panel.activeInHierarchy) {
			Panel.SetActive(false);
		} else {
			// TODO: Change text
			NameLabel.text = this.Name;
			// TODO: Change Button function
			Panel.GetComponent<EventCard>().SetCurrent(this);
			// TODO: Change Slider properties
			APSlider.value = 0;
			APSlider.maxValue = this.APLimit;
			Panel.SetActive(true);
		}
		return;
	}

	public void TakeAction(float input){
		if (input == 0)
			return;
		Debug.Log (input + "AP has been used on " + this.Name);
	}

	public Tile getTile(){
		return this.Position;
	}


    
}

public class Trap : InteractiveObject {
	// Activate Trap function
	public virtual void ActivateTrap(){
		// TODO: Add Trap effect
	}
}

public class PopUp : Trap {
	// Activate Trap function
	public override void ActivateTrap(){
		// TODO: Add Trap effect
		// TODO: Show popup
	}
}

public class StunTrap : Trap {
	// Activate Trap function
	public override void ActivateTrap(){
		// TODO: Add Trap effect

	}
}

public class Removeable : InteractiveObject {
	public virtual void TryToRemove(){
		// TODO: Add Trap effect
	}
}

public class Rubble : Removeable {
	
}

public class Terminal : InteractiveObject {
	
}

public class Door : InteractiveObject {
	
}