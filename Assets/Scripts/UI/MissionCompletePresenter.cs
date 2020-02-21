using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class MissionCompletePresenter : MonoBehaviour {
        MissionCompleteManager missionCompleteManager;
        public UnityEngine.UI.Text missionTime;
		public UnityEngine.UI.Text infoCollected;
		public UnityEngine.UI.Text missionCredits;

		private void Show (bool flag) {
			Debug.Log ("Set mission complete " + flag);
			this.gameObject.SetActive (flag);
            missionTime.text = (Time.time - CoreGame.Instance.state.currentLevel.startTime).ToString("0s");
			infoCollected.text = CoreGame.Instance.state.currentLevel.infoCollected + "/" + CoreGame.Instance.state.currentLevel.maxInfo;
			missionCredits.text = "$" + (CoreGame.Instance.state.currentLevel.completeCredits + CoreGame.Instance.state.currentLevel.creditsCollected);


		}        

		public void Repaint () {
			if (CoreGame.Instance.state.state == State.GameStateEnum.MISSIONCOMPLETE && !this.gameObject.activeInHierarchy) {
				Show (true);
			} else if (CoreGame.Instance.state.state != State.GameStateEnum.MISSIONCOMPLETE && this.gameObject.activeInHierarchy) {
				Show (false);
			}
		}

		public void Initialize (MissionCompleteManager missionCompleteManager) {
			this.missionCompleteManager = missionCompleteManager;
		}        

    }
}