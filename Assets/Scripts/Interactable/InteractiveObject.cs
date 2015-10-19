using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//class representing Interactive Objects
public class InteractiveObject : MonoBehaviour {
    
	//check if player is interacting with object
	//public bool IsInteracting = false;
	public string StringInput;
	public int Cost;
	public Stat ClassMultiplier;

	//the UICanvas
	//NOTE: Will there be problems with Blocking and Unblocking a tile with multiple players since each player uses their own MoveController???
	GameObject player;
	GameObject panel;
	Text nameLabel;
	Slider APSlider;
	//public int APLimit;
	Button button;
	Tile position;

	protected int MinCost;
	protected Player PlayerScript;
	protected bool IsInactivated;
	protected PrimaryObjectiveController PrimaryO;
	protected MovementController MController;
	public bool InstantInteract;
	
	public bool DebugOption = false;

	public virtual void StartMe() {
		StartMe(Object.FindObjectOfType<GameManager>());
	}

	public void StartMe(GameManager g) {

		if (DebugOption) Debug.Log("Started");

		this.position = new Tile(
			Tile.TilePosition(this.transform.position.x), 
			Tile.TilePosition(this.transform.position.z)
		);

		IsInactivated = false;
		MinCost = Cost;
		panel = GameObject.FindGameObjectWithTag("SkillPanel");
		nameLabel = panel.transform.FindChild("SkillCheckText").GetComponent<Text> ();
		APSlider = panel.transform.FindChild("Slider").GetComponent<Slider> ();
		button = panel.transform.FindChild("Button").GetComponent<Button> ();

		//MController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MovementController>();
		MController = g.GetPlayerControllers();
		//MController = Player.MyPlayer.GetComponent<MovementController>();
		player = GameObject.Find ("Player");
		PrimaryO = GameObject.FindGameObjectWithTag ("Objective UI")
			.GetComponent<PrimaryObjectiveController> ();
		player = Player.MyPlayer; 
		PlayerScript = player.GetComponent<Player>();
		MController.AddInteractable(this);

		if (DebugOption) Debug.Log(this.position.ToString());		
		
	}

	/**
	 * Set the player we are tracking
	 * 
	 * Arguments
	 * - GameObject playerObject - The game object of the player
	 */
	public void ChangeTrackedPlayer(GameObject playerObject) {
		player = playerObject;
		PlayerScript = playerObject.GetComponent<Player>();
	}
	
	public void Interact() {
		if (DebugOption) Debug.Log ("Interacted with " + this.name + " at " + this.position.ToString());

		Object.FindObjectOfType<SkillCheck>().SetMultiplierAndCost(ClassMultiplier, Cost);

		if (PlayerScript.IsSpawned) 
			return; // A spawned player cannot interact with this object
		else if (IsInactivated) // Already activated
			return;


		string classText = "";
		if (!ClassMultiplier.Equals(Stat.NOMULTIPLIER)) {
			classText = ": " + ClassMultiplier.ToString();
		}
		
		// TODO: Figure whats wrong with code below & add toggle Panel
		// TODO: Change text
		nameLabel.text = StringInput + " (" + MinCost + classText + ")";
		// TODO: Change Button function
		panel.GetComponent<SkillCheck>().SetCurrent(this);
		// TODO: Change Slider properties
		APSlider.value = 0;
		//APSlider.maxValue = this.APLimit;
		if (InstantInteract)
			TakeAction(0);
		else
			OpenEvent();
		return;
	}

	public bool SpendAP(int input, int cost) {
		double Multiplier = 1;
		if (!ClassMultiplier.Equals(Stat.NOMULTIPLIER))
			Multiplier = PlayerScript.GetPlayerClassObject().GetStat(ClassMultiplier);

		int actualAP = 
			(int) Mathf.Floor(
			(float)(PlayerScript.GetStatValue(Stat.AP) * (float) input / 100f)
			);

		PlayerScript.ReduceStatValue(Stat.AP, actualAP);

		int multipliedAP = 
			(int) Mathf.Floor(
			(float) actualAP * (float)Multiplier
			); // TODO: add character class element

		int rng = Random.Range(1, multipliedAP);

		if (DebugOption) Debug.Log("Rolled " + rng + " when used: " + actualAP + "(" + multipliedAP + ")");

		//CloseEvent();
		if (rng >= cost)
			return true;
		else
			return false;
	}

	public void CloseEvent(){
		if (DebugOption) Debug.Log ("Close Panel " + panel.GetComponent<RectTransform>().anchoredPosition.ToString());
		panel.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)-9999, (float)37.5);
	}

	public void OpenEvent(){
		if (DebugOption) Debug.Log ("Open Panel " + panel.GetComponent<RectTransform>().anchoredPosition.ToString());
		panel.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)-683, (float)37.5);
	}

	
	public virtual void TakeAction(int input){

	}

	public Tile GetTile(){
		return this.position;
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

