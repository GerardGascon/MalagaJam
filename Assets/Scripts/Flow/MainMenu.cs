using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flow {
	public class MainMenu : MonoBehaviour {
		public void LoadGame() {
			SceneManager.LoadScene(sceneBuildIndex: 1);
		}
	}
}