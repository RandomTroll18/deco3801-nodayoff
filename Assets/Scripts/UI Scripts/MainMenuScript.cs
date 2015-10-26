using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	public static string LevelToLoad; // The level user wants to load

	/*
	 * Simply load the level selected
	 * 
	 * Arguments
	 * - string level - The level to load
	 */
	public void LoadLevel(string level) {
		switch (level) { // Do actions depending on what level was loaded
		case "MainMenu": // Main menu being loaded
			Player.ChosenClass = null; // Player has not chosen a class
			SoundManagerScript.Singleton.PlayBGMusic(SoundManagerScript.Singleton.BGMusicList[6]);
			break;
		case "Main Level": // Main level being loaded
			// Play either one of the long notes
			SoundManagerScript.Singleton.PlayBGMusic(0, 1);
			break;
		default: // Nothing to do
			Debug.LogWarning("Unhandled level to load");
			break;
		}
		Application.LoadLevel(level);
	}

	/*
	 * Load class selection level
	 */
	public void LoadClassSelect() {
		Application.LoadLevel("ClassSelect");
	}

	/*
	 * Load the level the player wants to load with the given class
	 * 
	 * Arguments
	 * - string chosenClass - The class the player has chosen
	 */
	public void LoadLevelWithClass(string chosenClass) {
		Player.ChosenClass = chosenClass;

		if (PhotonNetwork.connected) { // Set selected class for match-making
			Debug.Log("Setting selected class for network player");
			switch (chosenClass) {
			case "Engineer":
				Debug.LogWarning("Setting eng unselectable");
				PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
					{"classselected", 0}
				});
				break;
			case "Technician":
				Debug.LogWarning("Setting tech unselectable");
				PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
					{"classselected", 1}
				});
				break;
			case "Scout":
				Debug.LogWarning("Setting scout unselectable");
				PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
					{"classselected", 2}
				});
				break;
			case "Marine":
				Debug.LogWarning("Setting marine unselectable");
				PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
					{"classselected", 3}
				});
				break;
			default:
				Debug.Log("Setting null class");
				break;
			}
		}
		if (LevelToLoad == null) {
			Debug.Log("No level to load");
			LoadLevel("Main Level");
		} else { 
			Debug.Log("There is a level to load");
			LoadLevel(LevelToLoad);
		}

	}


	/*
	 * Remember what the player wants to load
	 * 
	 * Arguments
	 * - The level the player wants to load
	 */
	public void RememberLevel(string level) {
		LevelToLoad = level;
	}

	/*
	 * Function used to quit the application
	 */
	public void Quit() {
		Application.Quit();
	}
}
