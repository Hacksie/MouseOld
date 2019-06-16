﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class CoreGame : MonoBehaviour {

		public static CoreGame instance;

		public Timer timer;

		public string testLevel = "IntroRoom";
		public string testLevelGenTemplate = "IntroRoom";

		[SerializeField]
		public GameState state = GameState.LOADING;

		private Input.IInputController inputController = new Input.DesktopInputController ();

		private GameObject player;
		private PlayerController playerController;
		public StartMenuManager startMenuManager;
		public StartMenuPanelPresenter startMenuPanel;
		public SelectMenuManager selectMenuManager;
		public SelectMenuPanelPresenter selectMenuPanel;
		public WorldMapManager worldMapManager;
		public WorldMapPanelPresenter worldMapPanel;

		public Story.InfoManager infoManager;
		public Story.InfoPanelPresenter infoPanel;
		public Story.TaskManager taskManager;
		public Story.TaskPanelPresenter taskPanel;
		public Dialogue.NarrationManager narrationManager;
		public Dialogue.NarrationPanelPresenter narrationPanel;
		public Dialogue.DialogueManager dialogueManager;
		public Dialogue.DialoguePanelPresenter dialoguePanel;

		public Level.LevelGenerator levelGenerator;

		public TimerPanelPresenter timerPanel;

		private List<Triggers.ITrigger> triggerList = new List<Triggers.ITrigger> ();
		private List<NPC.BaseNPCController> npcList = new List<NPC.BaseNPCController> ();

		CoreGame () {
			instance = this;
		}
#if UNITY_EDITOR
		/// <summary>
		/// Run in editor
		/// </summary>
		void Start () {

			Debug.Log("Editor start function called");
			
			if (SceneManager.GetActiveScene ().name != "MainMenu") {
				Initialization ();
				SceneInitialize (testLevel, testLevelGenTemplate);
			}
		}
#endif		

		/// <summary>
		/// Run only once
		/// </summary>
		public void Initialization () {
			state = GameState.LOADING;
			Debug.Log ("Initialization");

			timerPanel.Initialize (this.timer);

			narrationManager.Initialize (inputController);
			dialogueManager.Initialize (inputController);
			startMenuPanel.Initialize (startMenuManager);
			selectMenuPanel.Initialize (selectMenuManager, infoPanel, taskPanel);
			narrationPanel.Initialize (narrationManager);
			dialoguePanel.Initialize (dialogueManager);
			worldMapPanel.Initialize (worldMapManager);
			taskPanel.Initialize (taskManager);
			infoPanel.Initialize (infoManager);

		}

		/// <summary>
		/// Run this each time the scene is changed
		/// </summary>
		public void SceneInitialize (string name, string levelGenTemplate) {
			state = GameState.LOADING;
			Debug.Log ("Scene Initialization");
			player = GameObject.FindWithTag (TagManager.PLAYER);

			GameObject environmentObj = GameObject.FindWithTag (TagManager.ENVIRONMENT);
			levelGenerator.Initialize (environmentObj);

			int seed = UnityEngine.Random.seed;

			Level.Level level = levelGenerator.GenerateLevel (name, levelGenTemplate, seed);
			player.transform.position = level.ConvertLevelPosToWorld(level.spawn);			

			GameObject sceneStoriesObj = GameObject.FindWithTag (TagManager.STORY);

			playerController = player.GetComponent<PlayerController> ();

			


			SceneTriggersInitialize ();
			SceneNPCsInitialize ();

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

			timer.Start ();
		}

		void RepaintAll () {
			narrationPanel.Show (false);
			dialoguePanel.Show (false);
			startMenuPanel.Show (false);
			selectMenuPanel.Show (false);
			worldMapPanel.Show (false);
			timerPanel.Repaint ();
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
				NPC.BaseNPCController npc = npcObject.GetComponent<NPC.BaseNPCController> ();
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
			startMenuPanel.Show (false);
			selectMenuPanel.Show (false);
			narrationPanel.Show (false);
			dialoguePanel.Show (false);
			worldMapPanel.Show (false);
			Cursor.visible = false;
		}

		public void SetDialogue () {
			Debug.Log ("State set to DIALOGUE");
			Time.timeScale = 0;
			dialoguePanel.Show (true);
			state = GameState.DIALOGUE;
			Cursor.visible = true;
		}

		public void SetNarration () {
			Debug.Log ("State set to NARRATION");
			Time.timeScale = 0;
			narrationPanel.Show (true);
			state = GameState.NARRATION;
			Cursor.visible = true;
		}

		public void SetWorldMap () {
			Debug.Log ("State set to WORLDMAP");
			Time.timeScale = 0;
			state = GameState.WORLDMAP;
			worldMapPanel.Show (true);
			Cursor.visible = true;
		}

		// public void ChangeScene (string newGameScene) {
		// 	StartCoroutine (LoadGameScene (newGameScene));
		// }

		IEnumerator LoadGameScene (string newGameScene, string levelName, string levelGenTemplate) {
			Debug.Log ("Loading new game scenes");

			List<Scene> currentScenes = new List<Scene> ();

			for (int i = 0; i < SceneManager.sceneCount; i++) {
				if (SceneManager.GetSceneAt (i).isLoaded && SceneManager.GetSceneAt (i).name != "Core") {
					currentScenes.Add (SceneManager.GetSceneAt (i));

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

			for (int j = 0; j < currentScenes.Count; j++) {
				SceneManager.UnloadScene (currentScenes[j]);
			}

			CoreGame.instance.SceneInitialize (levelName, levelGenTemplate);
		}

		void Update () {

			switch (state) {

				case GameState.PLAYING:
					PlayingUpdate ();
					timer.UpdateTimer ();
					timerPanel.Repaint ();

					if (inputController.StartButtonUp ()) {
						Debug.Log ("Show start menu");
						Cursor.visible = true;
						state = GameState.STARTMENU;
						startMenuPanel.Show (true);
					}

					if (inputController.SelectButtonUp ()) {
						Debug.Log ("Show select menu");
						Cursor.visible = true;
						state = GameState.SELECTMENU;
						selectMenuPanel.Show (true);
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
		SELECTMENU,
		DOOR
	}
}