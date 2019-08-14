using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class CoreGame : MonoBehaviour {

		public static CoreGame instance;

		public State state = new State ();

		public Input.IInputController inputController;

		[Header ("Test Flags")]
		[SerializeField]
		private RuntimePlatform testPlatform;
		[SerializeField]
		private bool testPlatformFlag;

		[Header ("Player")]
		[SerializeField]
		private GameObject player;
		private PlayerController playerController;

		[Header ("Level")]
		[SerializeField]
		private Level.LevelGenerator levelGenerator;
		[SerializeField]
		private Level.LevelRenderer levelRenderer;
		[SerializeField]
		private GameObject levelParent;
		[SerializeField]
		private GameObject npcParent;
		[SerializeField]
		private PolyNav.PolyNav2D polyNav2D;

		[SerializeField]
		private GameObject roomAlertPrefab;
		private GameObject roomAlert;

		[Header ("Mobile UI")]
		[SerializeField]
		private Input.MobileInputUIPresenter mobileInputUI;

		[Header ("UI")]
		[SerializeField]
		private CursorPresenter cursorPresenter;
		[SerializeField]
		private MainMenuPresenter mainMenu;
		[SerializeField]
		private StartMenuManager startMenuManager;
		[SerializeField]
		private StartMenuPanelPresenter startMenuPanel;
		[SerializeField]
		private SelectMenuManager selectMenuManager;
		[SerializeField]
		private SelectMenuPanelPresenter selectMenuPanel;
		[SerializeField]
		private WorldMapManager worldMapManager;
		[SerializeField]
		private WorldMapPanelPresenter worldMapPanel;
		[SerializeField]
		private Story.InfoManager infoManager;
		[SerializeField]
		private Story.InfoPanelPresenter infoPanel;
		[SerializeField]
		private Story.TaskManager taskManager;
		[SerializeField]
		private Story.TaskPanelPresenter taskPanel;
		[SerializeField]
		private Dialogue.NarrationManager narrationManager;
		[SerializeField]
		private Dialogue.NarrationPanelPresenter narrationPanel;
		[SerializeField]
		private Dialogue.DialogueManager dialogueManager;
		[SerializeField]
		private Dialogue.DialoguePanelPresenter dialoguePanel;

		[SerializeField]
		private Level.LevelMapPanelPresenter levelMapPanel;

		// [SerializeField]
		// private TimerPanelPresenter timerPanel;
		// [SerializeField]
		// private Timer timer;

		

		

		CoreGame () {
			instance = this;
		}

		/// <summary>
		/// Run in editor
		/// </summary>
		void Start () {

			CheckBindings ();
			Initialization ();

		}

		void CheckBindings () {
			if (cursorPresenter == null) {
				Debug.LogError ("cursorPresenter not set");
			}

			if (levelMapPanel == null) {
				Debug.LogError ("levelMapPanel not set");
			}
		}

		/// <summary>
		/// Run only once
		/// </summary>
		public void Initialization () {
			state.state = GameState.MAINMENU;
			Debug.Log ("Initialization");

			SetPlatformInput ();

			//timerPanel.Initialize (this.timer);

			narrationManager.Initialize (inputController);
			dialogueManager.Initialize (inputController);
			infoPanel.Initialize (infoManager, selectMenuManager);
			taskPanel.Initialize (taskManager, selectMenuManager);
			startMenuPanel.Initialize (startMenuManager);
			selectMenuPanel.Initialize (selectMenuManager, infoPanel, taskPanel, levelMapPanel);
			narrationPanel.Initialize (narrationManager);
			dialoguePanel.Initialize (dialogueManager);
			worldMapPanel.Initialize (worldMapManager);
			levelRenderer.Initialize (levelParent, npcParent, polyNav2D);

			RepaintAll ();

			ShowPlayer (false);
		}

		private void SetPlatformInput () {
			switch (testPlatformFlag ? testPlatform : Application.platform) {
				case RuntimePlatform.Android:
					Debug.Log ("Android");
					inputController = new Input.AndroidInputController (mobileInputUI);
					break;
				default:
					Debug.Log ("Default platform");
					//inputController = new Input.AndroidInputController (mobileInputUI);
					inputController = new Input.DesktopInputController ();
					break;
			}

		}

		public void LoadNewGame () {
			state.state = GameState.LOADING;
			state.level = levelGenerator.GenerateLevel ("Victoria's Room", 1, 1, 2, 0, 0, 0);
			state.player = new Character.PlayerState();
			CoreGame.instance.SceneInitialize ();
		}

		public void LoadRandomGame (string template, int length, int height, int width, int difficulty, int enemies, int traps) {
			state.state = GameState.LOADING;
			state.level = levelGenerator.GenerateLevel (template, length, height, width, difficulty, enemies, traps);
			state.player = new Character.PlayerState();
			CoreGame.instance.SceneInitialize ();
		}

		public void EndGame () {
			state.state = GameState.MAINMENU;
			RepaintAll ();
			levelRenderer.DestroyLevel ();
			state.entityList.Clear ();
			ShowPlayer (false);

			Time.timeScale = 0;
		}

		private void ShowPlayer (bool flag) {
			if (player != null) {
				player.SetActive (flag);
			}
		}

		/// <summary>
		/// Run this each time the scene is changed
		/// </summary>
		public void SceneInitialize () {

			Debug.Log ("Scene Initialization");

			ShowPlayer (true);

			
			levelRenderer.Render (this.state.level);
			this.state.entityList.Clear();
			this.state.entityList.AddRange(levelRenderer.PopulateEnemySpawns(this.state.level));
			this.state.entityList.AddRange(levelRenderer.PopulateTrapSpawns(this.state.level));

			levelMapPanel.Initialize (selectMenuManager, state.level);

			player.transform.position = state.level.ConvertLevelPosToWorld (state.level.spawn);

			//GameObject sceneStoriesObj = GameObject.FindWithTag (TagManager.STORY);

			playerController = player.GetComponent<PlayerController> ();

			SceneTriggersInitialize ();
			CreateAlert();
	
			SetPlaying ();

			if (!string.IsNullOrWhiteSpace (state.level.template.startingAction)) {
				Story.ActionManager.instance.Invoke (state.level.template.startingAction);
			}

			RepaintAll ();

		}

		void RepaintAll () {

			mainMenu.Repaint ();
			dialoguePanel.Repaint ();
			narrationPanel.Repaint ();
			selectMenuPanel.Repaint ();
			startMenuPanel.Repaint ();

			levelMapPanel.Repaint ();
			worldMapPanel.Repaint ();
			//timerPanel.Repaint ();
			mobileInputUI.Repaint ();
			cursorPresenter.Repaint ();
		}

		void SceneTriggersInitialize () {
			state.triggerList.Clear ();
			Debug.Log ("Initializing triggers, count " + GameObject.FindGameObjectsWithTag ("Trigger").Length);

			foreach (GameObject triggerObject in GameObject.FindGameObjectsWithTag ("Trigger")) {
				Debug.Log ("Initializing trigger " + triggerObject.name);
				Triggers.ITrigger trigger = triggerObject.GetComponent<Triggers.ITrigger> ();
				if (trigger != null) {
					state.triggerList.Add (trigger);
					trigger.Initialize (inputController);
				}
			}
		}

		public GameObject GetPlayer () {
			return player;
		}

		public void GameOver () {
			Debug.Log ("GameOver");
			state.state = GameState.GAMEOVER;
		}

		public void SetPlaying () {
			Debug.Log ("State set to PLAYING");
			Time.timeScale = 1;
			state.state = GameState.PLAYING;
		}

		public void SetDialogue () {
			Debug.Log ("State set to DIALOGUE");
			Time.timeScale = 0;
			state.state = GameState.DIALOGUE;
			//Cursor.visible = true;
		}

		public void SetNarration () {
			Debug.Log ("State set to NARRATION");
			Time.timeScale = 0;
			state.state = GameState.NARRATION;
			RepaintAll ();
		}

		public void SetWorldMap () {
			Debug.Log ("State set to WORLDMAP");
			Time.timeScale = 0;
			state.state = GameState.WORLDMAP;
		}

		public void CreateAlert(){
			this.roomAlert = GameObject.Instantiate(roomAlertPrefab, Vector3.zero, Quaternion.identity, levelParent.transform);
			ClearAlert();
			
		}

		public void SetAlert (GameObject trap) {
			Debug.Log ("Level alert set");
			this.state.alertTrap = trap;
			this.roomAlert.transform.position = trap.transform.position;
			this.roomAlert.SetActive(true);

		}

		public void ClearAlert () {
			this.state.alertTrap = null;
			this.roomAlert.SetActive(false);
		}

		void Update () {

			switch (state.state) {

				case GameState.PLAYING:
					PlayingUpdate ();
					//timer.UpdateTimer ();

					if (inputController.StartButtonUp ()) {
						Debug.Log ("Show start menu");
						state.state = GameState.STARTMENU;
					}

					if (inputController.SelectButtonUp ()) {
						Debug.Log ("Show select menu");
						state.state = GameState.SELECTMENU;
					}

					break;

				case GameState.STARTMENU:
					if (inputController.StartButtonUp ()) {
						Debug.Log ("Hide start menu");
						SetPlaying ();
					}
					break;

				case GameState.SELECTMENU:
					if (inputController.SelectButtonUp ()) {
						Debug.Log ("Hide select menu");
						SetPlaying ();
					}
					break;

				case GameState.GAMEOVER:
					EndGame ();
					break;

			}

			RepaintAll ();

		}

		void LateUpdate () {

			switch (state.state) {
				case GameState.NARRATION:
					//narrationPanel.Repaint ();
					break;
			}
		}

		void FixedUpdate () {
			switch (state.state) {
				case GameState.PLAYING:
					PlayingFixedUpdate ();
					break;
			}
		}

		void PlayingUpdate () {

			playerController.UpdateMovement (inputController);
			PlayingNPCUpdate ();
			PlayingTriggerUpdate ();
		}

		void PlayingTriggerUpdate () {
			foreach (Triggers.ITrigger trigger in state.triggerList) {
				trigger.UpdateTrigger ();
			}
		}

		void PlayingNPCUpdate () {
			foreach (Entity.BaseEntity npc in state.entityList) {
				npc.UpdateBehaviour ();
			}
		}

		void PlayingFixedUpdate () {
			//mapUI.SetPlayerLocation(player.transform.position);
			playerController.UpdateTransform ();
		}
	}
}