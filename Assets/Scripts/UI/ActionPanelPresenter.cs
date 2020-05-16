using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    public class ActionPanelPresenter : MonoBehaviour
    {
        public Image batterySprite;
        public Text bugsCountText;
        private Vector2 batterySpriteMaxSize;
        [SerializeField] private Image dashCooldown = null;
        [SerializeField] private PlayerController playerController = null;

        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
            batterySpriteMaxSize = batterySprite.rectTransform.sizeDelta;
        }

        public void Repaint()
        {
            if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING)
            {
                if (!gameObject.activeInHierarchy)
                {
                    Show(true);
                }
                RepaintCounts();
            }
            else if (gameObject.activeInHierarchy)
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
            dashCooldown.fillAmount = 1 - playerController.DashPercentageComplete;
            //bugsCountText.text = CoreGame.Instance.state.player.bugs.ToString();
            batterySprite.rectTransform.sizeDelta = new Vector2(batterySpriteMaxSize.x * CoreGame.Instance.state.player.battery / CoreGame.Instance.state.player.maxBattery, batterySpriteMaxSize.y);
        }
    }
}