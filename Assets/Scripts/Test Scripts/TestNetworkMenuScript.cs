using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestNetworkMenuScript : MonoBehaviour {

	/* Input Fields */
	public InputField NameInputField;
	public InputField RoomInputField;
	public Text ErrorMessageLabelField; // Error message

	/**
	 * Load the test scene
	 */
	public void LoadNetworkTestScene() {
		if (NameInputField.text.Length == 0) ErrorMessageLabelField.text = "No name given";
		else if (RoomInputField.text.Length == 0) ErrorMessageLabelField.text = "No room given";
		else {
			ErrorMessageLabelField.text = "All fields are good";
			TestMatchMaker.PlayerName = NameInputField.text;
			TestMatchMaker.RoomName = RoomInputField.text;
			Application.LoadLevel("TestScene1");
		}
	}
}
