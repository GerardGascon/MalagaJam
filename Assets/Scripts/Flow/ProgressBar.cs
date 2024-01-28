using System;
using UnityEngine;
using UnityEngine.UI;

namespace Flow {
	public class ProgressBar : MonoBehaviour {
		[SerializeField] private Image bar;

		private int _progressDone;
		[SerializeField, Min(0)] private int numberToWin = 10;
		
		private void Awake() {
			UpdateFill();
		}

		public void AddProgress() {
			_progressDone++;

			if (_progressDone <= numberToWin) {
				UpdateFill();
			}
		}

		private void UpdateFill() {
			float fillPercentage = _progressDone / (float)numberToWin;
			bar.fillAmount = fillPercentage;
		}
	}
}