using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	public class WorldMapManager : MonoBehaviour {

		public List<Map.Sector> sectors = new List<Map.Sector> ();

		public void CancelEvent () {
			CoreGame.instance.SetResume ();
		}

		public void SectorSelectEvent () {

		}

		public void SectorClickEvent () {

		}

		public List<Map.Sector> GetSectors () {
			List<Map.Sector> results = new List<Map.Sector> ();

			for (int i = 0; i < sectors.Count; i++) {

				if (sectors[i] != null) {
					results.Add (sectors[i]);
				}
			}
			return results;

		}		

		public List<Map.Sector> GetActiveSectors () {
			List<Map.Sector> results = new List<Map.Sector> ();

			for (int i = 0; i < sectors.Count; i++) {

				if (sectors[i] != null && sectors[i].available) {
					results.Add (sectors[i]);
				}
			}
			return results;

		}

	}
}