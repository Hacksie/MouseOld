using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class CoreGame : MonoBehaviour {

		public static CoreGame Instance { get; private set; }

		public State CoreState {get; private set; } = new State ();

		private Input.IInputController inputController;

		[Header ("Test Flags")]
		[SerializeField]
		private RuntimePlatform testPlatform;
		[SerializeField]
		private bool testPlatformFlag;

		[Header("Game")]
		[SerializeField]
		private Entity.EntityManager entityManager;

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
		private GameObject UI;
		[SerializeField]
		private CursorPresenter cursorPresenter;
		[SerializeField]
		private MainMenuPresenter mainMenu;
		[SerializeField]
		private Story.ActionManager actionManager;		
		[SerializeField]
		private ActionConsolePresenter actionConsolePanel;
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
			Instance = this;
		}

		/// <summary>
		/// Run in editor
		/// </summary>
		void Start () {
			CheckBindings ();
			Initialization ();
		}

		void CheckBindings () {
			if(entityManager == null) {
				Debug.LogError(this.name +": entityManager not set");
			}
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
			CoreState.state = GameState.MAINMENU;
			Debug.Log (this.name + ": Initialization");

			UI.SetActive(true);

			SetPlatformInput ();

			//timerPanel.Initialize (this.timer);
			mobileInputUI.Initialize(inputController);
			actionConsolePanel.Initialize(actionManager);
			narrationManager.Initialize (inputController);
			dialogueManager.Initialize (inputController);
			infoPanel.Initialize (infoManager, selectMenuManager);
			taskPanel.Initialize (taskManager, selectMenuManager);
			startMenuPanel.Initialize (startMenuManager);
			selectMenuPanel.Initialize (selectMenuManager, infoPanel, taskPanel, levelMapPanel);
			narrationPanel.Initialize (narrationManager);
			dialoguePanel.Initialize (dialogueManager);
			worldMapPanel.Initialize (worldMapManager);
			levelRenderer.Initialize (entityManager, levelParent, npcParent, polyNav2D);

			RepaintAll ();

			ShowPlayer (false);
		}

		private void SetPlatformInput () {
			switch (testPlatformFlag ? testPlatform : Application.platform) {
				case RuntimePlatform.Android:
					Debug.Log (this.name + ": input platform Android");
					inputController = new Input.AndroidInputController (mobileInputUI);
					break;
				default:
					Debug.Log (this.name + ": input platform Default");
					//inputController = new Input.AndroidInputController (mobileInputUI);
					inputController = new Input.DesktopInputController ();
					break;
			}

		}

		public void LoadNewGame () {
			Debug.Log(this.name + ": loading new game");
			CoreState.state = GameState.LOADING;
			
			entityManager.Initialize(npcParent);
			CoreState.level = levelGenerator.GenerateLevel ("Olivia's Room", 1, 1, 1, 0, 0, 0);
			CoreState.player = new Character.PlayerState();
			
			CoreGame.Instance.SceneInitialize ();
		}

		public void LoadRandomGame (string template, int length, int height, int width, int difficulty, int enemies, int traps) {
			CoreState.state = GameState.LOADING;
			CoreState.level = levelGenerator.GenerateLevel (template, length, height, width, difficulty, enemies, traps);
			CoreState.player = new Character.PlayerState();
			entityManager.Initialize(npcParent);
			CoreGame.Instance.SceneInitialize ();
		}

		public void EndGame () {
			CoreState.state = GameState.MAINMENU;
			RepaintAll ();
			levelRenderer.DestroyLevel ();
			CoreState.entityList.Clear ();
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

			Debug.Log (this.name + ": scene initialization");

			ShowPlayer (true);

			
			levelRenderer.Render (this.CoreState.level);
			this.CoreState.entityList.Clear();
			this.CoreState.entityList.AddRange(levelRenderer.PopulateNPCSpawns(this.CoreState.level));
			this.CoreState.entityList.AddRange(levelRenderer.PopulateEnemySpawns(this.CoreState.level));
			this.CoreState.entityList.AddRange(levelRenderer.PopulateTrapSpawns(this.CoreState.level));

			levelMapPanel.Initialize (selectMenuManager, CoreState.level);

			player.transform.position = CoreState.level.ConvertLevelPosToWorld (CoreState.level.playerSpawn.levelLocation) + CoreState.level.playerSpawn.worldOffset;
			

			//GameObject sceneStoriesObj = GameObject.FindWithTag (TagManager.STORY);

			playerController = player.GetComponent<PlayerController> ();

			SceneTriggersInitialize ();
			CreateAlert();
	
			SetPlaying ();

			if (!string.IsNullOrWhiteSpace (CoreState.level.template.startingAction)) {
				Story.ActionManager.instance.Invoke (CoreState.level.template.startingAction);
			}

			RepaintAll ();

		}

		void RepaintAll () {

			mainMenu.Repaint ();
			actionConsolePanel.Repaint();
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
			CoreState.triggerList.Clear ();
			Debug.Log (this.name + ": initializing triggers, count " + GameObject.FindGameObjectsWithTag ("Trigger").Length);

			foreach (GameObject triggerObject in GameObject.FindGameObjectsWithTag ("Trigger")) {
				Debug.Log (this.name + ": initializing trigger " + triggerObject.name);
				Triggers.ITrigger trigger = triggerObject.GetComponent<Triggers.ITrigger> ();
				if (trigger != null) {
					CoreState.triggerList.Add (trigger);
					trigger.Initialize (inputController);
				}
			}
		}

		public GameObject GetPlayer () {
			return player;
		}

		public void GameOver () {
			Debug.Log (this.name + ": GameOver");
			CoreState.state = GameState.GAMEOVER;
		}

		public void SetPlaying () {
			Debug.Log (this.name + ": state set to PLAYING");
			Time.timeScale = 1;
			CoreState.state = GameState.PLAYING;
		}

		public void SetDialogue () {
			Debug.Log (this.name + ": state set to DIALOGUE");
			Time.timeScale = 0;
			CoreState.state = GameState.DIALOGUE;
			//Cursor.visible = true;
		}

		public void SetNarration () {
			Debug.Log (this.name + ": state set to NARRATION");
			Time.timeScale = 0;
			CoreState.state = GameState.NARRATION;
			RepaintAll ();
		}

		public void SetWorldMap () {
			Debug.Log (this.name + ": state set to WORLDMAP");
			Time.timeScale = 0;
			CoreState.state = GameState.WORLDMAP;
		}

		public void CreateAlert(){
			this.roomAlert = GameObject.Instantiate(roomAlertPrefab, Vector3.zero, Quaternion.identity, levelParent.transform);
			ClearAlert();
			
		}

		public void SetAlert (GameObject trap) {
			Debug.Log (this.name + ": level alert set");
			this.CoreState.alertTrap = trap;
			this.roomAlert.transform.position = trap.transform.position;
			this.roomAlert.SetActive(true);

		}

		public void ClearAlert () {
			this.CoreState.alertTrap = null;
			this.roomAlert.SetActive(false);
		}

		void Update () {

			switch (CoreState.state) {

				case GameState.PLAYING:
					PlayingUpdate ();
					//timer.UpdateTimer ();

					if (inputController.StartButtonUp ()) {
						Debug.Log (this.name + ": show start menu");
						CoreState.state = GameState.STARTMENU;
					}

					if (inputController.SelectButtonUp ()) {
						Debug.Log (this.name + ": show select menu");
						CoreState.state = GameState.SELECTMENU;
					}

					break;

				case GameState.STARTMENU:
					if (inputController.StartButtonUp ()) {
						Debug.Log (this.name + ": hide start menu");
						SetPlaying ();
					}
					break;

				case GameState.SELECTMENU:
					if (inputController.SelectButtonUp ()) {
						Debug.Log (this.name + ": hide select menu");
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

			switch (CoreState.state) {
				case GameState.NARRATION:
					//narrationPanel.Repaint ();
					break;
			}
		}

		void FixedUpdate () {
			switch (CoreState.state) {
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
			foreach (Triggers.ITrigger trigger in CoreState.triggerList) {
				trigger.UpdateTrigger ();
			}
		}

		void PlayingNPCUpdate () {
			foreach (Entity.BaseEntity npc in CoreState.entityList) {
				npc.UpdateBehaviour ();
			}
		}

		void PlayingFixedUpdate () {
			//mapUI.SetPlayerLocation(player.transform.position);
			playerController.UpdateTransform ();
		}
	}
}