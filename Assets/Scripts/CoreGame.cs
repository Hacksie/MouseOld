namespace HackedDesign
{
    using System.IO;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Experimental.Rendering.Universal;

    public class CoreGame : MonoBehaviour
    {
        [Header("Test Flags")]
        [SerializeField] private RuntimePlatform testPlatform = RuntimePlatform.WindowsEditor;
        [SerializeField] private bool testPlatformFlag = false;

        [Header("Game")]
        [SerializeField] private Entities.EntityManager entityManager = null;
        [SerializeField] private AudioSource denied;

        [Header("Player")]
        [SerializeField] private GameObject player = null;
        private PlayerController playerController = null;

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

        [Header("Mobile UI")]
        [SerializeField] private Input.MobileInputUIPresenter mobileInputUI = null;

        [Header("UI")]
        [SerializeField] private GameObject UI = null;
        [SerializeField] private CursorPresenter cursorPresenter = null;
        [SerializeField] private MainMenuPresenter mainMenu = null;
        [SerializeField] private Story.ActionManager actionManager = null;
        [SerializeField] private ActionConsolePresenter actionConsolePanel = null;
        [SerializeField] private StartMenuManager startMenuManager = null;
        [SerializeField] private StartMenuPanelPresenter startMenuPanel = null;
        [SerializeField] private SelectMenuManager selectMenuManager = null;
        [SerializeField] private SelectMenuPanelPresenter selectMenuPanel = null;
        [SerializeField] private ActionPanelPresenter actionPanel = null;
        [SerializeField] private Story.InfoManager infoManager = null;
        [SerializeField] private Story.InfoPanelPresenter infoPanel = null;
        [SerializeField] private StashPanelPresenter stashPanel = null;
        [SerializeField] private PsychPanelPresenter psychPanel = null;
        [SerializeField] private Story.TaskDefinitionManager taskManager = null;
        [SerializeField] private Story.TaskPanelPresenter taskPanel = null;
        [SerializeField] private Dialogue.NarrationManager narrationManager = null;
        [SerializeField] private Dialogue.NarrationPanelPresenter narrationPanel = null;
        [SerializeField] private MissionCompleteManager missionCompleteManager = null;
        [SerializeField] private LevelCompleteManager levelCompleteManager = null;
        [SerializeField] private LevelCompletePresenter levelCompletePresenter = null;
        [SerializeField] private MissionCompletePresenter missionCompletePanel = null;
        [SerializeField] private Level.MinimapPresenter minimapPanel = null;
        [SerializeField] private TitlecardPresenter titlecardPanel = null;
        [SerializeField] private Level.LevelMapPanelPresenter levelMapPanel = null;
        [SerializeField] private TimerPanelPresenter timerPanel = null;
        [SerializeField] private Level.WorldMapPresenter worldMapPanel = null;

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
                    break;
            }
        }

        void FixedUpdate()
        {
            switch (state.state)
            {
                case GameState.GameStateEnum.PLAYING:
                    PlayingFixedUpdate();
                    break;
            }
        }

        public void LoadNewGame()
        {
            Logger.Log(name, "Loading new game");
            state.state = GameState.GameStateEnum.LOADING;
            entityManager.Initialize();
            actionManager.Initialize(entityManager, taskManager);
            state.currentLevel = levelGenerator.GenerateLevel(newGameLevel, 0, 0, 0);
            state.isRandom = false;
            state.player = new GameState.PlayerState();
            SceneInitialize();
        }

        public void LoadRandomGame(string template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            Logger.Log(name, "Loading random game");
            state.state = GameState.GameStateEnum.LOADING;
            state.currentLevel = levelGenerator.GenerateLevel(template, length, height, width, difficulty, enemies, traps);
            state.isRandom = true;
            state.player = new GameState.PlayerState();
            entityManager.Initialize();
            actionManager.Initialize(entityManager, taskManager);
            SceneInitialize();
        }

        public void LoadNewLevel(string template)
        {
            Logger.Log(name, "Loading new level");
            state.state = GameState.GameStateEnum.LOADING;
            state.entityList.Clear();
            state.currentLevel = levelGenerator.GenerateLevel(template);
            entityManager.Initialize();
            actionManager.Initialize(entityManager, taskManager);
            SceneInitialize();
        }

        public void EndGame()
        {
            Logger.Log(name, "End Game");
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
            Logger.Log(name, "Scene initialization");
            ShowPlayer(true);
            SetLight(GameState.GlobalLightTypes.Default);

            levelRenderer.Render(state.currentLevel);

            levelRenderer.PopulateLevelDoors(state.currentLevel, state.doorList);
            levelRenderer.PopulateNPCSpawns(state.currentLevel, state.entityList); 
            levelRenderer.PopulateEnemySpawns(state.currentLevel, state.enemyList);
            //this.state.entityList.AddRange(levelRenderer.PopulateTrapSpawns(this.state.currentLevel));

            levelMapPanel.Initialize(selectMenuManager, state.currentLevel);
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
            Logger.Log(name, "Game Over");
            state.state = GameState.GameStateEnum.GAMEOVER;
        }

        public void SetTitlecard()
        {
            Logger.Log(name, "State set to TITLECARD");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.TITLECARD;
        }

        public void SetPlaying()
        {
            Logger.Log(name, "State set to PLAYING");
            Time.timeScale = 1;
            state.state = GameState.GameStateEnum.PLAYING;
        }

        public void SetDialogue()
        {
            Logger.Log(name, "State set to DIALOGUE");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.DIALOGUE;
        }

        public void SetNarration()
        {
            Logger.Log(name, "State set to NARRATION");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.NARRATION;
            RepaintAllUI();
        }

        public void SetMissionComplete()
        {
            Logger.Log(name, "State set to MISSION COMPLETE");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.MISSIONCOMPLETE;
            RepaintAllUI();
        }

        public void SetLevelComplete()
        {
            Logger.Log(name, "State set to LEVEL COMPLETE");
            Time.timeScale = 0;
            state.state = GameState.GameStateEnum.LEVELCOMPLETE;
            RepaintAllUI();
        }

        public void SetWorldMap()
        {
            Logger.Log(name, "State set to WORLDMAP");
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
            Logger.Log(name, "Set alert");
            state.alertTrap = trap;
            roomAlert.transform.position = trap.transform.position;
            roomAlert.SetActive(true);
        }

        public void ClearAlert()
        {
            Logger.Log(name, "Clear alert");
            state.alertTrap = null;
            roomAlert.SetActive(false);
        }

        public void SaveGame()
        {
            Logger.Log(name, "Saving state");
            string json = JsonUtility.ToJson(state);
            string path = Path.Combine(Application.persistentDataPath, $"SaveFile{state.gameSlot}.json");
            File.WriteAllText(path, json);
            Logger.Log(name, "Saved ", path);
        }

        public void UpdateLights()
        {
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
            }
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
                Logger.Log(name, "entityManager not set");
            }
            if (cursorPresenter == null)
            {
                Logger.Log(name, "cursorPresenter not set");
            }
            if (levelMapPanel == null)
            {
                Logger.Log(name, "levelMapPanel not set");
            }
        }

        /// <summary>
        /// Run only once
        /// </summary>
        private void Initialization()
        {
            state = new GameState.GameState();
            state.state = GameState.GameStateEnum.MAINMENU;
            Logger.Log(name, "Initialization");

            GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
            GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);

            playerController = player.GetComponent<PlayerController>();

            UI.SetActive(true);
            SetPlatformInput();
            narrationManager.Initialize();
            infoManager.Initialize();
            taskManager.Initialize();
            missionCompleteManager.Initialize(actionManager);
            levelCompleteManager.Initialize(actionManager);
            mobileInputUI.Initialize();
            actionConsolePanel.Initialize(actionManager);
            infoPanel.Initialize(infoManager, selectMenuManager);
            stashPanel.Initialize(selectMenuManager);
            psychPanel.Initialize(selectMenuManager);
            taskPanel.Initialize(selectMenuManager);
            startMenuPanel.Initialize(state, startMenuManager);
            selectMenuPanel.Initialize(selectMenuManager, infoPanel, taskPanel, stashPanel, psychPanel, levelMapPanel);
            narrationPanel.Initialize(narrationManager, infoManager);
            actionPanel.Initialize(playerController);
            titlecardPanel.Initialize(actionManager);
            levelRenderer.Initialize(entityManager, infoManager, levelParent, enemiesParent, polyNav2D);
            missionCompletePanel.Initialize(missionCompleteManager);
            levelCompletePresenter.Initialize(levelCompleteManager);
            worldMapPanel.Initialize();
            RepaintAllUI();
            ShowPlayer(false);
        }

        private void SceneTriggersInitialize()
        {
            state.triggerList.Clear();
            Logger.Log(name, "initializing triggers");

            foreach (var triggerObject in GameObject.FindGameObjectsWithTag(TagManager.TRIGGER))
            {
                if (!triggerObject.activeInHierarchy)
                {
                    continue;
                }

                Logger.Log(name, "initializing trigger - ", triggerObject.name);
                Triggers.ITrigger trigger = triggerObject.GetComponent<Triggers.ITrigger>();
                if (trigger != null)
                {
                    state.triggerList.Add(trigger);
                    trigger.Initialize();
                }
            }
        }

        private void SetPlatformInput()
        {
            /*
            switch (testPlatformFlag ? testPlatform : Application.platform)
            {
                case RuntimePlatform.Android:
                    Logger.Log(this.name, "input platform Android");
                    inputController = new Input.AndroidInputController(mobileInputUI);
                    break;
                default:
                    Logger.Log(this.name, "input platform Default");
                    inputController = new Input.DesktopInputController();
                    break;
            }*/
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
            levelMapPanel.Repaint();
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
            //playerController.UpdateMovement(inputController);
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

        private void PlayingFixedUpdate()
        {
            //playerController.UpdateTransform();
        }
    }
}