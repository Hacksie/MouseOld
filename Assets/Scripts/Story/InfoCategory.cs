using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[System.Serializable]
		[CreateAssetMenu(fileName = "Category", menuName = "Mouse/Content/Info Category")]
		public class InfoCategory : ScriptableObject {
			public string id;
			public string text;
			public bool available = true;
			public string lookupList;
		}
	}
}