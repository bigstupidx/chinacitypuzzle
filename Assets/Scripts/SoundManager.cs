using UnityEngine;
using System.Collections;

public enum BGM {
	Opening = 0, MainGame = 1, Result = 2, Credit = 3
}

public class SoundManager : MonoBehaviour {

	[System.Serializable] public class SoundVolume {
		public float BGM = 1.0f;
		public float Voice = 1.0f;
		public float SE = 1.0f;
		public bool Mute = false;

		public void Init() {
			BGM = 1.0f;
			Voice = 1.0f;
			SE = 1.0f;
			Mute = false;
		}
	}

	protected static SoundManager instance;

	public static SoundManager Instance {
		get {
			if ( instance == null ) {
				instance = (SoundManager)FindObjectOfType (typeof(SoundManager));

				if ( instance == null ) {
					Debug.LogError ("SoundManager Instance Error");
				}
			}

			return instance;

		}
	}

	public SoundVolume volume = new SoundVolume();

	// === Audio Sources ===
	// BGM
	AudioSource BGMSource;
	// SE
	AudioSource[] SESources = new AudioSource[16];
	// Voice
	AudioSource[] VoiceSources = new AudioSource[16];

	// === Audio Clip ===
	// BGM
	public AudioClip[] BGM;
	public AudioClip[] SE;
	public AudioClip[] Voice;



	void Awake() {
		GameObject[] obj = GameObject.FindGameObjectsWithTag ("SoundManager");
		if (obj.Length > 1) {
			// Delete If you already exist
			Destroy (gameObject);
		} else {
			// Do not discard in the scene transition
			DontDestroyOnLoad (gameObject);
		}

		// Add all AudioSource Components
		BGMSource = gameObject.AddComponent<AudioSource> ();
		// BGM loop on
		BGMSource.loop = true;

		// SE AudioSource
		for (int i = 0; i < SESources.Length; i++) {
			SESources [i] = gameObject.AddComponent<AudioSource> ();
		}

		// Voice AndioSource
		for (int i = 0; i < VoiceSources.Length; i++) {
			VoiceSources [i] = gameObject.AddComponent<AudioSource> ();
		}
	}

	void Update() {
		// To mute
		BGMSource.mute = volume.Mute;
		foreach (AudioSource source in SESources) {
			source.mute = volume.Mute;
		}

		foreach (AudioSource source in VoiceSources) {
			source.mute = volume.Mute;
		}

		// Volume settings
		BGMSource.volume = volume.BGM;
		foreach (AudioSource source in SESources) {
			source.volume = volume.SE;
		}

		foreach (AudioSource source in VoiceSources) {
			source.volume = volume.Voice;
		}
	}

	/// <summary>
	/// Play BGM
	/// </summary>
	/// <param name="index">Index.</param>
	public void PlayBGM(int index) {
		if ( 0 > index || BGM == null || BGM.Length <= index) {
			return;
		}

		// Do not do anything when the same BGM
		if (BGMSource.clip == BGM[index]){
			return;
		}

		BGMSource.Stop();
		BGMSource.clip = BGM[index];
		BGMSource.Play();
	}


	/// <summary>
	/// Stop BGM
	/// </summary>
	public void StopBGM() {
		BGMSource.Stop ();
		BGMSource.clip = null;
	}


	/// <summary>
	/// Play SE
	/// </summary>
	/// <param name="index">Index.</param>
	public void PlaySE(int index) {
		if (0 > index || SE == null || SE.Length <= index) {
			return;
		}

		// Playing in AudioSource not playing
		foreach (AudioSource source in SESources) {
			if (source.isPlaying == false) {
				source.clip = SE [index];
				source.Play ();
				return;
			}
		}
	}

	/// <summary>
	/// STOP SE
	/// </summary>
	public void StopSE() {
		// To stop the AudioSource for all of SE
		foreach (AudioSource source in SESources) {
			source.Stop ();
			source.clip = null;
		}
	}


	/// <summary>
	/// Play voice
	/// </summary>
	/// <param name="index">Index.</param>
	public void PlayVoice(int index) {
		if (0 > index || Voice.Length <= index) {
			return;
		}

		// Playing in AudioSource not playing
		foreach (AudioSource source in VoiceSources) {
			if (source.isPlaying == false) {
				source.clip = Voice [index];
				source.Play ();
				return;
			}
		}
	}

	/// <summary>
	/// Stop voice
	/// </summary>
	public void StopVoice() {
		// To stop the AudioSource for all of Voice
		foreach (AudioSource source in VoiceSources) {
			source.Stop ();
			source.clip = null;
		}
	}
}


