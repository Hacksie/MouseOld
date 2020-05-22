
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


namespace HackedDesign
{
    public class GameManager : MonoBehaviour
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

        [Header("Lights")]
        [SerializeField] private Light2D globalLight = null;
        [SerializeField] private Color lightsDefault = Color.black;
        [SerializeField] private Color lightsWarn = Color.black;
        [SerializeField] private Color lightsAlert = Color.black;
        [SerializeField] private Color lightsBar = Color.black;

        [Header("Managers")]
        [SerializeField] private Story.ActionManager actionManager = null;
        [SerializeField] private StartMenuManager startMenuManager = null;
        [SerializeField] private SelectMenuManager selectMenuManager = null;
        [SerializeField] private Dialogue.NarrationManager narrationManager = null;
        [SerializeField] private MissionCompleteManager missionCompleteManager = null;
        [SerializeField] private LevelCompleteManager levelCompleteManager = null;

        [Header("UI")]
        [SerializeField] private UI.MobileInputUIPresenter mobileInputUI = null;
        [SerializeField] public UI.MainMenuPresenter mainMenuPresenter = null;
        [SerializeField] public UI.ActionConsolePresenter actionConsolePanel = null;
        [SerializeField] public UI.StartMenuPanelPresenter startMenuPanel = null;
        [SerializeField] public UI.SelectMenuPanelPresenter selectMenuPanel = null;
        [SerializeField] public UI.ActionPanelPresenter actionPanel = null;
        [SerializeField] public UI.InfoPanelPresenter infoPanel = null;
        [SerializeField] public UI.StashPanelPresenter stashPanel = null;
        [SerializeField] public UI.PsychPanelPresenter psychPanel = null;
        [SerializeField] public UI.TaskPanelPresenter taskPanel = null;
        [SerializeField] public UI.NarrationPanelPresenter narrationPanel = null;
        [SerializeField] public UI.LevelCompletePresenter levelCompletePresenter = null;
        [SerializeField] public UI.MissionCompletePresenter missionCompletePanel = null;
        [SerializeField] public UI.MinimapPresenter minimapPanel = null;
        [SerializeField] public UI.TitlecardPresenter titlecardPanel = null;
        [SerializeField] public UI.TimerPanelPresenter timerPanel = null;
        [SerializeField] public UI.WorldMapPresenter worldMapPanel = null;

        [Header("State")]
        [SerializeField] private GameData gameState;

        public GameData GameState { get { return gameState; } private set { gameState = value; } }
        public PolyNav.PolyNav2D PolyNav { get { return polyNav2D; } private set { polyNav2D = value; } }

        public static GameManager Instance { get; private set; }

        private IState currentState;

        public IState CurrentState
        {
            get
            {
                return currentState;
            }
            private set
            {
                if (currentState != null)
                {
                    currentState.End();
                }
                currentState = value;
                if (currentState != null)
                {
                    currentState.Start();
                }
            }
        }

        public PlayerController PlayerController
        {
            get
            {
                return playerController;
            }
            private set
            {
                playerController = value;
            }
        }

        public GameStateEnum state;

        private GameManager()
        {
            Instance = this;
        }

        private void Awake()
        {
            CheckBindings();
            Initialization();
            CurrentState = new MainMenuState(mainMenuPresenter);
        }

        private void Update()
        {
            CurrentState.Update();
        }

        private void LateUpdate()
        {
            CurrentState.LateUpdate();
        }

        public void LoadNewGame()
        {
            Logger.Log(this, "Loading new game");
            CurrentState = new LoadingState();
            state = GameStateEnum.InGame;
            GameState = new GameData(false);
            entityManager.Initialize();
            actionManager.Initialize(entityManager);
            GameState.CurrentLevel = levelGenerator.GenerateLevel(newGameLevel, 0, 0, 0);
            SceneInitialize();
        }

        public void LoadRandomGame(string template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            Logger.Log(this, "Loading random game");
            state = GameStateEnum.InGame;
            GameState = new GameData(true);
            GameState.CurrentLevel = levelGenerator.GenerateLevel(template, length, height, width, difficulty, enemies, traps);
            entityManager.Initialize();
            actionManager.Initialize(entityManager);
            SceneInitialize();
        }

        public void LoadNewLevel(string template)
        {
            Logger.Log(this, "Loading new level");
            state = GameStateEnum.InGame;
            GameState = new GameData(false);
            GameState.entityList.Clear();
            GameState.CurrentLevel = levelGenerator.GenerateLevel(template);
            entityManager.Initialize();
            actionManager.Initialize(entityManager);
            SceneInitialize();
        }

        public void EndGame()
        {
            Logger.Log(this, "End Game");
            CurrentState = new MainMenuState(mainMenuPresenter);
            state = GameStateEnum.MainMenu;
            levelRenderer.DestroyLevel();
            GameState.entityList.Clear();
            ShowPlayer(false);
            Time.timeScale = 0;
        }

        // public GameObject GetPlayer()
        // {
        //     return player;
        // }

        public void GameOver()
        {
            Logger.Log(this, "Game Over");
            GameState.PlayState = PlayStateEnum.GameOver;
        }

        public void SetStartMenu()
        {
            CurrentState = new StartMenuState();
            GameState.PlayState = PlayStateEnum.StartMenu;
        }

        public void SetSelectMenu()
        {
            CurrentState = new SelectMenuState();
            GameState.PlayState = PlayStateEnum.SelectMenu;
        }

        public void SetTitlecard()
        {
            CurrentState = new TitlecardState(this.titlecardPanel);
            GameState.PlayState = PlayStateEnum.Titlecard;
        }

        public void SetPlaying()
        {
            CurrentState = new PlayingState(this.playerController, this.actionManager);
            GameState.SetPlaying();
        }

        public void SetNarration()
        {
            CurrentState = new NarrationState(this.narrationPanel);
            GameState.PlayState = PlayStateEnum.Narration;
        }

        public void SetMissionComplete()
        {
            Logger.Log(this, "State set to MISSION COMPLETE");
            CurrentState = new MissionCompleteState();
            GameState.PlayState = PlayStateEnum.MissionComplete;
        }

        public void SetLevelComplete()
        {
            CurrentState = new LevelCompleteState();
            GameState.PlayState = PlayStateEnum.LevelComplete;
        }

        public void SetWorldMap()
        {
            Logger.Log(this, "State set to WORLDMAP");
            CurrentState = new WorldMapState(this.worldMapPanel);
            Time.timeScale = 0;
            GameState.PlayState = PlayStateEnum.Worldmap;
        }

        public void ToggleStart()
        {
            if (GameState.PlayState == PlayStateEnum.Playing)
            {
                SetStartMenu();
            }
            else if (GameState.PlayState == PlayStateEnum.StartMenu)
            {
                SetPlaying();
            }
        }

        public void ToggleSelect()
        {
            if (GameState.PlayState == PlayStateEnum.Playing)
            {
                SetSelectMenu();
            }
            else if (GameState.PlayState == PlayStateEnum.SelectMenu)
            {
                SetPlaying();
            }
        }


        public void SaveGame()
        {
            Logger.Log(this, "Saving state");
            string json = JsonUtility.ToJson(GameState);
            string path = Path.Combine(Application.persistentDataPath, $"SaveFile{GameState.GameSlot}.json");
            File.WriteAllText(path, json);
            Logger.Log(this, "Saved ", path);
        }

        public void SetLight(GlobalLightTypes light)
        {
            GameState.currentLight = light;
        }

        private void CheckBindings()
        {
            if (entityManager == null)
            {
                Logger.Log(this, "entityManager not set");
            }
        }

        private void Initialization()
        {
            Logger.Log(this, "Initialization");
            state = GameStateEnum.MainMenu;

            playerController = player.GetComponent<PlayerController>();

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
            startMenuPanel.Initialize(startMenuManager);
            selectMenuPanel.Initialize(selectMenuManager, infoPanel, taskPanel, stashPanel, psychPanel);
            narrationPanel.Initialize(narrationManager);
            actionPanel.Initialize(playerController);
            titlecardPanel.Initialize(actionManager);
            levelRenderer.Initialize(entityManager, levelParent, enemiesParent, polyNav2D);
            missionCompletePanel.Initialize(missionCompleteManager);
            levelCompletePresenter.Initialize(levelCompleteManager);
            worldMapPanel.Initialize();
            HideAllInGameUI();
            ShowPlayer(false);
        }

        private void HideAllInGameUI()
        {
            actionConsolePanel.Hide();
            narrationPanel.Hide();
            selectMenuPanel.Hide();
            startMenuPanel.Hide();
            missionCompletePanel.Hide();
            levelCompletePresenter.Hide();
            stashPanel.Hide();
            psychPanel.Hide();
            actionPanel.Hide();
            timerPanel.Hide();
            mobileInputUI.Hide();
            minimapPanel.Hide();
            titlecardPanel.Hide();
            worldMapPanel.Hide();
        }

        /// <summary>
        /// Run this each time the scene is changed
        /// </summary>
        private void SceneInitialize()
        {
            Logger.Log(this, "Scene initialization");
            ShowPlayer(true);
            SetLight(GlobalLightTypes.Default);

            levelRenderer.Render(GameState.CurrentLevel);
            levelRenderer.PopulateLevelDoors(GameState.CurrentLevel, GameState.doorList);
            levelRenderer.PopulateNPCSpawns(GameState.CurrentLevel, GameState.entityList);
            levelRenderer.PopulateEnemySpawns(GameState.CurrentLevel, GameState.entityList);
            minimapPanel.Initialize(GameState.CurrentLevel, PlayerController.transform);

            player.transform.position = GameState.CurrentLevel.ConvertLevelPosToWorld(GameState.CurrentLevel.playerSpawn.levelLocation) + GameState.CurrentLevel.playerSpawn.worldOffset;

            SceneTriggersInitialize();
            timerPanel.Initialize(GameState.CurrentLevel.timer);

            SetPlaying();

            if (!string.IsNullOrWhiteSpace(GameState.CurrentLevel.template.startingAction))
            {
                Story.ActionManager.instance.Invoke(GameState.CurrentLevel.template.startingAction);
            }
        }

        private void SceneTriggersInitialize()
        {
            GameState.triggerList.Clear();
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
                    GameState.triggerList.Add(trigger);
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

        private void ShowPlayer(bool flag)
        {
            if (player != null)
            {
                player.SetActive(flag);
            }
        }
    }
}