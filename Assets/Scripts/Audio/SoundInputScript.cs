using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundInputScript : MonoBehaviour {

	public List<AudioClip> SoundEfxToPlay; // The sound effects to play

	/**
	 * Play sound effects
	 */
	public void PlaySoundEfx() {
		if (SoundEfxToPlay.Count <= 0) // Nothing to play
			return; 

		if (SoundManagerScript.Singleton != null) // Only play if singleton exists
			SoundManagerScript.Singleton.PlaySingle(SoundEfxToPlay);
	}

	/**
	 * Play 3D sound effects
	 */
	public void PlaySoundEfx3D() {
		if (SoundEfxToPlay.Count <= 0) // Nothing to play
			return;

		if (SoundManagerScript.Singleton != null) // Only play if singleton exists
			SoundManagerScript.Singleton.PlaySingle3D(SoundEfxToPlay);
	}

	
	/**
	 * Play sound effects with the volume indicated
	 * 
	 * Arguments
	 * - float volume - The volume
	 */
	public void PlaySoundEfx(float volume) {
		if (SoundEfxToPlay.Count <= 0) // Nothing to play
			return; 
		
		if (SoundManagerScript.Singleton != null) // Only play if singleton exists
			SoundManagerScript.Singleton.PlaySingle(SoundEfxToPlay);
	}
	
	/**
	 * Play 3D sound effects with the volume indicated
	 * 
	 * Arguments
	 * - float volume - The volume
	 */
	public void PlaySoundEfx3D(float volume) {
		if (SoundEfxToPlay.Count <= 0) // Nothing to play
			return;
		
		if (SoundManagerScript.Singleton != null) // Only play if singleton exists
			SoundManagerScript.Singleton.PlaySingle3D(SoundEfxToPlay);
	}
}
