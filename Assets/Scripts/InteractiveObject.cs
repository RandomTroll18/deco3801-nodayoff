using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//class representing Interactive Objects
public class InteractiveObject : MonoBehaviour {
    
	//check if player is interacting with object
	//public bool IsInteracting = false;

	//the UICanvas
	//NOTE: Will there be problems with Blocking and Unblocking a tile with multiple players since each player uses their own MoveController???
	public GameObject Player;
	public GameObject Panel;
	public GameObject[] Targets;

	public Text NameLabel;
	public string StringInput;

	public Slider APSlider;
	public int APLimit;

	public Button Button;
	public bool FunctionToggle;
	
	private int ObjectID;
	private Tile Position;

	public bool IsActivated;
	public bool IsBlocked;
	public bool IsClosed;

	private MovementController MController;


	void Awake() {

		if (Targets.Length == 0) {
			// Set Target to self
		}
		this.ObjectID = 0;
		this.Position = new Tile(
			Tile.TilePosition(this.transform.position.x), 
			Tile.TilePosition(this.transform.position.z)
		);

		/*
		this.Name = "Test";
		this.Description = "Lorum Ipsum";
		this.APLimit = 3;
		this.RNGvariable = 1; 
		 */

		this.IsActivated = false;
		this.IsClosed = true;

		this.MController = Player.GetComponent<MovementController>();
		Debug.Log (this.Position.ToString());		
		
	}
	
	public void Interact() {
		Debug.Log ("Interacted with " + this.name + " at " + this.Position.ToString());
		// TODO: Figure whats wrong with code below & add toggle Panel
		Panel.SetActive(true);
		// TODO: Change text
		NameLabel.text = StringInput;
		// TODO: Change Button function
		Panel.GetComponent<SkillCheck>().SetCurrent(this);
		// TODO: Change Slider properties
		APSlider.value = 0;
		APSlider.maxValue = this.APLimit;
		Panel.SetActive(true);

		return;
	}

	public void CloseEvent(){
		Panel.SetActive(false);
	}

	public void TakeAction(float input){
		if (IsActivated) {
			Debug.Log ("Interactable is already activated");
			return;
		}

		if (input == 0) {
			Debug.Log ("Input AP");
			return;
		}

		//PlayerClass Class = this.Player.GetPlayerClassObject();
		//double Multiplier;

		Debug.Log (input + "AP has been used on " + gameObject.ToString());
		// TODO: RNG Element to opening
		if (FunctionToggle) {
			if (IsClosed) {
				this.IsClosed = false;
				//this.Open(this.GetTile());
				TargetOpen ();
				Debug.Log ("OPEN");
			} else {
				this.IsClosed = true;
				//this.Close(this.GetTile());
				TargetClose ();
				Debug.Log ("CLOSE");
			}
		} else {
			this.TargetDestroy();
		}
	}

	public Tile GetTile(){
		return this.Position;
	}

	public void Open(Tile t){
		MController.UnblockTile(t);
	}

	public void Close(Tile t){
		MController.BlockTile(t);
	}

	public void TargetOpen(){
		foreach (GameObject Target in Targets) {
			Tile t = new Tile (
				Tile.TilePosition (Target.transform.position.x), 
				Tile.TilePosition (Target.transform.position.z)
			);
			MController.UnblockTile (t);
		}
	}

	public void TargetClose(){
		foreach (GameObject Target in Targets) {
			Tile t = new Tile(
				Tile.TilePosition(Target.transform.position.x), 
				Tile.TilePosition(Target.transform.position.z)
			);
			MController.BlockTile(t);
		}
	}

	public void TargetDestroy(){
		foreach (GameObject Target in Targets) {
			Destroy(Target);
		}
		this.IsActivated = true;
	}
}

