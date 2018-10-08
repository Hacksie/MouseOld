using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class CoreGame : MonoBehaviour {

		public static CoreGame instance;

		[SerializeField]
		private GameState state = GameState.LOADING;

		private Input.IInputController inputController = new Input.DesktopInputController ();

		private GameObject player;
		private PlayerController playerController;
		private GameObject startMenuPanel;
		private GameObject selectMenuPanel;
		private GameObject taskPanel;
		private Dialogue.INarrationManager narrationManager;
		private Dialogue.NarrationPanelPresenter narrationPanel;

		public Story.StoryEvent startingStory;

		CoreGame () {
			instance = this;
		}

		// Use this for initialization
		void Start () {
			//FIXME: Make this work in editory only 
			if (SceneManager.GetActiveScene ().name == "Core" || SceneManager.GetActiveScene ().name == "IntroRoom") {
				Initialization ();
				SceneInitialize ();
			}
		}

		public void Initialization () {
			state = GameState.LOADING;
			Debug.Log ("Initialization");
			startMenuPanel = GameObject.FindWithTag (TagManager.STARTMENU);
			selectMenuPanel = GameObject.FindWithTag (TagManager.SELECTMENU);
			taskPanel = GameObject.FindWithTag (TagManager.TASK_PANEL);

			GameObject narrationManagerObj = GameObject.FindWithTag (TagManager.NARRATION_MANAGER);
			GameObject narrationPanelObj = GameObject.FindWithTag (TagManager.NARRATION_PANEL);

			narrationManager = narrationManagerObj.GetComponent<HackedDesign.Dialogue.NarrationManager> ();
			narrationPanel = narrationPanelObj.GetComponent<HackedDesign.Dialogue.NarrationPanelPresenter> ();

			narrationManager.Initialize (inputController);
			narrationPanel.Initialize (narrationManager);

			startMenuPanel.SetActive (false);
			selectMenuPanel.SetActive (false);
			taskPanel.SetActive (false);
		}

		/// <summary>
		/// Run this each time the scene is changed
		/// </summary>
		public void SceneInitialize () {
			Debug.Log ("Scene Initialization");
			player = GameObject.FindWithTag (TagManager.PLAYER);
			playerController = player.GetComponent<PlayerController> ();
			state = GameState.PLAYING;
			Cursor.visible = false;

			if (startingStory != null) {
				startingStory.Start ();
			} else {
				Debug.LogError ("No starting story set");
			}

		}

		public void ResumeEvent () {
			SetResume ();
		}

		public void QuitEvent () {
			//FIXME: Ask for save?
			SceneManager.LoadScene ("MainMenu");
		}

		// public void Pause () {
		// 	Time.timeScale = 0;
		// 	playingUI.SetActive (false);
		// 	pausedUI.SetActive (true);
		// 	state = GameState.PAUSE;
		// 	Cursor.visible = true;
		// }

		public void SetResume () {
			Debug.Log ("State set to RESUME");
			Time.timeScale = 1;
			HideStartMenu ();
			HideSelectMenu ();
			narrationPanel.Repaint ();
			//pausedUI.SetActive (false);
			//playingUI.SetActive (true);
			state = GameState.PLAYING;
			Cursor.visible = false;
		}

		public void SetDialogue () {
			Debug.Log ("State set to DIALOGUE");
			Time.timeScale = 0;
			//pausedUI.SetActive (false);
			//playingUI.SetActive (true);
			state = GameState.DIALOGUE;
			Cursor.visible = true;

		}

		public void SetNarration () {
			Debug.Log ("State set to NARRATIOn");
			Time.timeScale = 0;
			narrationPanel.Repaint ();
			state = GameState.NARRATION;
			Cursor.visible = true;
		}

		void Update () {

			switch (state) {

				case GameState.PLAYING:
					PlayingUpdate ();

					if (inputController.StartButtonUp ()) {
						ShowStartMenu ();
					}

					if (inputController.SelectButtonUp ()) {
						ShowSelectMenu ();
					}

					break;

				case GameState.SELECTMENU:
					if (inputController.SelectButtonUp ()) {
						state = GameState.PLAYING;
						HideSelectMenu ();
					}
					break;

				case GameState.STARTMENU:
					if (inputController.StartButtonUp ()) {
						state = GameState.PLAYING;
						HideStartMenu ();
					}
					break;
			}
		}

		void LateUpdate () {

			switch (state) {
				case GameState.NARRATION:
					//narrationPanel.Repaint ();
					break;
			}
		}

		void FixedUpdate () {
			switch (state) {
				case GameState.PLAYING:
					PlayingFixedUpdate ();
					break;
			}
		}

		void ShowStartMenu () {
			state = GameState.STARTMENU;
			startMenuPanel.SetActive (true);
			Cursor.visible = true;
		}

		void HideStartMenu () {
			startMenuPanel.SetActive (false);
			Cursor.visible = false;
		}

		void ShowSelectMenu () {
			state = GameState.SELECTMENU;
			selectMenuPanel.SetActive (true);
			Cursor.visible = true;
		}

		void HideSelectMenu () {
			selectMenuPanel.SetActive (false);
			Cursor.visible = false;
		}

		void PlayingUpdate () {
			playerController.UpdateMovement (inputController);
		}

		void PlayingFixedUpdate () {
			playerController.UpdateTransform ();
		}

	}

	public enum GameState {
		MAINMENU,
		CUTSCENE,
		PLAYING,
		LOADING,
		NARRATION,
		DIALOGUE,
		STARTMENU,
		SELECTMENU
	}
}