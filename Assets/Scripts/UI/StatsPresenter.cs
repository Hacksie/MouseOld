using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    public class StatsPresenter : MonoBehaviour
    {
        float deltaTime = 0.0f;
        bool show = true;
        public Text statsText;

        public void Initialize()
        {


        }

        public void Repaint()
        {
            if (!show) return;

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            if (CoreGame.Instance.State.state == GameStateEnum.PLAYING)
            {
                if (!this.gameObject.activeInHierarchy)
                {
                    Show(true);
                }

                float msec = deltaTime * 1000.0f;
                float fps = 1.0f / deltaTime;
                var player = CoreGame.Instance.GetPlayer();
                statsText.text = string.Format("{0:####}, {1:###}, {2:###.##} {3:###.##}", fps, msec, player.gameObject.transform.position.x, player.gameObject.transform.position.y);
            }
            else if (this.gameObject.activeInHierarchy)
            {
                Show(false);
            }
        }

        private void Show(bool flag)
        {
            this.gameObject.SetActive(flag);
        }
    }
}