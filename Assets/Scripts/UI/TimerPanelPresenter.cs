using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign.UI
{
    public class TimerPanelPresenter : AbstractPresenter
    {
        private Timer timer;
        [SerializeField] private Text timerText = null;
        [SerializeField] private Text alertLevelText = null;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color warningColor = Color.white;
        [SerializeField] private Color alertColor = Color.white;

        public void Initialize(Timer timer)
        {
            this.timer = timer;
        }

        public override void Repaint()
        {
            if (timer == null)
            {
                Hide();
                return;
            }

            if (alertLevelText != null)
            {
                alertLevelText.text = GameManager.Instance.GameState.CurrentLevel.alertLevel.ToString();
            }

            if (timer.running)
            {

                float time = timer.maxTime - (Time.time - timer.startTime);

                if (time < timer.alertTime)
                {
                    timerText.color = alertColor;
                }
                else if (time < timer.warningTime)
                {
                    timerText.color = warningColor;
                }
                else
                {
                    timerText.color = defaultColor;
                }

                timerText.text = time.ToString("000");
            }

            // if (GameManager.Instance.GameState.PlayState == PlayStateEnum.Playing)
            // {
            //     Show();


            // }
            // else
            // {
            //     Hide();
            // }
        }
    }
}