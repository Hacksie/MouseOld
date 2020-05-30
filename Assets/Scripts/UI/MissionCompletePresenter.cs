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
            missionTime.text = (Time.time - GameManager.Instance.Data.CurrentLevel.startTime).ToString("0s");
            infoCollected.text = GameManager.Instance.Data.CurrentLevel.infoCollected + "/" + GameManager.Instance.Data.CurrentLevel.maxInfo;
            missionCredits.text = "$" + (GameManager.Instance.Data.CurrentLevel.completeCredits + GameManager.Instance.Data.CurrentLevel.creditsCollected);

        }

        public void Initialize(MissionCompleteManager missionCompleteManager)
        {
            this.missionCompleteManager = missionCompleteManager;
        }

    }
}