
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;


namespace HackedDesign
{
    public class CoreGame : MonoBehaviour
    {
        [Header("Test Flags")]
        [SerializeField] private RuntimePlatform testPlatform = RuntimePlatform.WindowsEditor;
        [SerializeField] private bool testPlatformFlag = false;

        [Header("Game")]
        [SerializeField] private Entities.EntityManager entityManager = null;
        [SerializeField] private GameObject player = null;
        private PlayerController playerController = null;

        [Header("Audio")]
        [SerializeField] private AudioSource denied;

        [Header("Level")]
        [SerializeField] private Level.LevelGenerator levelGenerator = null;
        [SerializeField] private Level.LevelRenderer levelRenderer = null;
        [SerializeField] private GameObject levelParent = null;
        [SerializeField] private GameObject enemiesParent = null;
        [SerializeField] private PolyNav.PolyNav2D polyNav2D = null;
        [SerializeField] private string newGameLevel = "Olivia's Room";
        [SerializeField] private GameObject roomAlertPrefab = null;
        [SerializeField] private GameObject roomAlert = null;

        [Header("Lights")]
        [SerializeField] private Light2D globalLight = null;
        [SerializeField] private Color lightsDefault = Color.black;
        [SerializeField] private Color lightsWarn = Color.black;
        [SerializeField] private Color lightsAlert = Color.black;
        [SerializeField] private Color lightsBar = Color.black;

        [Header("UI")]
        [SerializeField] private Input.MobileInputUIPresenter mobileInputUI = null;
        [SerializeField] private GameObject UI = null;
        [SerializeField] private UI.CursorPresenter cursorPresenter = null;
        [SerializeField] private UI.MainMenuPresenter mainMenu = null;
        [SerializeField] private Story.ActionManager actionManager = null;
        [SerializeField] private UI.ActionConsolePresenter actionConsolePanel = null;
        [SerializeField] private StartMenuManager startMenuManager = null;
        [SerializeField] private UI.StartMenuPanelPresenter startMenuPanel = null;
        [SerializeField] private SelectMenuManager selectMenuManager = null;
        [SerializeField] private UI.SelectMenuPanelPresenter selectMenuPanel = null;
        [SerializeField] private UI.ActionPanelPresenter actionPanel = null;
        [SerializeField] private UI.InfoPanelPresenter infoPanel = null;
        [SerializeField] private UI.StashPanelPresenter stashPanel = null;
        [SerializeField] private UI.PsychPanelPresenter psychPanel = null;
        [SerializeField] private UI.TaskPanelPresenter taskPanel = null;
        [SerializeField] private Dialogue.NarrationManager narrationManager = null;
        [SerializeField] private UI.NarrationPanelPresenter narrationPanel = null;
        [SerializeField] private MissionCompleteManager missionCompleteManager = null;
        [SerializeField] private LevelCompleteManager levelCompleteManager = null;
        [SerializeField] private UI.LevelCompletePresenter levelCompletePresenter = null;
        [SerializeField] private UI.MissionCompletePresenter missionCompletePanel = null;
        [SerializeField] private UI.MinimapPresenter minimapPanel = null;
        [SerializeField] private UI.TitlecardPresenter titlecardPanel = null;
        [SerializeField] private UI.TimerPanelPresenter timerPanel = null;
        [SerializeField] private UI.WorldMapPresenter worldMapPanel = null;

        private CoreGame()
        {
            Instance = this;
        }

        public static CoreGame Instance { get; private set; }

        public GameState.GameState state { get; private set; }

        /// <summary>
        /// Run in editor
        /// </summary>
        void Start()
        {
            CheckBindings();
            Initialization();
        }

        void Update()
        {
            switch (state.state)
            {
                case GameState.GameStateEnum.PLAYING:
                    PlayingUpdate();
                    break;
                case GameState.GameStateEnum.STARTMENU:
                    break;
                case GameState.GameStateEnum.MISSIONCOMPLETE:
                    break;
                case GameState.GameStateEnum.GAMEOVER:
                    EndGame();
                    break;
                default:
                    break;
            }

            UpdateLights();
            RepaintAllUI();
        }

        void LateUpdate()
        {
            actionManager.UpdateBehaviour();
            switch (state.state)
            {
                case GameState.GameStateEnum.CAPTURED:
                case GameState.GameStateEnum.DIALOGUE:
                case GameState.GameStateEnum.GAMEOVER:
                case GameState.GameStateEnum.MISSIONCOMPLETE:
                case GameState.GameStateEnum.LEVELCOMPLETE:
                case GameState.GameStateEnum.SELECTMENU:
                case GameState.GameStateEnum.STARTMENU:
                case GameState.GameStateEnum.WORLDMAP:
                case GameState.GameStateEnum.NARRATION:
                case GameState.GameStateEnum.PLAYING:
                    UpdateDoorAnimations();
                    playerController.Animate();
                    break;
            }
        }

        public void LoadNewGame()
        {
            Logger.Log(this, "Loading new game");
            state.state = GameState.GameStateEnum.LOADING;
            entityManager.Initialize();
            actionManager.Initialize(entityManager);
            state.currentLevel = levelGenerator.GenerateLevel(newGameLevel, 0, 0, 0);
            state.isRandom = false;
            state.player = new GameState.PlayerState();
            SceneInitialize();
        }

        public void LoadRandomGame(string template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            Logger.Log(this, "Loading random game");
            state.state = GameState.GameStateEnum.LOADING;
            state.currentLevel = levelGenerator.GenerateLevel(template, length, height, width, difficulty, enemies, traps);
            state.isRandom = true;
            state.player = new GameState.PlayerState();
            entityManager.Initialize();
            actionManager.Initialize(entityManager);
            SceneInitialize();
        }

        public void LoadNewLevel(string template)
        {
            Logger.Log(this, "Loading new level");
            state.state = GameState.GameStateEnum.LOADING;
            state.entityList.Clear();
            state.currentLevel = levelGenerator.GenerateLevel(template);
            entityManager.Initialize();
            actionManager.Initialize(entityManager);
            SceneInitialize();
        }

        public void EndGame()
        {
            Logger.Log(this, "End Game");
            state.state = GameState.GameStateEnum.MAINMENU;
            RepaintAllUI();
            levelRenderer.DestroyLevel();
            state.entityList.Clear();
            ShowPlayer(false);
            Time.timeScale = 0;
        }

        /// <summary>
        /// Run this each time the scene is changed
        /// </summary>
        public void SceneInitialize()
        {
            Logger.Log(this, "Scene initialization");
            ShowPlayer(true);
            SetLight(GameState.GlobalLightTypes.Default);

            levelRenderer.Render(state.currentLevel);
            levelRenderer.PopulateLevelDoors(state.currentLevel, state.doorList);
            levelRenderer.PopulateNPCSpawns(state.currentLevel, state.entityList);
            levelRenderer.PopulateEnemySpawns(state.currentLevel, state.enemyList);
            minimapPanel.Initialize(state.currentLevel);

            player.transform.position = state.currentLevel.ConvertLevelPosToWorld(state.currentLevel.playerSpawn.levelLocation) + state.currentLevel.playerSpawn.worldOffset;



            SceneTriggersInitialize();
            CreateAlert();
            timerPanel.Initialize(state.currentLevel.timer);

            SetPlaying();

            if (!string.IsNullOrWhiteSpace(state.currentLevel.template.startingAction))
            {
                Story.ActionManager.instance.Invoke(state.currentLevel.template.startingAction);
            }

            RepaintAllUI();
        }

        public GameObject GetPlayer()
        {
            return player;
        }

        public void GameOver()
        {
            Logger.Log(this, "Game Over");
            state.state = GameState.GameStateEnum.GAMEOVER;
        }

        public void SetTitlecard()
        {
            Logger.Log(this, "State set to TITLECARD");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.TITLECARD;
        }

        public void SetPlaying()
        {
            Logger.Log(this, "State set to PLAYING");
            Time.timeScale = 1;
            state.state = GameState.GameStateEnum.PLAYING;
        }

        public void SetDialogue()
        {
            Logger.Log(this, "State set to DIALOGUE");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.DIALOGUE;
        }

        public void SetNarration()
        {
            Logger.Log(this, "State set to NARRATION");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.NARRATION;
            RepaintAllUI();
        }

        public void SetMissionComplete()
        {
            Logger.Log(this, "State set to MISSION COMPLETE");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.MISSIONCOMPLETE;
            RepaintAllUI();
        }

        public void SetLevelComplete()
        {
            Logger.Log(this, "State set to LEVEL COMPLETE");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.LEVELCOMPLETE;
            RepaintAllUI();
        }

        public void SetWorldMap()
        {
            Logger.Log(this, "State set to WORLDMAP");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.WORLDMAP;
        }

        public void ToggleStart()
        {
            if (state.state == GameState.GameStateEnum.PLAYING)
            {
                state.state = GameState.GameStateEnum.STARTMENU;
            }
            else if (state.state == GameState.GameStateEnum.STARTMENU)
            {
                state.state = GameState.GameStateEnum.PLAYING;
            }
        }

        public void ToggleSelect()
        {
            if (state.state == GameState.GameStateEnum.PLAYING)
            {
                state.state = GameState.GameStateEnum.SELECTMENU;
            }
            else if (state.state == GameState.GameStateEnum.SELECTMENU)
            {
                state.state = GameState.GameStateEnum.PLAYING;
            }
        }

        public void CreateAlert()
        {
            roomAlert = Instantiate(roomAlertPrefab, Vector3.zero, Quaternion.identity, levelParent.transform);
            ClearAlert();
        }

        public void SetAlert(GameObject trap)
        {
            Logger.Log(this, "Set alert");
            state.alertTrap = trap;
            roomAlert.transform.position = trap.transform.position;
            roomAlert.SetActive(true);
        }

        public void ClearAlert()
        {
            Logger.Log(this, "Clear alert");
            state.alertTrap = null;
            roomAlert.SetActive(false);
        }

        public void SaveGame()
        {
            Logger.Log(this, "Saving state");
            string json = JsonUtility.ToJson(state);
            string path = Path.Combine(Application.persistentDataPath, $"SaveFile{state.gameSlot}.json");
            File.WriteAllText(path, json);
            Logger.Log(this, "Saved ", path);
        }

        public void UpdateLights()
        {
            /*
            switch (state.currentLight)
            {
                case GameState.GlobalLightTypes.Warn:
                    globalLight.color = lightsWarn;
                    break;
                case GameState.GlobalLightTypes.Alert:
                    globalLight.color = lightsAlert;
                    break;
                case GameState.GlobalLightTypes.Bar:
                    globalLight.color = lightsBar;
                    break;
                case GameState.GlobalLightTypes.Default:
                default:
                    globalLight.color = lightsDefault;
                    break;
            }*/
        }

        public void SetLight(GameState.GlobalLightTypes light)
        {
            state.currentLight = light;
        }

        public bool IsPlaying()
        {
            return state.state == GameState.GameStateEnum.PLAYING;
        }

        public bool IsInGame()
        {
            return state.state == GameState.GameStateEnum.PLAYING || state.state == GameState.GameStateEnum.NARRATION || state.state == GameState.GameStateEnum.STARTMENU || state.state == GameState.GameStateEnum.SELECTMENU || CoreGame.Instance.state.state == GameState.GameStateEnum.LEVELCOMPLETE || state.state == GameState.GameStateEnum.WORLDMAP;
        }

        private void CheckBindings()
        {
            if (entityManager == null)
            {
                Logger.Log(this, "entityManager not set");
            }
            if (cursorPresenter == null)
            {
                Logger.Log(this, "cursorPresenter not set");
            }
        }

        /// <summary>
        /// Run only once
        /// </summary>
        private void Initialization()
        {
            state = new GameState.GameState();
            state.state = GameState.GameStateEnum.MAINMENU;
            Logger.Log(this, "Initialization");

            // GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
            // GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);

            playerController = player.GetComponent<PlayerController>();

            UI.SetActive(true);
            SetPlatformInput();
            narrationManager.Initialize();
            missionCompleteManager.Initialize(actionManager);
            levelCompleteManager.Initialize(actionManager);
            mobileInputUI.Initialize();
            actionConsolePanel.Initialize(actionManager);
            infoPanel.Initialize(selectMenuManager);
            stashPanel.Initialize(selectMenuManager);
            psychPanel.Initialize(selectMenuManager);
            taskPanel.Initialize(selectMenuManager);
            startMenuPanel.Initialize(state, startMenuManager);
            selectMenuPanel.Initialize(selectMenuManager, infoPanel, taskPanel, stashPanel, psychPanel);
            narrationPanel.Initialize(narrationManager);
            actionPanel.Initialize(playerController);
            titlecardPanel.Initialize(actionManager);
            levelRenderer.Initialize(entityManager, levelParent, enemiesParent, polyNav2D);
            missionCompletePanel.Initialize(missionCompleteManager);
            levelCompletePresenter.Initialize(levelCompleteManager);
            worldMapPanel.Initialize();
            RepaintAllUI();
            ShowPlayer(false);
        }

        private void SceneTriggersInitialize()
        {
            state.triggerList.Clear();
            Logger.Log(this, "Initializing triggers");

            foreach (var triggerObject in GameObject.FindGameObjectsWithTag(TagManager.TRIGGER))
            {
                if (!triggerObject.activeInHierarchy)
                {
                    continue;
                }

                Logger.Log(this, "Initializing trigger - ", triggerObject.name);
                BaseTrigger trigger = triggerObject.GetComponent<BaseTrigger>();
                if (trigger != null)
                {
                    state.triggerList.Add(trigger);
                    trigger.Initialize();
                }
            }
        }

        private void SetPlatformInput()
        {
            switch (testPlatformFlag ? testPlatform : Application.platform)
            {
                case RuntimePlatform.Android:
                    Logger.Log(this, "Input platform Android");
                    break;
                default:
                    Logger.Log(this, "Input platform Default");
                    break;
            }
        }

        private void RepaintAllUI()
        {
            mainMenu.Repaint();
            actionConsolePanel.Repaint();
            narrationPanel.Repaint();
            selectMenuPanel.Repaint();
            startMenuPanel.Repaint();
            missionCompletePanel.Repaint();
            levelCompletePresenter.Repaint();
            stashPanel.Repaint();
            psychPanel.Repaint();
            actionPanel.Repaint();
            timerPanel.Repaint();
            mobileInputUI.Repaint();
            minimapPanel.Repaint();
            cursorPresenter.Repaint();
            titlecardPanel.Repaint();
            worldMapPanel.Repaint();
        }

        private void ShowPlayer(bool flag)
        {
            if (player != null)
            {
                player.SetActive(flag);
            }
        }

        private void UpdateDoorAnimations()
        {
            foreach (var door in state.doorList)
            {
                door.UpdateAnimation();
            }
        }

        private void PlayingUpdate()
        {
            playerController.UpdateTransform();
            //PlayingNPCUpdate();
            PlayingEnemyUpdate();
            PlayingTriggerUpdate();
            state.currentLevel.timer.Update();
        }

        private void PlayingTriggerUpdate()
        {
            foreach (var trigger in state.triggerList)
            {
                //trigger.UpdateTrigger(inputController);
            }
        }

        private void PlayingEnemyUpdate()
        {
            foreach (var enemy in state.enemyList)
            {
                enemy.UpdateBehaviour();
            }
        }

        //void PlayingNPCUpdate()
        //{
        //    foreach (Entities.BaseEntity npc in state.entityList)
        //    {
        //        npc.UpdateBehaviour();
        //    }
        //}
    }
}