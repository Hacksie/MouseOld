using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class MissionCompletePresenter : AbstractPresenter
    {
        MissionCompleteManager missionCompleteManager;
        public UnityEngine.UI.Text missionTime;
        public UnityEngine.UI.Text infoCollected;
        public UnityEngine.UI.Text missionCredits;

         public override void Repaint()
        {
            if (CoreGame.Instance.state.state == GameState.GameStateEnum.MISSIONCOMPLETE)
            {
                Show();
                missionTime.text = (Time.time - CoreGame.Instance.state.currentLevel.startTime).ToString("0s");
                infoCollected.text = CoreGame.Instance.state.currentLevel.infoCollected + "/" + CoreGame.Instance.state.currentLevel.maxInfo;
                missionCredits.text = "$" + (CoreGame.Instance.state.currentLevel.completeCredits + CoreGame.Instance.state.currentLevel.creditsCollected);
            }
            else
            {
                Hide();
            }
        }

        public void Initialize(MissionCompleteManager missionCompleteManager)
        {
            this.missionCompleteManager = missionCompleteManager;
        }

    }
}