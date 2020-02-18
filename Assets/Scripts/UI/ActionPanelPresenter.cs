using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    public class ActionPanelPresenter : MonoBehaviour
    {
        public Image batterySprite;
        public Text bugsCountText;
        private Vector2 batterySpriteMaxSize;

        public void Initialize()
        {
            batterySpriteMaxSize = batterySprite.rectTransform.sizeDelta;
        }

        public void Repaint()
        {
            if (CoreGame.Instance.State.state == GameStateEnum.PLAYING)
            {
                if (!this.gameObject.activeInHierarchy)
                {
                    Show(true);
                }
                RepaintCounts();
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

        private void RepaintCounts()
        {
            bugsCountText.text = CoreGame.Instance.State.player.bugs.ToString();
            batterySprite.rectTransform.sizeDelta = new Vector2(batterySpriteMaxSize.x * CoreGame.Instance.State.player.battery / CoreGame.Instance.State.player.maxBattery, batterySpriteMaxSize.y);
        }
    }
}