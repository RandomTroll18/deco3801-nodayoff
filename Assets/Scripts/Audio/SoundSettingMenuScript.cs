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

		/* Get current volume and mute setting */
		BGMuteToggle.isOn = SoundManagerScript.Singleton.BGMute;
		EfxMuteToggle.isOn = SoundManagerScript.Singleton.EfxMute;
		BGVolumeAdjuster.value = SoundManagerScript.Singleton.BGVolume;
		EfxVolumeAdjuster.value = SoundManagerScript.Singleton.EfxVolume;
	}

	void Update() {
		if (SoundManagerScript.Singleton == null) // Sound manager is null
			return;

		/* Set current volume and mute setting */
		SoundManagerScript.Singleton.BGMute = BGMuteToggle.isOn;
		SoundManagerScript.Singleton.EfxMute = EfxMuteToggle.isOn;
		SoundManagerScript.Singleton.BGVolume = BGVolumeAdjuster.value;
		SoundManagerScript.Singleton.EfxVolume = EfxVolumeAdjuster.value;
	}
}
