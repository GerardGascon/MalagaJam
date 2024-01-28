using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flow {
	public class MainMenu : MonoBehaviour {
		private void Start() {
			AudioManager.instance.Play("Menu");
		}
		
		public void LoadGame() {
			AudioManager.instance.FadeOut("Menu", 1f);
			AudioManager.instance.FadeIn("Musica", 1f);
			SceneManager.LoadScene(sceneBuildIndex: 1);
		}
		
		public void QuitGame() {
			Application.Quit();
		}
	}
}