using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
namespace HackedDesign.UI
{
    public class WorldMapPresenter : AbstractPresenter
    {
        [SerializeField] private Text locationTitle = null;
        [SerializeField] private Text locationDescription = null;
        [SerializeField] List<Button> locationButtons = null;
        [SerializeField] private Transform floorListParent = null;
        [SerializeField] private GameObject floorListItemPrefab = null;

        private WorldMapManager worldMapManager = null;
        private Story.SceneManager sceneManager = null;


        public void Initialize(WorldMapManager worldMapManager, Story.SceneManager sceneManager)
        {
            this.worldMapManager = worldMapManager;
            this.sceneManager = sceneManager;
        }

        public override void Repaint()
        {
            RepaintLocations();
        }

        public void RepaintLocations()
        {
            Logger.Log(this, "Repaint Locations");
            
            var knownLocations = sceneManager.GetKnownLocations(); // FIXME: Move this to scenemanager

            foreach (var button in locationButtons.Where(b => b != null))
            {
                var desc = button.GetComponent<InfoEntityDescriptor>();
                if (desc != null)
                {
                    button.interactable = knownLocations.Any(l => l == desc.id);

                    if (desc.id == worldMapManager.selectedLocation)
                    {
                        EventSystem.current.SetSelectedGameObject(button.gameObject);
                    }

                }
                else
                {
                    button.interactable = false;
                }
            }

            RepaintDescription();
        }

        public void SelectLocation()
        {
            var selectedButton = EventSystem.current.currentSelectedGameObject;
            var desc = selectedButton.GetComponent<InfoEntityDescriptor>();
            worldMapManager.selectedLocation = desc.id;
            RepaintDescription();
        }

        private void RepaintDescription()
        {
            var locationId = worldMapManager.selectedLocation;

            if (locationId != null)
            {
                var entity = Story.InfoRepository.Instance.GetLocation(locationId);
                locationDescription.text = entity?.description;
                locationTitle.text =entity?.name;
            }
            else
            {
                locationTitle.text = "Unknown location";
                locationDescription.text = "";
            }

            RepaintFloors();

        }

        private void RepaintFloors()
        {
            Logger.Log(this, "Floors location ", worldMapManager.selectedLocation);
            
            var floors = this.sceneManager.GetFloorsForLocation(worldMapManager.selectedLocation);

            for (int i = 0; i < floorListParent.childCount; i++)
            {
                Destroy(floorListParent.GetChild(i).gameObject);
            }

            foreach (var floor in floors)
            {
                var item = Instantiate(floorListItemPrefab, floorListParent);
                var text = item.GetComponentInChildren<UnityEngine.UI.Text>();
                text.text = floor.name;
                var floorItem = item.GetComponent<FloorListItem>();
                floorItem.Initialize(this.worldMapManager);
                floorItem.floor = floor;
            }
        }

        public void NextLevel()
        {
            worldMapManager.NextLevel();   

        }
    }
}