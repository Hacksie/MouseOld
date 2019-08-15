using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class WorldMapManager : MonoBehaviour {

		public List<Map.Sector> sectors = new List<Map.Sector> ();
		public Map.Sector selectedSector;
		public Map.Building selectedBuilding;
		public Map.Location selectedLocation;

		public void CancelEvent () {
			CoreGame.Instance.SetPlaying ();
		}

		public void LocationClickEvent () {
			GameObject selectedLocation = EventSystem.current.currentSelectedGameObject;
			Map.LocationBehaviour lb = selectedLocation.GetComponent<Map.LocationBehaviour> ();

			for(int i=0; i < SceneManager.sceneCount; i++)
			{

				if(SceneManager.GetSceneAt(i).isLoaded && SceneManager.GetSceneAt(i).name == lb.location.scene)
				{
					Debug.LogWarning("We can't travel to the scene we're already in");
					return;
				}
			}
		}

		public Map.Sector GetSelectedSector () {
			return selectedSector;
		}

		public void SetSelectedSector (Map.Sector sector) {
			selectedSector = sector;
			//sector.current = true;
		}

		public Map.Building GetSelectedBuilding () {
			return selectedBuilding;
		}

		public void SetSelectedBuilding (Map.Building building) {
			selectedBuilding = building;
			//sectors.Find (s => s.current).buildings.Find (b => b.current).current = false;
			//building.current = true;
		}

		public Map.Location GetSelectedLocation () {
			return selectedLocation;
		}

		public void SetSelectedLocation (Map.Location location) {
			selectedLocation = location;
			//sectors.Find (s => s.current).buildings.Find (b => b.current).locations.Find (l => l.current).current = false;
			//location.current = true;
		}

		public List<Map.Sector> GetSectors () {
			return sectors.FindAll (s => s != null);
		}

		public List<Map.Sector> GetActiveSectors () {
			return sectors.FindAll (s => s != null && s.available);
		}

		public List<Map.Building> GetBuildings (string sector) {
			return sectors.Find (s => s.title == sector).buildings;
		}
	}
}