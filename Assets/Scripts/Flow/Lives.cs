using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Flow {
	public class Lives : MonoBehaviour {

		[SerializeField] private Image[] lives;
		[SerializeField] private Sprite correctLive;
		[SerializeField] private Sprite wrongLive;
		
		public int CurrentLives { private set; get; }

		private void Awake() {
			ResetLives();
		}

		public void Wrong() {
			CurrentLives--;

			int timesHurt = lives.Length - CurrentLives;
			for (int i = 0; i < timesHurt; i++) {
				lives[i].sprite = wrongLive;
			}
		}

		public void ResetLives() {
			CurrentLives = lives.Length;

			foreach (Image life in lives) {
				life.sprite = correctLive;
			}
		}
	}
}