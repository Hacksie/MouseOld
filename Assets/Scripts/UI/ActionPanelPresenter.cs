using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign.UI
{
    public class ActionPanelPresenter : AbstractPresenter
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

        public override void Repaint()
        {
            RepaintBatteryBar();
            // if (GameManager.Instance.GameState.IsPlaying())
            // {
            //     Show();
            //     RepaintCounts();
            // }
            // else
            // {
            //     Hide();
            // }
        }

        private void RepaintBatteryBar()
        {
            batterySprite.rectTransform.sizeDelta = new Vector2(batterySpriteMaxSize.x * GameManager.Instance.Data.Player.battery / GameManager.Instance.Data.Player.maxBattery, batterySpriteMaxSize.y);
        }

        private void RepaintCounts()
        {
            dashCooldown.fillAmount = 1 - playerController.DashPercentageComplete;
            //bugsCountText.text = CoreGame.Instance.state.player.bugs.ToString();
            
        }
    }
}