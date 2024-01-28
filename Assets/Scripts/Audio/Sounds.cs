using UnityEngine;
using UnityEngine.Audio;

namespace Audio {
	[CreateAssetMenu(fileName = "Sounds", menuName = "Simple Tools/Sounds", order = 11)]
	public class Sounds : ScriptableObject {

		[Tooltip("The music mixer.")]
		public AudioMixerGroup musicMixer;
		[Tooltip("The SFX mixer.")]
		public AudioMixerGroup sfxMixer;

		public List[] sounds;

		[System.Serializable]
		public class List {
			[Tooltip("Name of the sound. Each name has to be different between each other.")]
			public string name;

			public AudioClip clip;

			[System.Serializable] public enum Type { Music, SFX }
			[Space]
			[Tooltip("Is it part of the music or the SFX?")] public Type type;

			[Space]
			[Tooltip("Default volume of the sound."), Range(0f, 1f)] public float volume;
			[Tooltip("Variance percentage of the volume"), Range(0f, 1f)] public float volumeVariance;
			[Tooltip("Default pitch of the sound."), Range(.1f, 3f)] public float pitch;
			[Tooltip("Variance percentage of the pitch"), Range(0f, 1f)] public float pitchVariance;

			public bool loop;

			[HideInInspector] public AudioSource source;

			private float _randomVolume;
			public float RandomVolume {
				get {
					_randomVolume = volume * (1f + Random.Range(-volumeVariance / 2f, volumeVariance / 2f));
					return _randomVolume;
				}
			}

			private float _randomPitch;
			public float RandomPitch {
				get {
					_randomPitch = pitch * (1f + Random.Range(-pitchVariance / 2f, pitchVariance / 2f));
					return _randomPitch;
				}
			}
		}
	}
}