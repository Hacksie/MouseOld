using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    public class ActionPanelPresenter : MonoBehaviour
    {
        public Text batteryCountText;
        public Text keycardCountText;
        public Text bugsCountText;

        public void Initialize()
        {

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
            keycardCountText.text = "" + CoreGame.Instance.State.player.keycards;
            batteryCountText.text = "" + CoreGame.Instance.State.player.battery;
            bugsCountText.text = "" + CoreGame.Instance.State.player.bugs;
        }
    }
}