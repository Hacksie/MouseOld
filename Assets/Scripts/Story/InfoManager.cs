using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		public class InfoManager : MonoBehaviour {

			public static InfoManager instance;



			[Header("Config")]
			public List<InfoCategory> categories = new List<InfoCategory> ();
			public List<InfoEntity> entities = new List<InfoEntity>();

			[Header("State")]
			public List<InfoEntity> knownEntities = new List<InfoEntity>();


			public string selectedInfoCategory;
			public string selectedInfoEntity;

			public InfoManager()
			{
				instance = this;
			}

			public List<InfoCategory> GetCategories () {
				return categories;
			}

			public void AddToKnownEntities(InfoEntity entity) {
				if(!knownEntities.Contains(entity))
				{
					Debug.Log(this.name + ": adding entity " + entity.name + " to known entities");
					knownEntities.Add(entity);
					ActionManager.instance.AddActionMessage("Added '" + entity.name + "' entry to AI");
				}
			}

		}
	}
}