using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundSettingMenuScript : MonoBehaviour {

	public Toggle BGMuteToggle; // Toggle for muting bg music
	public Toggle EfxMuteToggle; // Toggle for muting sound effects
	public Slider BGVolumeAdjuster; // Slider for adjusting bg music volume
	public Slider EfxVolumeAdjuster; // Slider for adjusting sound effect volume

	void Start() {
		if (SoundManagerScript.Singleton == null) // Sound manager is null
			return;

		BGMuteToggle.isOn = SoundManagerScript.Singleton.BGMusicSource.mute;
		EfxMuteToggle.isOn = SoundManagerScript.Singleton.EfxSource.mute;
		BGVolumeAdjuster.value = SoundManagerScript.Singleton.BGMusicSource.volume;
		EfxVolumeAdjuster.value = SoundManagerScript.Singleton.EfxSource.volume;
	}

	void Update() {
		if (SoundManagerScript.Singleton == null) // Sound manager is null
			return;

		SoundManagerScript.Singleton.BGMusicSource.mute = BGMuteToggle.isOn;
		SoundManagerScript.Singleton.EfxSource.mute = EfxMuteToggle.isOn;
		SoundManagerScript.Singleton.BGMusicSource.volume = BGVolumeAdjuster.value;
		SoundManagerScript.Singleton.EfxSource.volume = EfxVolumeAdjuster.value;
	}
}
