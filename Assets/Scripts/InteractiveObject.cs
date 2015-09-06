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
	public Slider Slider;
	public Button Button;

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

	public InteractiveObject(int ID, Tile t) {
		this.ObjectID = ID;
		this.Position = t;

		this.Name = "Test";
		this.Description = "Lorum Ipsum";
		this.APLimit = 3;
		this.multiplier = 1;
		this.RNGvariable = 1;
		this.IsActivated = false;
		this.IsClosed = true;
	
	}
	
	public InteractiveObject() {
		;
	}
	
	public void Interact() {
		Debug.Log ("Interacted with " + this.name + " at " + this.Position.ToString());
		// TODO: Figure whats wrong with code below & add toggle Panel
		/*
		Debug.Log (Panel.activeInHierarchy);
		if (Panel.activeInHierarchy) {
			Panel.SetActive(false);
		} else {
			Panel.SetActive(true);
		}
		*/
		// TODO: Change text
		NameLabel.text = this.name;
		// TODO: Change Button function
		// TODO: Change Slider properties

		return;
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