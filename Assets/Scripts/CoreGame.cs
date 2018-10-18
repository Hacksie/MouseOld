using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class CoreGame : MonoBehaviour {

		public static CoreGame instance;

		[SerializeField]
		public GameState state = GameState.LOADING;

		private Input.IInputController inputController = new Input.DesktopInputController ();

		private GameObject player;
		private PlayerController playerController;
		private StartMenuManager startMenuManager;
		private StartMenuPanelPresenter startMenuPanel;
		private SelectMenuManager selectMenuManager;
		private SelectMenuPanelPresenter selectMenuPanel;
		private WorldMapManager worldMapManager;
		private WorldMapPanelPresenter worldMapPanel;

		private Story.TaskManager taskManager;
		private Story.TaskPanelPresenter taskPanel;
		private Dialogue.INarrationManager narrationManager;
		private Dialogue.NarrationPanelPresenter narrationPanel;
		private Dialogue.IDialogueManager dialogueManager;
		private Dialogue.DialoguePanelPresenter dialoguePanel;

		//public Story.StoryEvent startingStory;

		private List<Triggers.ITrigger> triggerList = new List<Triggers.ITrigger> ();
		private List<NPCController> npcList = new List<NPCController> ();

		CoreGame () {
			instance = this;
		}

		// Use this for initialization
		void Start () {
			//FIXME: Make this work in editory only 
			#if UNITY_EDITOR
			//if ((SceneManager.GetActiveScene ().name == "Core" || SceneManager.GetActiveScene ().name == "IntroRoom")) {
				Initialization ();
				SceneInitialize ();
			//}
			#endif
		}

		public void Initialization () {
			state = GameState.LOADING;
			Debug.Log ("Initialization");

			//startMenuPanel = GameObject.FindWithTag (TagManager.STARTMENU);
			//selectMenuPanel = GameObject.FindWithTag (TagManager.SELECTMENU);
			
			GameObject startMenuManagerObj = GameObject.FindWithTag (TagManager.STARTMENU_MANAGER);
			GameObject startMenuPanelObj = GameObject.FindWithTag (TagManager.STARTMENU_PANEL);
			GameObject selectMenuManagerObj = GameObject.FindWithTag (TagManager.SELECTMENU_MANAGER);
			GameObject selectMenuPanelObj = GameObject.FindWithTag (TagManager.SELECTMENU_PANEL);

			GameObject narrationManagerObj = GameObject.FindWithTag (TagManager.NARRATION_MANAGER);
			GameObject narrationPanelObj = GameObject.FindWithTag (TagManager.NARRATION_PANEL);
			GameObject dialogueManagerObj = GameObject.FindWithTag (TagManager.DIALOGUE_MANAGER);
			GameObject dialoguePanelObj = GameObject.FindWithTag (TagManager.DIALOGUE_PANEL);

			GameObject worldmapManagerObj = GameObject.FindWithTag (TagManager.WORLDMAP_MANAGER);
			GameObject worldmapPanelObj = GameObject.FindWithTag (TagManager.WORLDMAP_PANEL);

			GameObject taskManagerObj = GameObject.FindWithTag(TagManager.TASK_MANAGER);
			GameObject taskPanelObj = GameObject.FindWithTag (TagManager.TASK_PANEL);

			startMenuManager = startMenuManagerObj.GetComponent<StartMenuManager> ();
			startMenuPanel = startMenuPanelObj.GetComponent<StartMenuPanelPresenter> ();

			selectMenuManager = selectMenuManagerObj.GetComponent<SelectMenuManager> ();
			selectMenuPanel = selectMenuPanelObj.GetComponent<SelectMenuPanelPresenter> ();

			worldMapManager = worldmapManagerObj.GetComponent<WorldMapManager> ();
			worldMapPanel = worldmapPanelObj.GetComponent<WorldMapPanelPresenter> ();

			narrationManager = narrationManagerObj.GetComponent<Dialogue.NarrationManager> ();
			narrationPanel = narrationPanelObj.GetComponent<Dialogue.NarrationPanelPresenter> ();

			dialogueManager = dialogueManagerObj.GetComponent<Dialogue.DialogueManager> ();
			dialoguePanel = dialoguePanelObj.GetComponent<Dialogue.DialoguePanelPresenter> ();

			taskManager = taskManagerObj.GetComponent<Story.TaskManager>();
			taskPanel = taskPanelObj.GetComponent<Story.TaskPanelPresenter>();

			startMenuPanel.Initialize (startMenuManager);
			selectMenuPanel.Initialize (selectMenuManager);

			narrationManager.Initialize (inputController);
			narrationPanel.Initialize (narrationManager);

			dialogueManager.Initialize (inputController);
			dialoguePanel.Initialize (dialogueManager);

			//worldMapManager.
			worldMapPanel.Initialize (worldMapManager);

			taskPanel.Initialize(taskManager);

		}

		/// <summary>
		/// Run this each time the scene is changed
		/// </summary>
		public void SceneInitialize () {
			state = GameState.LOADING;
			Debug.Log ("Scene Initialization");
			player = GameObject.FindWithTag (TagManager.PLAYER);
			GameObject sceneStoriesObj = GameObject.FindWithTag (TagManager.STORY);
			playerController = player.GetComponent<PlayerController> ();

			SceneTriggersInitialize ();
			SceneNPCsInitialize ();

			narrationPanel.Repaint ();
			dialoguePanel.Repaint ();
			taskPanel.Repaint();

			//taskPanel.SetActive (false);

			startMenuPanel.Repaint ();
			selectMenuPanel.Repaint ();
			worldMapPanel.Repaint ();		
			SetResume();

			//state = GameState.PLAYING; // Set this, and the starting stories can override it if need be	

			if (sceneStoriesObj != null) {

				Story.StoryEventTransition[] stories = sceneStoriesObj.GetComponents<Story.StoryEventTransition> ();

				for (int i = 0; i < stories.Length; i++) {
					stories[i].Invoke ();
				}
			} else {
				Debug.LogWarning ("No starting stories set");
			}


			// if (startingStory != null) {
			// 	startingStory.Start ();
			// } else {
			// 	Debug.LogError ("No starting story set");
			// }
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

		void SceneNPCsInitialize () {
			npcList.Clear ();

			foreach (GameObject npcObject in GameObject.FindGameObjectsWithTag ("NPC")) {
				NPCController npc = npcObject.GetComponent<NPCController> ();
				if (npc != null) {
					npcList.Add (npc);
					npc.Initialize (player.transform);
				}
			}
		}

		public GameObject GetPlayer () {
			return player;
		}

		public void SetResume () {
			Debug.Log ("State set to RESUME");
			Time.timeScale = 1;
			state = GameState.PLAYING;
			startMenuPanel.Repaint ();
			selectMenuPanel.Repaint ();
			narrationPanel.Repaint ();
			dialoguePanel.Repaint ();
			worldMapPanel.Repaint ();
			Cursor.visible = false;
		}

		public void SetDialogue () {
			Debug.Log ("State set to DIALOGUE");
			Time.timeScale = 0;
			dialoguePanel.Repaint ();
			state = GameState.DIALOGUE;
			Cursor.visible = true;
		}

		public void SetNarration () {
			Debug.Log ("State set to NARRATION");
			Time.timeScale = 0;
			narrationPanel.Repaint ();
			state = GameState.NARRATION;
			Cursor.visible = true;
		}

		public void SetWorldMap () {
			Debug.Log ("State set to WORLDMAP");
			Time.timeScale = 0;
			state = GameState.WORLDMAP;
			worldMapPanel.Repaint ();
			Cursor.visible = true;
		}

		public void ChangeScene (string newGameScene) {
			StartCoroutine (LoadGameScene (newGameScene));
		}

		IEnumerator LoadGameScene (string newGameScene) {
			Debug.Log ("Loading new game scenes");

			List<Scene> currentScenes = new List<Scene>();

			for(int i = 0 ; i < SceneManager.sceneCount; i++)
			{
				if(SceneManager.GetSceneAt(i).isLoaded && SceneManager.GetSceneAt(i).name != "Core")
				{
					currentScenes.Add(SceneManager.GetSceneAt(i));

				}
			}

			//Scene currentScene = SceneManager.GetActiveScene ();

			AsyncOperation asyncLoadRubyScene = SceneManager.LoadSceneAsync (newGameScene, LoadSceneMode.Additive);
			asyncLoadRubyScene.allowSceneActivation = false;

			yield return null;

			//Wait until we are done loading the scene
			while (asyncLoadRubyScene.progress < 0.9f) {
				Debug.Log ("Loading scene #:" + newGameScene + " [][] Progress: " + asyncLoadRubyScene.progress);
				yield return null;
			}

			Debug.Log (newGameScene + " ready");

			asyncLoadRubyScene.allowSceneActivation = true;

			while (!asyncLoadRubyScene.isDone) {
				Debug.Log ("Activating scenes");
				yield return null;
			}

			SceneManager.SetActiveScene (SceneManager.GetSceneByName (newGameScene));

			for(int j = 0; j < currentScenes.Count; j++)
			{
				SceneManager.UnloadScene (currentScenes[j]);
			}

			CoreGame.instance.SceneInitialize ();
		}

		void Update () {

			switch (state) {

				case GameState.PLAYING:
					PlayingUpdate ();

					if (inputController.StartButtonUp ()) {
						Debug.Log ("Show start menu");
						Cursor.visible = true;
						state = GameState.STARTMENU;
						startMenuPanel.Repaint ();

						//ShowStartMenu ();
					}

					if (inputController.SelectButtonUp ()) {
						Debug.Log ("Show select menu");
						Cursor.visible = true;
						state = GameState.SELECTMENU;
						selectMenuPanel.Repaint ();
					}

					break;

				case GameState.STARTMENU:
					if (inputController.StartButtonUp ()) {
						Debug.Log ("Hide start menu");
						state = GameState.PLAYING;
						//startMenuPanel.Repaint ();
						//HideStartMenu ();
					}
					break;

				case GameState.SELECTMENU:
					if (inputController.SelectButtonUp ()) {
						Debug.Log ("Hide select menu");
						SetResume ();
						//state = GameState.PLAYING;
						//selectMenuPanel.Repaint ();
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

		void PlayingUpdate () {
			//Debug.Log("Test");
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
			foreach (NPCController npc in npcList) {
				npc.UpdateBehaviour ();
			}
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
		WORLDMAP,
		STARTMENU,
		SELECTMENU
	}
}