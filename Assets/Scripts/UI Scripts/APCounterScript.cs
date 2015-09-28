using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class APCounterScript : MonoBehaviour {

	public GameObject Owner; // The owner of this AP Counter
	public Text APCounterTextObject; // The text object of the AP counter

	/**
	 * Update function
	 */
	void Update() {
		Player playerScript = Owner.GetComponent<Player>();
		if (playerScript == null) return; // Not a player
		if (playerScript.IsSpawned) APCounterTextObject.text = "Spawn AP Count: " + playerScript.GetStatValue(Stat.AP);
		else APCounterTextObject.text = "Player AP Count: " + playerScript.GetStatValue(Stat.AP);
	}

}
