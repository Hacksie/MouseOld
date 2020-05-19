using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign.UI
{
    public class TimerPanelPresenter : AbstractPresenter
    {
        private Timer timer;
        [SerializeField] private Text timerText = null;
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

            if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING)
            {
                Show();

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
            }
            else
            {
                Hide();
            }
        }
    }
}