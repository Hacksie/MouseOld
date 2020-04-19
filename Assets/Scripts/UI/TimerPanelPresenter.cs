using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
	public class TimerPanelPresenter : MonoBehaviour {

		private Timer timer;
		public Text timerText;
		public Color defaultColor;
		public Color warningColor;
		public Color alertColor;		

		public void Initialize (Timer timer) {
			this.timer = timer;
		}

		public void Repaint () {

			if (timer == null) {
				gameObject.SetActive (false);
				return;
			}

			if (!timer.running) {
				gameObject.SetActive (false);
				return;
			}

			if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING) {

				if (timer.running) {
					if (!gameObject.activeInHierarchy) {
						gameObject.SetActive (true);
					}

					float time = timer.maxTime - (Time.time - timer.startTime);

					if (time < timer.alertTime) {
						timerText.color = alertColor;
					} else if (time < timer.warningTime) {
						timerText.color = warningColor;
					} else {
						timerText.color = defaultColor;
					}

					timerText.text = time.ToString ("000");
				}
			}
			else {
				gameObject.SetActive(false);
			}
		}
	}
}