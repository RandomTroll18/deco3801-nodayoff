using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//class representing Interactive Objects
public class InteractiveObject : MonoBehaviour {
    
	//check if player is interacting with object
	//public bool IsInteracting = false;

	//the UICanvas
	//NOTE: Will there be problems with Blocking and Unblocking a tile with multiple players since each player uses their own MoveController???
	private GameObject Player;
	private GameObject Panel;
	public string SkillType;

	private Text NameLabel;
	public string StringInput;

	private Slider APSlider;
	public int APLimit;

	private Button Button;
	private Tile Position;

	public double ChanceDecimal;

	protected double Chance;
	protected Player playerScript;
	protected bool IsInactivated;
	protected PrimaryObjectiveController PrimaryO;
	protected MovementController MController;


	void Awake() {

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

		IsInactivated = false;

		Panel = GameObject.FindGameObjectWithTag("SkillPanel");
		NameLabel = Panel.transform.FindChild("SkillCheckText").GetComponent<Text> ();
		APSlider = Panel.transform.FindChild("Slider").GetComponent<Slider> ();
		Button = Panel.transform.FindChild("Button").GetComponent<Button> ();

		Chance = ChanceDecimal;
		MController = GameObject.FindGameObjectWithTag("GameController")
			.GetComponent<MovementController>();
		Player = GameObject.Find ("Player");
		PrimaryO = GameObject.FindGameObjectWithTag ("Objective UI").GetComponent<PrimaryObjectiveController> ();
		playerScript = Player.GetComponent<Player>();
		MController.AddInteractable (this);
		Debug.Log (this.Position.ToString());		
		
	}
	
	public void Interact() {
		Debug.Log ("Interacted with " + this.name + " at " + this.Position.ToString());
		// TODO: Figure whats wrong with code below & add toggle Panel
		// TODO: Change text
		NameLabel.text = StringInput;
		// TODO: Change Button function
		Panel.GetComponent<SkillCheck>().SetCurrent(this);
		// TODO: Change Slider properties
		APSlider.value = 0;
		APSlider.maxValue = this.APLimit;
		OpenEvent();
		return;
	}

	public void CloseEvent(){
		Debug.Log ("Close Panel " + Panel.GetComponent<RectTransform>().anchoredPosition.ToString());
		Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)-9999, (float)37.5);
	}

	public void OpenEvent(){
		Debug.Log ("Open Panel " + Panel.GetComponent<RectTransform>().anchoredPosition.ToString());
		Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)-683, (float)37.5);
	}

	
	public virtual void TakeAction(float input){

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

	/*
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
		IsInactivated = true;
	}
	*/


}

