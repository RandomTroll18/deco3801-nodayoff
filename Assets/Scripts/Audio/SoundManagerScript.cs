using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundManagerScript : MonoBehaviour {

	/* The Sound Effect Sources */
	public AudioSource EfxSource1;
	public AudioSource EfxSource2;
	public AudioSource EfxSource3;
	public AudioSource BGMusicSource; // BG Music Source
	public bool BGMute; // Record if we need to mute bg music
	public bool EfxMute; // Record if we need to mute efx
	public float EfxVolume; // Volume for effects
	public float BGVolume; // Volume for bg music
	public List<AudioClip> BGMusicList; // List of BG Music

	public static SoundManagerScript Singleton = null; // The singleton representing this script
	
	/**
	 * Awake function
	 */
	void Awake() {
		if (Singleton == null)
			Singleton = this;
		else if (Singleton != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	/**
	 * For randomly playing background music
	 */
	void Update() {
		/* Set volume */
		BGMusicSource.volume = BGVolume;
		EfxSource1.volume = EfxSource2.volume = EfxSource3.volume = EfxVolume;

		/* Mute if needed */
		BGMusicSource.mute = BGMute;
		EfxSource1.mute = EfxSource2.mute = EfxSource3.mute = EfxMute;
	}

	/**
	 * Play a random selection of music from the given range of bg music (exclusive)
	 * 
	 * Arguments
	 * - int start - The starting index
	 * - int end - End index
	 */
	public void PlayBGMusic(int start, int end) {
		if (start > end || end >= BGMusicList.Count || start < 0)
			return;
		PlayBGMusic(BGMusicList[Random.Range(start, end + 1)]);
	}

	/**
	 * Play a random selection of music from the given list of bg music
	 * 
	 * Arguments
	 * - List<AudioClip> bgList - List of bg music to play
	 */
	public void PlayBGMusic(List<AudioClip> bgList) {
		if (bgList == null || bgList.Count <= 0)
			return;
		PlayBGMusic(bgList[Random.Range(0, bgList.Count)]);
	}

	/**
	 * Play a given background music
	 * 
	 * Arguments
	 * - AudioClip bg - The background music to play
	 */
	public void PlayBGMusic(AudioClip bg) {
		if (isBGMusicPlaying(bg)) { // Required bg is already being played
			Debug.Log("BG given is already playing");
			return;
		} else if (bg == null)
			return;
		Debug.Log("It's not playing");
		BGMusicSource.Stop();
		BGMusicSource.clip = bg;
		BGMusicSource.Play();
	}

	/**
	 * Determine the available efx source
	 * 
	 * Returns
	 * - The efx source that is currently available to play
	 */
	AudioSource determineAvailableEfxSource() {
		if (EfxSource1.isPlaying) { // 1st effect source is playing. Check next one
			if (EfxSource2.isPlaying) { // 2nd effect source is still playing. Check last one
				if (EfxSource3.isPlaying) // Disable it
					EfxSource3.Stop();
				return EfxSource3;
			} else // Choose EfxSource 2
				return EfxSource2;
		} else
			return EfxSource1;
	}

	/**
	 * Play a single audio clip. Used for unique sound effects
	 * 
	 * Arguments
	 * - AudioClip clip - The sound clip
	 */
	public void PlaySingle(AudioClip clip) {
		AudioSource toPlay = determineAvailableEfxSource(); // The effect source to play

		if (clip == null)
			return;

		toPlay.clip = clip;
		toPlay.spatialBlend = 0.0f; // Full spatial blend
		toPlay.Play();
	}

	/**
	 * Play a random single clip from the given list of sound effects
	 * 
	 * Arguments
	 * - List<AudioClip> efx - List of sound effects
	 */
	public void PlaySingle(List<AudioClip> efx) {
		if (efx == null || efx.Count <= 0)
			return;

		PlaySingle(efx[Random.Range(0, efx.Count)]);
	}

	/**
	 * Play a single audio clip with 3D effect
	 * 
	 * Arguments
	 * - AudioClip clip - The sound clip
	 */
	public void PlaySingle3D(AudioClip clip) {
		AudioSource toPlay = determineAvailableEfxSource(); // The effect source to play

		if (clip == null)
			return;

		toPlay.clip = clip;
		toPlay.spatialBlend = 1.0f; // Full spatial blend
		toPlay.Play();
	}

	/**
	 * Play a random single clip from list of sound effects with 3D effect
	 * 
	 * Arguments
	 * - List<AudioClip> efx - List of sound effects
	 */
	public void PlaySingle3D(List<AudioClip> efx) {
		if (efx == null || efx.Count <= 0)
			return;

		PlaySingle3D(efx[Random.Range(0, efx.Count)]);
	}

	/**
	 * Check if the given bg music clip is playing
	 * 
	 * Arguments
	 * - AudioClip toCompare - The bg music we are checking for
	 * 
	 * Returns
	 * - true if this bg music is playing. false otherwise
	 */
	bool isBGMusicPlaying(AudioClip toCompare) {
		if (!BGMusicSource.isPlaying) // BG source is not even playing
			return false;
		else if (BGMusicSource.isPlaying && BGMusicSource.clip == toCompare) // Found match
			return true;
		return false;
	}
}
