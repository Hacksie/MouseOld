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
            missionTime.text = (Time.time - CoreGame.Instance.State.currentLevel.startTime).ToString("0s");
			infoCollected.text = CoreGame.Instance.State.currentLevel.infoCollected + "/" + CoreGame.Instance.State.currentLevel.maxInfo;
			missionCredits.text = "$" + (CoreGame.Instance.State.currentLevel.completeCredits + CoreGame.Instance.State.currentLevel.creditsCollected);


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