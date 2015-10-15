using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundManagerScript : MonoBehaviour {
	
	public AudioSource EfxSource; // Sound Effect Source 
	public AudioSource BGMusicSource; // BG Music Source
	public bool BGMute = false; // Record if we need to mute bg music
	public bool EfxMute = false; // Record if we need to mute efx
	public float EfxVolume = 2; // Volume for effects
	public float BGVolume = 2; // Volume for bg music
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
		if (!BGMusicSource.isPlaying)
			playDefaultBGMusic();

		/* Set volume */
		BGMusicSource.volume = BGVolume;
		EfxSource.volume = EfxVolume;

		/* Mute if needed */
		BGMusicSource.mute = BGMute;
		EfxSource.mute = EfxMute;
	}

	/**
	 * Play default background music
	 */
	void playDefaultBGMusic() {
		BGMusicSource.clip = BGMusicList[Random.Range(0, BGMusicList.Count)];
		BGMusicSource.Play();
	}

	/**
	 * Play a given background music
	 * 
	 * Arguments
	 * - AudioClip bg - The background music to play
	 */
	public void PlayBGMusic(AudioClip bg) {
		BGMusicSource.Stop();
		BGMusicSource.clip = bg;
		BGMusicSource.Play();
	}

	/**
	 * Play a single audio clip. Used for unique sound effects
	 * 
	 * Arguments
	 * - AudioClip clip - The sound clip
	 */
	public void PlaySingle(AudioClip clip) {
		EfxSource.clip = clip;
		EfxSource.Play();
	}

	/**
	 * Play a random single clip from the given list of sound effects
	 * 
	 * Arguments
	 * - List<AudioClip> efx - List of sound effects
	 */
	public void PlaySingle(List<AudioClip> efx) {
		EfxSource.clip = efx[Random.Range(0, efx.Count)];
		EfxSource.Play();
	}
}
