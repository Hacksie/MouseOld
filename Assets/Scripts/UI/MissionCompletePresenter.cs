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
            if (GameManager.Instance.GameState.PlayState == PlayStateEnum.MissionComplete)
            {
                Show();
                missionTime.text = (Time.time - GameManager.Instance.GameState.CurrentLevel.startTime).ToString("0s");
                infoCollected.text = GameManager.Instance.GameState.CurrentLevel.infoCollected + "/" + GameManager.Instance.GameState.CurrentLevel.maxInfo;
                missionCredits.text = "$" + (GameManager.Instance.GameState.CurrentLevel.completeCredits + GameManager.Instance.GameState.CurrentLevel.creditsCollected);
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