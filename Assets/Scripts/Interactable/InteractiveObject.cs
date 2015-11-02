using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/**
 * Super class for all interactive object scripts
 */
public class InteractiveObject : MonoBehaviour {
    
	public string StringInput; // The string given to this interactive object
	public int Cost; // The cost for this interactive object 
	public Stat ClassMultiplier; // The class multiplier we are currently looking at
	public bool InstantInteract; // Record whether or not this object should activate without spending AP
	public bool DebugOption = false; // Debug option

	/* Succeed and failure sound effets */
	public List<AudioClip> SuccessEfx;
	public List<AudioClip> FailEfx;
	
	GameObject player; // The current player we are focusing on
	GameObject panel; // The panel we are attached to
	Text nameLabel; // The name object of this interactive object
	Slider APSlider; // The AP slider
	Tile position; // The position of the interactive object

	protected int MinCost; // The minimum cost to complete this object
	protected Player PlayerScript; // The player script of the player we are tracking
	protected bool IsInactivated; // Record if this object is not activated
	protected PrimaryObjectiveController PrimaryO; // The Primary Objective Controller
	protected MovementController MController; // The movement controller script of the player

	/**
	 * Initialize this interactive object
	 */
	public virtual void StartMe() {
		ClassMultiplier = Stat.NOMULTIPLIER;
		StartMe(Object.FindObjectOfType<GameManager>());
	}

	/**
	 * Overload for intializing this interactive object. 
	 * 
	 * Arguments
	 * - GameManager g - The game manager script
	 */
	public void StartMe(GameManager g) {

		if (DebugOption) 
			Debug.Log("Started");

		position = new Tile(
			Tile.TilePosition(transform.position.x), 
			Tile.TilePosition(transform.position.z)
		);

		IsInactivated = false;
		MinCost = Cost;
		panel = GameObject.FindGameObjectWithTag("SkillPanel");
		nameLabel = panel.transform.FindChild("SkillCheckText").GetComponent<Text>();
		APSlider = panel.transform.FindChild("Slider").GetComponent<Slider>();

		MController = g.GetPlayerControllers();
		player = GameObject.Find ("Player");
		PrimaryO = GameObject.FindGameObjectWithTag ("Objective UI")
			.GetComponent<PrimaryObjectiveController>();
		player = Player.MyPlayer; 
		PlayerScript = player.GetComponent<Player>();
		MController.AddInteractable(this);

		if (DebugOption) 
			Debug.Log(position.ToString());		
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
		string classText = ""; // The string representing the player's class

		if (DebugOption) 
			Debug.Log("Interacted with " + name + " at " + position.ToString());

		Object.FindObjectOfType<SkillCheck>().SetMultiplierAndCost(ClassMultiplier, Cost);

		if (PlayerScript.IsSpawned) // A spawned player cannot interact with this object
			return; 
		else if (IsInactivated) // Already activated
			return;

		if (!ClassMultiplier.Equals(Stat.NOMULTIPLIER)) { // Class exists
			switch (ClassMultiplier) {
			case Stat.MARINEMULTIPLIER:
				classText = ": Marine";
				break;
			case Stat.ENGMULTIPLIER:
				classText = ": Engineer";
				break;
			case Stat.TECHMULTIPLIER:
				classText = ": Technician";
				break;
			default: // No class
				classText = "";
				break;
			}

		}
		
		nameLabel.text = StringInput + " (" + MinCost + classText + ")";
		panel.GetComponent<SkillCheck>().SetCurrent(this);
		APSlider.value = 0;

		if (InstantInteract) // Immediately take the action
			TakeAction(0);
		else // Start an event
			OpenEvent();
		return;
	}

	/**
	 * Handle the spending of AP
	 * 
	 * Arguments
	 * - int input - The AP being spent
	 * - int cost - The cost for this Interactive Object
	 */
	public bool SpendAP(int input, int cost) {
		double multiplier = 1; // The multiplier
		int multipliedAP, rng; // The multiplied AP and the amount of AP applied to this interactable

		if (!ClassMultiplier.Equals(Stat.NOMULTIPLIER))
			multiplier = PlayerScript.GetPlayerClassObject().GetStat(ClassMultiplier);

		int actualAP = (int)Mathf.Floor((float)(PlayerScript.GetStatValue(Stat.AP) * (float) input / 100f));

		PlayerScript.ReduceStatValue(Stat.AP, actualAP);

		/* Calculate the real amount of AP spent and determine the true  */
		multipliedAP = (int) Mathf.Floor((float) actualAP * (float)multiplier);
		rng = Random.Range(1, multipliedAP);

		if (DebugOption) 
			Debug.Log("Rolled " + rng + " when used: " + actualAP + "(" + multipliedAP + ")");

		if (rng >= cost) // AP roll succeeded
			return true;
		else { // Failed
			ChatTest.Instance.AllChat(true, "FAILED");
			return false;
		}
	}

	/**
	 * Actions to take when closing the event
	 */
	public void CloseEvent(){
		if (DebugOption) 
			Debug.Log("Close Panel " + panel.GetComponent<RectTransform>().anchoredPosition.ToString());
		// Move panel out of the UI
		panel.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)-9999, (float)37.5);
	}

	/**
	 * Actions to take when initiating this event
	 */
	public void OpenEvent() {
		if (DebugOption) 
			Debug.Log("Open Panel " + panel.GetComponent<RectTransform>().anchoredPosition.ToString());
		// Move panel into the UI
		panel.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)-683, (float)37.5);
	}

	/**
	 * Execute the action in this interactable
	 * 
	 * Arguments
	 * - int input - Ken. I don't know what this is for :P
	 */
	public virtual void TakeAction(int input) {

	}

	/**
	 * Get the position of this interactive object
	 */
	public Tile GetTile() {
		return position;
	}

	/**
	 * Unblock/Open this interactive object to allow players to 
	 * move in it
	 */
	public void Open(Tile t) {
		MController.UnblockTile(t);
	}

	/**
	 * Block/Close this interactive object, preventing 
	 * players from moving in it
	 */
	public void Close(Tile t) {
		MController.BlockTile(t);
	}

	/**
	 * Set this interactive object is inactive
	 */
	public void SetInactive() {
		IsInactivated = true;
	}

	/**
	 * Set this interactabe to be active
	 */
	public void SetActive() {
		IsInactivated = false;
	}

	/**
	 * Play sound effects for success
	 */
	protected void PlaySuccessEfx() {
		if (SoundManagerScript.Singleton != null) {
			SoundManagerScript.Singleton.gameObject.transform.position = gameObject.transform.position;
			SoundManagerScript.Singleton.PlaySingle3D(SuccessEfx);
		}
	}

	/**
	 * Play sound effects for failure
	 */
	protected void PlayFailureEfx() {
		if (SoundManagerScript.Singleton != null) {
			SoundManagerScript.Singleton.gameObject.transform.position = gameObject.transform.position;
			SoundManagerScript.Singleton.PlaySingle3D(FailEfx);
		}
	}
	
}

