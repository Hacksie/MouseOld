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
            if (GameManager.Instance.state.state == GameStateEnum.MISSIONCOMPLETE)
            {
                Show();
                missionTime.text = (Time.time - GameManager.Instance.state.currentLevel.startTime).ToString("0s");
                infoCollected.text = GameManager.Instance.state.currentLevel.infoCollected + "/" + GameManager.Instance.state.currentLevel.maxInfo;
                missionCredits.text = "$" + (GameManager.Instance.state.currentLevel.completeCredits + GameManager.Instance.state.currentLevel.creditsCollected);
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