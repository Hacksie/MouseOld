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
			taskPanel = GameObject.FindWithTag(TagManager.TASK_PANEL);

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
		}

		public void ResumeEvent () {
			state = GameState.PLAYING;
			HideStartMenu();
			HideSelectMenu();
		}

		public void QuitEvent () {
			//FIXME: Ask for save?
			SceneManager.LoadScene ("MainMenu");
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
		}

		void HideStartMenu() {
			startMenuPanel.SetActive (false);
		}



		void ShowSelectMenu () {
			state = GameState.SELECTMENU;
			selectMenuPanel.SetActive (true);
		}

		void HideSelectMenu() {
			selectMenuPanel.SetActive (false);
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
		DIALOGUE,
		STARTMENU,
		SELECTMENU
	}
}