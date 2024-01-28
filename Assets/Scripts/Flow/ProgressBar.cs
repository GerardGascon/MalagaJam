using System;
using DG.Tweening;
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

		public bool AddProgress() {
			_progressDone++;

			if (_progressDone <= numberToWin) {
				UpdateFill();
			}

			if (_progressDone >= numberToWin) {
				FindObjectOfType<WinScreen>().Win();
				return true;
			}

			return false;
		}

		private void UpdateFill() {
			float fillPercentage = _progressDone / (float)numberToWin;
			bar.DOFillAmount(fillPercentage, .25f);
		}
	}
}