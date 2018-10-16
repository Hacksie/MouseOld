using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign {
	public class WorldMapPanelPresenter : MonoBehaviour {

		WorldMapManager worldMapManager;

		//public GameObject sectorButtonPrefab;
		public Transform sectorButtonParent;
		public Transform buildingButtonParent;
		public Transform locationButtonParent;

		public Text descriptionText;

		public void Initialize (WorldMapManager worldMapManager) {
			this.worldMapManager = worldMapManager;
		}

		public void Repaint () {
			Debug.Log ("Repaint World map");

			if (CoreGame.instance.state != GameState.WORLDMAP) {
				Debug.Log ("Hiding world map");
				this.gameObject.SetActive (false);
				return;
			}

			this.gameObject.SetActive (true);
			Reset();

			RepaintSectors (worldMapManager.GetSectors ());

		}

		private void Reset()
		{
			EventSystem.current.SetSelectedGameObject (null);
			UpdateDescription("");
		}

		private void RepaintSectors (List<Map.Sector> sectors) {
			Debug.Log("Repaint sectors");

			if (sectorButtonParent == null) {
				Debug.LogWarning ("No sector button parent found");
				return;
			}

			RepaintBuildings (null);

			for (int i = 0; i < sectorButtonParent.childCount; i++) {
				Transform sbt = sectorButtonParent.GetChild (i);
				Button b = sbt.GetComponent<Button> ();
				Text t = sbt.GetComponentInChildren<Text> ();
				Map.SectorBehaviour sb = sbt.GetComponent<Map.SectorBehaviour> ();

				if (sectors.Count > i) {
					sbt.gameObject.SetActive (true); // Just in case it isn't active
					Map.Sector currentSector = sectors[i];

					sb.sector = currentSector;
					t.text = currentSector.title;

					if (currentSector.available) {
						b.interactable = true;
						t.color = Color.white;
					} else {
						b.interactable = false;
						t.color = Color.grey;
					}

					if (worldMapManager.GetSelectedSector() == currentSector) {
						// if (EventSystem.current.currentSelectedGameObject == null) {
						// 	EventSystem.current.SetSelectedGameObject (sbt.gameObject);
						// }

						RepaintBuildings (sectors[i].buildings);
					}
				} else {
					sbt.gameObject.SetActive (false);
				}
			}

		}

		private void RepaintBuildings (List<Map.Building> buildings) {
			Debug.Log("Repaint buildings");

			if (buildingButtonParent == null) {
				Debug.LogWarning ("No building button parent found");
				return;
			}

			RepaintLocations (null);

			for (int i = 0; i < buildingButtonParent.childCount; i++) {
				Transform bbt = buildingButtonParent.GetChild (i);

				if (buildings != null && buildings.Count > i) {
					bbt.gameObject.SetActive (true); // Just in case it isn't active
					Map.Building currentBuilding = buildings[i];

					Text t = bbt.GetComponentInChildren<Text> ();
					Map.BuildingBehaviour bb = bbt.GetComponent<Map.BuildingBehaviour> ();
					bb.building = currentBuilding;
					t.text = currentBuilding.title;
					if (worldMapManager.GetSelectedBuilding() == currentBuilding) {
						// if (EventSystem.current.currentSelectedGameObject == null) {
						// 	EventSystem.current.SetSelectedGameObject (bbt.gameObject);
						// }
						RepaintLocations (currentBuilding.locations);
					}
				} else {
					bbt.gameObject.SetActive (false);
				}
			}
		}

		private void RepaintLocations (List<Map.Location> locations) {
			Debug.Log("Repaint locations");

			if (locationButtonParent == null) {
				Debug.LogWarning ("No location button parent found");
				return;
			}

			for (int i = 0; i < locationButtonParent.childCount; i++) {
				Transform lbt = locationButtonParent.GetChild (i);

				if (locations != null && locations.Count > i) {
					lbt.gameObject.SetActive (true); // Just in case it isn't active

					Map.Location currentLocation = locations[i];

					Text t = lbt.GetComponentInChildren<Text> ();
					Map.LocationBehaviour lb = lbt.GetComponent<Map.LocationBehaviour> ();
					lb.location = currentLocation;
					t.text = currentLocation.title;
					if (worldMapManager.GetSelectedLocation() == currentLocation) {
						if (EventSystem.current.currentSelectedGameObject == null) {
							EventSystem.current.SetSelectedGameObject (lbt.gameObject);
						}
						//RepaintBuildings(buildings[i].buildings);
					}
				} else {
					lbt.gameObject.SetActive (false);
				}
			}
		}

		private void UpdateDescription (string text) {
			if (descriptionText == null) {
				Debug.LogWarning ("No world map description text object set");
				return;
			}

			descriptionText.text = text;
		}

		public void SelectSectorEvent () {
			Map.SectorBehaviour sb = EventSystem.current.currentSelectedGameObject.GetComponent<Map.SectorBehaviour> ();
			if (sb == null) {
				Debug.LogWarning ("No SectorBehaviour available");
				return;
			}
			worldMapManager.SetSelectedSector (sb.sector);
			RepaintBuildings (sb.sector.buildings);
			UpdateDescription (sb.sector.description);
		}

		public void SelectBuildingEvent () {
			//Debug.Log("Selected worldmap object " + EventSystem.current.currentSelectedGameObject.name);
			Map.BuildingBehaviour bb = EventSystem.current.currentSelectedGameObject.GetComponent<Map.BuildingBehaviour> ();
			if (bb == null) {
				Debug.LogWarning ("No BuildingBehaviour available");
				return;
			}

			worldMapManager.SetSelectedBuilding (bb.building);
			RepaintLocations (bb.building.locations);
			UpdateDescription (bb.building.description);
		}

		public void SelectLocationEvent () {
			//Debug.Log("Selected worldmap object " + EventSystem.current.currentSelectedGameObject.name);
			Map.LocationBehaviour lb = EventSystem.current.currentSelectedGameObject.GetComponent<Map.LocationBehaviour> ();
			if (lb == null) {
				Debug.LogWarning ("No BuildingBehaviour available");
				return;
			}

			worldMapManager.SetSelectedLocation (lb.location);
			UpdateDescription (lb.location.description);
		}

	}
}