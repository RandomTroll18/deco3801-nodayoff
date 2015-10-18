using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorAudio : MonoBehaviour {

	public List<AudioClip> OpenDoorEfx; // Sound effects for opening doors

	/**
	 * Play door opening audio
	 */
	public void PlayOpeningEfx() {
		if (SoundManagerScript.Singleton != null) {
			// Move sound manager to this door
			SoundManagerScript.Singleton.gameObject.transform.position = gameObject.transform.position;
			SoundManagerScript.Singleton.PlaySingle3D(OpenDoorEfx);
		}
	}
}
