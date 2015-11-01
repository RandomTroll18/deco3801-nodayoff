using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class APCounterScript : MonoBehaviour {

	public GameObject Owner; // The owner of this AP Counter
	public Text APCounterTextObject; // The text object of the AP counter

	public void StartMe() {
		SetPublicVariables();
	}

	void SetPublicVariables() {
		Owner = Player.MyPlayer;
	}

	/**
	 * Update function
	 */
	void Update() {
		try {
			Player playerScript = Owner.GetComponent<Player>();
			if (playerScript == null) 
				return; // Not a player
			if (playerScript.IsSpawned) 
				APCounterTextObject.text = "" + playerScript.GetStatValue(Stat.AP);
			else 
				APCounterTextObject.text = "" + playerScript.GetStatValue(Stat.AP);
		} 
		catch (MissingReferenceException) { // Handle Security System Kill state
			Application.LoadLevel("GameOver");
		}
	}

}
