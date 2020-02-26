using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class LevelCompletePresenter : MonoBehaviour {
        LevelCompleteManager levelCompleteManager;

		private void Show (bool flag) {
			Logger.Log (this.name, "Set level complete - " + flag);
			this.gameObject.SetActive (flag);
		}        

		public void Repaint () {
			if (CoreGame.Instance.state.state == State.GameStateEnum.LEVELCOMPLETE && !this.gameObject.activeInHierarchy) {
				Show (true);
			} else if (CoreGame.Instance.state.state != State.GameStateEnum.LEVELCOMPLETE && this.gameObject.activeInHierarchy) {
				Show (false);
			}
		}

		public void Initialize (LevelCompleteManager levelCompleteManager) {
			this.levelCompleteManager = levelCompleteManager;
		}        

    }
}