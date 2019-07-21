using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace HackedDesign {
	public class MainMenu : MonoBehaviour {

		public string newGameScene = "IntroRoom";

		public GameObject optionsPanel;
		public GameObject creditsPanel;
		public UnityEngine.UI.Dropdown resolutions;
		public UnityEngine.UI.Toggle windowToggle;
		public UnityEngine.UI.Slider masterSlider;
		public UnityEngine.UI.Slider ambientSlider;
		public UnityEngine.UI.Slider musicSlider;
		public UnityEngine.UI.Slider fxSlider;
		public UnityEngine.Audio.AudioMixer masterMixer;

		public void Start() {
			ShowOptionsPanel(false);
			ShowCreditsPanel(false);
			PopulateResolutions();
			PopulateAudioSliders();
		}

		public void PopulateResolutions()
		{
			resolutions.ClearOptions();
			resolutions.AddOptions(Screen.resolutions.ToList().ConvertAll(r => new UnityEngine.UI.Dropdown.OptionData(r.ToString())));

			resolutions.value = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);
		}

		public void SetResolution()
		{
			Resolution res = Screen.resolutions.ToList()[resolutions.value];
			Screen.SetResolution(res.width, res.height, windowToggle.isOn, res.refreshRate);
		}

		public void PopulateAudioSliders()
		{
			float masterVolume;
			float ambientVolume;
			float fxVolume;
			float musicVolume;
			masterMixer.GetFloat("MasterVolume", out masterVolume);
			masterMixer.GetFloat("AmbientVolume", out ambientVolume);
			masterMixer.GetFloat("FXVolume", out fxVolume);
			masterMixer.GetFloat("MusicVolume", out musicVolume);

			masterSlider.value = (masterVolume + 80) / 100;
			ambientSlider.value = (ambientVolume + 80) / 100;
			fxSlider.value = (fxVolume + 80) / 100;
			musicSlider.value = (musicVolume + 80) / 100;
			//Debug.Log(masterVolume);
			
		}

		public void ShowOptionsPanel(bool show)
		{
			if(optionsPanel != null)
			{
				optionsPanel.SetActive(show);
			}
		}

		public void ShowCreditsPanel(bool show) 
		{
			if(creditsPanel != null) 
			{
				creditsPanel.SetActive(show);
			}
		}


		public void ContinueEvent () {
			Debug.Log ("Continue Event");
			ShowCreditsPanel(false);
			ShowOptionsPanel(false);			
		}

		public void NewGameEvent () {
			Debug.Log ("New Game Event");
			ShowCreditsPanel(false);
			ShowOptionsPanel(false);
			CoreGame.instance.LoadNewGame();
			//StartCoroutine (LoadNewGameScenes ( "IntroRoom", "IntroRoom"));
		}

		public void OptionsEvent () {
			Debug.Log ("Options Event");
			ShowCreditsPanel(false);
			ShowOptionsPanel(true);
		}

		public void CreditsEvent () {
			Debug.Log ("Credits Event");
			ShowCreditsPanel(true);
			ShowOptionsPanel(false);			
		}

		public void QuitEvent () {
			Debug.Log ("Quit Event");
			Application.Quit ();
		}

		public void HideMainMenu()
		{
			gameObject.SetActive(false);
		}	

		public void ShowMainMenu()
		{
			Time.timeScale = 0;
			gameObject.SetActive(true);
		}			


		// IEnumerator LoadNewGameScenes (string levelName, string levelGenTemplate) {
		// 	Debug.Log ("Loading new game scenes");

		// 	AsyncOperation asyncLoadBaseScene = SceneManager.LoadSceneAsync ("Core", LoadSceneMode.Additive);
		// 	asyncLoadBaseScene.allowSceneActivation = false;

		// 	yield return null;

		// 	//Wait until we are done loading the scene
		// 	while (asyncLoadBaseScene.progress < 0.9f) {
		// 		Debug.Log ("Loading scene #:" + "Core" + " [][] Progress: " + asyncLoadBaseScene.progress);
		// 		yield return null;
		// 	}

		// 	Debug.Log ("Core ready");

		// 	// AsyncOperation asyncLoadRubyScene = SceneManager.LoadSceneAsync (newGameScene, LoadSceneMode.Additive);
		// 	// asyncLoadRubyScene.allowSceneActivation = false;

		// 	// yield return null;

		// 	// //Wait until we are done loading the scene
		// 	// while (asyncLoadRubyScene.progress < 0.9f) {
		// 	// 	Debug.Log ("Loading scene #:" + newGameScene + " [][] Progress: " + asyncLoadRubyScene.progress);
		// 	// 	yield return null;
		// 	// }

		// 	// Debug.Log (newGameScene +" ready");

		// 	asyncLoadBaseScene.allowSceneActivation = true;
		// 	//asyncLoadRubyScene.allowSceneActivation = true;

			

		// 	while (!asyncLoadBaseScene.isDone ) {
		// 		Debug.Log ("Activating scenes");
		// 		yield return null;
		// 	}

		// 	SceneManager.SetActiveScene (SceneManager.GetSceneByName ("Core"));

		// 	SceneManager.UnloadScene ("MainMenu");
			
		// 	CoreGame.instance.Initialization ();
		// 	//CoreGame.instance.SceneInitialize ("Jennifer's Room", "Jennifer's Room");
		// 	CoreGame.instance.SceneInitialize ("Easy Magenta", "Easy Magenta");

		// }
	}
}

