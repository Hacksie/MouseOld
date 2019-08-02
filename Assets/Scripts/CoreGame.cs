using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class CoreGame : MonoBehaviour {

		public static CoreGame instance;

		public State state = new State ();

		public Input.IInputController inputController;

		[Header("Test Flags")]
		[SerializeField]
		private RuntimePlatform testPlatform;
		[SerializeField]
		private bool testPlatformFlag;

		[Header("Player")]
		[SerializeField]
		private GameObject player;
		private PlayerController playerController;		

		[Header("Mobile UI")]
		[SerializeField]
		private Input.MobileInputUIPresenter mobileInputUI;

		[Header("UI")]
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
		[SerializeField]
		private Level.LevelGenerator levelGenerator;

		// [SerializeField]
		// private TimerPanelPresenter timerPanel;
		// [SerializeField]
		// private Timer timer;

		private List<Triggers.ITrigger> triggerList = new List<Triggers.ITrigger> ();
		private List<NPC.BaseNPCController> npcList = new List<NPC.BaseNPCController> ();

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
			selectMenuPanel.Initialize (selectMenuManager, infoPanel, taskPanel);
			narrationPanel.Initialize (narrationManager);
			dialoguePanel.Initialize (dialogueManager);
			worldMapPanel.Initialize (worldMapManager);
			

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
			CoreGame.instance.Initialization ();
			//CoreGame.instance.SceneInitialize ("Jennifer's Room", "Jennifer's Room");
			CoreGame.instance.SceneInitialize ("Easy Magenta Lab", 7, 10, 10, 0, 2);
		}

		public void LoadRandomGame (string template, int length, int height, int width, int difficulty, int enemies) {
			CoreGame.instance.SceneInitialize (template, length, height, width, difficulty, enemies);
		}

		public void EndGame () {
			state.state = GameState.MAINMENU;
			RepaintAll ();
			levelGenerator.DestroyLevel ();
			npcList.Clear ();
			ShowPlayer (false);

			Time.timeScale = 0;
		}

		public void SceneInitialize (string levelGenTemplate) {
			SceneInitialize (levelGenTemplate, 0, 0, 0, 0, 0);
		}

		private void ShowPlayer (bool flag) {
			if (player != null) {
				player.SetActive (flag);
			}
		}

		/// <summary>
		/// Run this each time the scene is changed
		/// </summary>
		public void SceneInitialize (string levelGenTemplate, int length, int height, int width, int difficulty, int enemies) {
			state.state = GameState.LOADING;
			Debug.Log ("Scene Initialization");

			ShowPlayer (true);
			//player = GameObject.FindWithTag (TagManager.PLAYER);

			GameObject environmentObj = GameObject.FindWithTag (TagManager.ENVIRONMENT);
			levelGenerator.Initialize (environmentObj);

			Level.Level level = levelGenerator.GenerateLevel (levelGenTemplate, length, height, width, difficulty, enemies);

			levelMapPanel.Initialize (level);

			player.transform.position = level.ConvertLevelPosToWorld (level.spawn);

			GameObject sceneStoriesObj = GameObject.FindWithTag (TagManager.STORY);

			playerController = player.GetComponent<PlayerController> ();

			SceneTriggersInitialize ();
			SceneNPCsInitialize (level);

			RepaintAll ();
			SetResume ();

			if (sceneStoriesObj != null) {

				Story.StoryEventTransition[] stories = sceneStoriesObj.GetComponents<Story.StoryEventTransition> ();

				for (int i = 0; i < stories.Length; i++) {
					stories[i].Invoke ();
				}
			} else {
				Debug.LogWarning ("No starting stories set");
			}

			//timer.Start ();
		}

		void RepaintAll () {

			mainMenu.Repaint();
			dialoguePanel.Repaint ();
			narrationPanel.Repaint ();
			selectMenuPanel.Repaint ();
			startMenuPanel.Repaint ();

			levelMapPanel.Repaint ();
			worldMapPanel.Repaint();
			//timerPanel.Repaint ();
			mobileInputUI.Repaint();
		}

		void SceneTriggersInitialize () {
			triggerList.Clear ();
			Debug.Log ("Initializing triggers, count " + GameObject.FindGameObjectsWithTag ("Trigger").Length);

			foreach (GameObject triggerObject in GameObject.FindGameObjectsWithTag ("Trigger")) {
				Debug.Log ("Initializing trigger " + triggerObject.name);
				Triggers.ITrigger trigger = triggerObject.GetComponent<Triggers.ITrigger> ();
				if (trigger != null) {
					triggerList.Add (trigger);
					trigger.Initialize (inputController);
				}
			}
		}

		void SceneNPCsInitialize (Level.Level level) {
			npcList.Clear ();

			foreach (GameObject npcObject in GameObject.FindGameObjectsWithTag ("NPC")) {
				NPC.BaseNPCController npc = npcObject.GetComponent<NPC.BaseNPCController> ();
				if (npc != null) {
					npcList.Add (npc);
					npc.Initialize (level, levelGenerator.polyNav2D);
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

		public void SetResume () {
			Debug.Log ("State set to RESUME");
			Time.timeScale = 1;
			
			state.state = GameState.PLAYING;
		}

		public void SetDialogue () {
			Debug.Log ("State set to DIALOGUE");
			Time.timeScale = 0;
			//dialoguePanel.Show (true);
			state.state = GameState.DIALOGUE;
			//Cursor.visible = true;
		}

		public void SetNarration () {
			Debug.Log ("State set to NARRATION");
			Time.timeScale = 0;
			state.state = GameState.NARRATION;
		}

		public void SetWorldMap () {
			Debug.Log ("State set to WORLDMAP");
			Time.timeScale = 0;
			state.state = GameState.WORLDMAP;
		}

		void Update () {

			switch (state.state) {

				case GameState.PLAYING:
					PlayingUpdate ();
					//timer.UpdateTimer ();

					if (inputController.StartButtonUp ()) {
						Debug.Log ("Show start menu");
						Cursor.visible = true;
						state.state = GameState.STARTMENU;
					}

					if (inputController.SelectButtonUp ()) {
						Debug.Log ("Show select menu");
						Cursor.visible = true;
						state.state = GameState.SELECTMENU;
					}

					break;

				case GameState.STARTMENU:
					if (inputController.StartButtonUp ()) {
						Debug.Log ("Hide start menu");
						state.state = GameState.PLAYING;
						SetResume ();
					}
					break;

				case GameState.SELECTMENU:
					if (inputController.SelectButtonUp ()) {
						Debug.Log ("Hide select menu");
						state.state = GameState.PLAYING;
						SetResume ();
					}
					break;

				case GameState.GAMEOVER:
					EndGame ();
					break;

			}

			RepaintAll();


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
			foreach (Triggers.ITrigger trigger in triggerList) {
				trigger.UpdateTrigger ();
			}
		}

		void PlayingNPCUpdate () {
			foreach (NPC.BaseNPCController npc in npcList) {
				npc.UpdateBehaviour ();
			}
		}

		void PlayingFixedUpdate () {
			//mapUI.SetPlayerLocation(player.transform.position);
			playerController.UpdateTransform ();
		}
	}
}