using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HackedDesign {
	public class WorldMapPanelPresenter : MonoBehaviour {

		WorldMapManager worldMapManager;

		public GameObject sectorButtonPrefab;
		public Transform sectorButtonParent;

		public void Initialize (WorldMapManager worldMapManager) {
			this.worldMapManager = worldMapManager;
		}

		public void Repaint () {
			Debug.Log ("Repaint World map");

			if (CoreGame.instance.state != GameState.WORLDMAP) {
				Debug.Log("Hiding world map");
				this.gameObject.SetActive (false);
				return;
			}

			this.gameObject.SetActive (true);
			RepaintSectors ();

		}

		public void RepaintSectors () {
			List<Map.Sector> results = worldMapManager.GetSectors ();

			if (sectorButtonParent == null) {
				Debug.LogWarning ("No sector button parent found");
				return;
			}

			for(int i = 0; i < sectorButtonParent.childCount; i++)
			{
				Destroy (sectorButtonParent.GetChild(i).gameObject);
			}

			for(int j = 0; j < results.Count; j++)
			{
				//Transform sb = sectorButtonParent.Find(results[j].name);
				GameObject s = GameObject.Instantiate(sectorButtonPrefab);
				Text t = s.GetComponentInChildren<Text>();
				t.text = results[j].title;
				s.transform.SetParent(sectorButtonParent);
				if(results[j].current)
				{
					EventSystem.current.SetSelectedGameObject(s);
				}
			}
		}

		public void RepaintBuildings() {
			
		}

	}

}