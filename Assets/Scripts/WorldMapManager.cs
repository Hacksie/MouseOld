using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class WorldMapManager : MonoBehaviour 
	{
        [SerializeField] private string defaultLocation = "AisanaContractTower2";
        public string selectedLocation;
        public string selectedFloor;

        public void Initialize() {
            selectedLocation = defaultLocation;
        }

        public void NextLevel()
        {
            // This should just set a new story state, and that stage should load the level accordingly;
            Story.ActionManager.Instance.CurrentScene.Next();
        }
    }
}

