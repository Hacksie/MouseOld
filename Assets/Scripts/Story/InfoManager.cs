using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		public class InfoManager : MonoBehaviour {

			public List<InfoCategory> categories = new List<InfoCategory> ();
			public InfoCategory selectedCategory;
			//public Map.Building selectedBuilding;
			//public Map.Location selectedLocation;

			public List<InfoCategory> GetCategories()
			{
				return categories;
			}

		}
	}
}