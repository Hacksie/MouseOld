using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class MissionCompletePresenter : MonoBehaviour {
        MissionCompleteManager missionCompleteManager;
        UnityEngine.UI.Text missionTime;

		private void Show (bool flag) {
			Debug.Log ("Set mission complete " + flag);
			this.gameObject.SetActive (flag);
            missionTime.Text = (Time.time - CoreGame.Instance.State.currentLevel.startTime).ToString("{0s}");

		}        

		public void Repaint () {
			if (CoreGame.Instance.State.state == GameStateEnum.MISSIONCOMPLETE && !this.gameObject.activeInHierarchy) {
				Show (true);
			} else if (CoreGame.Instance.State.state != GameStateEnum.MISSIONCOMPLETE && this.gameObject.activeInHierarchy) {
				Show (false);
			}
		}

		public void Initialize (MissionCompleteManager missionCompleteManager) {
			this.missionCompleteManager = missionCompleteManager;
		}        

    }
}