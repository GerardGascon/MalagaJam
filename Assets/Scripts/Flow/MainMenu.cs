using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flow {
	public class MainMenu : MonoBehaviour {
		private void Start() {
			AudioManager.instance.Play("Musica");
		}
		
		public void LoadGame() {
			SceneManager.LoadScene(sceneBuildIndex: 1);
		}
		
		public void QuitGame() {
			Application.Quit();
		}
	}
}