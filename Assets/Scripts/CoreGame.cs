using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class CoreGame : MonoBehaviour {


		public static CoreGame instance;

		[SerializeField]
		private GameState state = GameState.LOADING;

		private Input.IInputController inputController = new Input.DesktopInputController();


		private GameObject player;
		private PlayerController playerController;

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
			Debug.Log("Initialization");
		}

		/// <summary>
		/// Run this each time the scene is changed
		/// </summary>
		public void SceneInitialize () {
			Debug.Log("Scene Initialization");
			player = GameObject.FindWithTag (TagManager.PLAYER);
			playerController = player.GetComponent<PlayerController> ();
		}		


		void Update () {

			switch (state) {
				case GameState.LOADING:
					break;

				case GameState.PLAYING:
					PlayingUpdate ();
					break;

				case GameState.CUTSCENE:
					break;

				case GameState.DIALOGUE:
					break;

				case GameState.PAUSE:
					break;
			}
		}

		void FixedUpdate() {
			switch (state) {
				case GameState.LOADING:
					break;

				case GameState.PLAYING:
					PlayingFixedUpdate ();
					break;

				case GameState.CUTSCENE:
					break;

				case GameState.DIALOGUE:
					break;

				case GameState.PAUSE:
					break;
			}			
		}

		void PlayingUpdate () {
			playerController.UpdateMovement (inputController);
		}

		void PlayingFixedUpdate() {
			playerController.UpdateTransform();
		}


	}



	public enum GameState {
		CUTSCENE,
		PLAYING,
		PAUSE,
		LOADING,
		DIALOGUE,
		STARTMENU,
		SELECTMENU
	}
}