using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class WorldMapManager : MonoBehaviour 
	{
		
        [SerializeField] private string defaultLocation = "AisanaContractTower2";
        public string selectedLocation;

        public void Initialize() {
            selectedLocation = defaultLocation;
        }
    }
}

