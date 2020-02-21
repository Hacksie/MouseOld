using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class MainMenuPresenter : MonoBehaviour {

		public GameObject optionsPanel;
		public GameObject creditsPanel;
		public GameObject randomPanel;
		public UnityEngine.UI.Dropdown resolutionsDropdown;
		public UnityEngine.UI.Toggle fullScreenToggle;
		public UnityEngine.UI.Slider masterSlider;
		public UnityEngine.UI.Slider ambientSlider;
		public UnityEngine.UI.Slider musicSlider;
		public UnityEngine.UI.Slider fxSlider;
		public UnityEngine.Audio.AudioMixer masterMixer;
		public UnityEngine.UI.InputField seedInput;
		public UnityEngine.UI.Dropdown templateDropdown;
		public UnityEngine.UI.Slider lengthSlider;
		public UnityEngine.UI.Slider heightSlider;
		public UnityEngine.UI.Slider widthSlider;
		public UnityEngine.UI.Dropdown difficultyDropdown;
		public UnityEngine.UI.Slider enemiesSlider;
		public UnityEngine.UI.Slider camerasSlider;

		public Level.LevelGenTemplate[] templates;

		public void Start () {
			ShowOptionsPanel (false);
			ShowCreditsPanel (false);
			ShowRandomPanel (false);
			PopulateResolutions ();
			PopulateAudioSliders ();
			PopulateCorpTemplates();
		}

		public void Repaint()
		{
			if(CoreGame.Instance.state.state == State.GameStateEnum.MAINMENU)
			{
				if(!this.gameObject.activeInHierarchy) {
					this.gameObject.SetActive(true);
					//Cursor.visible = true;
				}
			} else
			{
				if (this.gameObject.activeInHierarchy)
				{
					this.gameObject.SetActive(false);
				}
				//Cursor.visible = false;
			}

		}

		public void ShowOptionsPanel (bool show) {
			if (optionsPanel != null) {
				optionsPanel.SetActive (show);
			}
		}

		public void ShowCreditsPanel (bool show) {
			if (creditsPanel != null) {
				creditsPanel.SetActive (show);
			}
		}

		public void ShowRandomPanel (bool show) {
			if (randomPanel != null) {
				randomPanel.SetActive (show);
			}
		}

		public void ContinueEvent () {
			Debug.Log (this.name + ": Continue Event");
			ShowCreditsPanel (false);
			ShowOptionsPanel (false);
			ShowRandomPanel (false);

		}

		public void NewGameEvent () {
			Debug.Log (this.name + ": New Game Event");
			ShowCreditsPanel (false);
			ShowOptionsPanel (false);
			ShowRandomPanel (false);
			CoreGame.Instance.LoadNewGame ();
			//StartCoroutine (LoadNewGameScenes ( "IntroRoom", "IntroRoom"));
		}

		public void RandomGameEvent () {
			Debug.Log (this.name + ": Random Game Event");
			UnityEngine.Random.InitState (UnityEngine.Random.seed);
			seedInput.text = UnityEngine.Random.seed.ToString ();
			ShowCreditsPanel (false);
			ShowOptionsPanel (false);
			ShowRandomPanel (true);
		}

		public void StartRandomGameEvent () {
			Debug.Log (this.name + ": Start Random Game Event");
			UnityEngine.Random.InitState (System.Convert.ToInt32 (seedInput.text));
			ShowCreditsPanel (false);
			ShowOptionsPanel (false);
			ShowRandomPanel (false);
			Debug.Log (this.name + ": " + templateDropdown.options[templateDropdown.value].text);
			CoreGame.Instance.LoadRandomGame (templateDropdown.options[templateDropdown.value].text, (int) lengthSlider.value, (int) heightSlider.value, (int) widthSlider.value, difficultyDropdown.value, (int) enemiesSlider.value, (int) camerasSlider.value);		}

		public void OptionsEvent () {
			Debug.Log (this.name + ": Options Event");
			ShowCreditsPanel (false);
			ShowOptionsPanel (true);
			ShowRandomPanel (false);
		}

		public void CreditsEvent () {
			Debug.Log (this.name + ": Credits Event");
			ShowCreditsPanel (true);
			ShowOptionsPanel (false);
			ShowRandomPanel (false);
		}

		public void QuitEvent () {
			Debug.Log (this.name + ": Quit Event");
			Application.Quit ();
		}

		// public void HideMainMenu () {
		// 	gameObject.SetActive (false);
		// }

		// public void ShowMainMenu () {
		// 	Time.timeScale = 0;
		// 	gameObject.SetActive (true);
		// }

		private void PopulateCorpTemplates() {
			templateDropdown.ClearOptions();
			templateDropdown.AddOptions(templates.ToList().ConvertAll(t => new UnityEngine.UI.Dropdown.OptionData(t.name.ToString())));
		}

		private void PopulateResolutions () {
			resolutionsDropdown.ClearOptions ();
			resolutionsDropdown.AddOptions (Screen.resolutions.ToList ().ConvertAll (r => new UnityEngine.UI.Dropdown.OptionData (r.ToString ())));
			resolutionsDropdown.value = Screen.resolutions.ToList ().IndexOf (Screen.currentResolution);
			fullScreenToggle.isOn = Screen.fullScreen;	
		}

		private void SetResolution () {
			Resolution res = Screen.resolutions.ToList () [resolutionsDropdown.value];
			Screen.SetResolution (res.width, res.height, fullScreenToggle.isOn, res.refreshRate);
		}

		private void PopulateAudioSliders () {
			float masterVolume;
			float ambientVolume;
			float fxVolume;
			float musicVolume;
			masterMixer.GetFloat ("MasterVolume", out masterVolume);
			masterMixer.GetFloat ("AmbientVolume", out ambientVolume);
			masterMixer.GetFloat ("FXVolume", out fxVolume);
			masterMixer.GetFloat ("MusicVolume", out musicVolume);
			masterSlider.value = (masterVolume + 80) / 100;
			ambientSlider.value = (ambientVolume + 80) / 100;
			fxSlider.value = (fxVolume + 80) / 100;
			musicSlider.value = (musicVolume + 80) / 100;
			//Debug.Log(masterVolume);

		}
	}
}