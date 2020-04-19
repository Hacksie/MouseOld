using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    public class StatsPresenter : MonoBehaviour
    {
        [SerializeField]
        private bool show = true;
        [SerializeField]
        private Text statsText = null;

        float deltaTime = 0.0f;
        
        public void Initialize()
        {
        }

        public void Repaint()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING)
            {
                if (!this.gameObject.activeInHierarchy)
                {
                    Show(true && show);
                }

                if (!show) return;

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