using UnityEngine;

namespace HackedDesign.UI
{
    public class FloorListItem : AbstractPresenter
    {
        [Header("Reference GameObjects")]
        [SerializeField] private UnityEngine.UI.Text label = null;

        public Story.Floor floor;
        private WorldMapManager worldMapManager;

        public void Initialize(WorldMapManager worldMapManager)
        {
            this.worldMapManager = worldMapManager;

        }

        public override void Repaint()
        {
            if (floor != null)
            {
                label.text = floor.name;
            }
            else
            {
                label.text = "<invalid>";
                Logger.LogError(this, "no task set");
            }
        }

        public void Click()
        {
            Logger.Log(this, "Floor List Item clicked");
            worldMapManager.selectedFloor = this.floor.id;
        }
    }
}