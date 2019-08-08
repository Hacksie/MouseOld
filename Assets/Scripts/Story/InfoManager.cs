using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		public class InfoManager : MonoBehaviour {

			public InfoManager instance;



			[Header("Config")]
			public List<InfoCategory> categories = new List<InfoCategory> ();
			public List<InfoEntity> entities = new List<InfoEntity>();



			public string selectedInfoCategory;
			public string selectedInfoEntity;

			public List<InfoCategory> GetCategories () {
				return categories;
			}

		}
	}
}