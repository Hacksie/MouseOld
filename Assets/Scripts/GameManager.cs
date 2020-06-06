
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


namespace HackedDesign
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game")]
        [SerializeField] private PlayerController playerController = null;

        [Header("Level")]
        [SerializeField] private Level.LevelRenderer levelRenderer = null;
        [SerializeField] private GameObject levelParent = null;
        [SerializeField] private GameObject enemiesParent = null;
        [SerializeField] private PolyNav.PolyNav2D polyNav2D = null;

        [Header("Lights")]
        [SerializeField] private Light2D globalLight = null;
        [SerializeField] private Color lightsDefault = Color.black;
        [SerializeField] private Color lightsWarn = Color.black;
        [SerializeField] private Color lightsAlert = Color.black;

        [Header("Managers")]
        [SerializeField] private EntityManager entityManager = null;
        [SerializeField] private Story.SceneManager sceneManager = null;
        [SerializeField] private StartMenuManager startMenuManager = new StartMenuManager();
        [SerializeField] private SelectMenuManager selectMenuManager = new SelectMenuManager();
        [SerializeField] private Dialogue.NarrationManager narrationManager = null;
        [SerializeField] private MissionCompleteManager missionCompleteManager = new MissionCompleteManager();
        [SerializeField] private LevelCompleteManager levelCompleteManager = new LevelCompleteManager();
        [SerializeField] private WorldMapManager worldMapManager = null;

        [Header("UI")]
        [SerializeField] private UI.MobileInputUIPresenter mobileInputUI = null;
        [SerializeField] private UI.MainMenuPresenter mainMenuPresenter = null;
        [SerializeField] private UI.ActionConsolePresenter actionConsolePanel = null;
        [SerializeField] private UI.StartMenuPanelPresenter startMenuPanel = null;
        [SerializeField] private UI.SelectMenuPanelPresenter selectMenuPanel = null;
        [SerializeField] private UI.ActionPanelPresenter actionPanel = null;
        [SerializeField] private UI.InfoPanelPresenter infoPanel = null;
        [SerializeField] private UI.StashPanelPresenter stashPanel = null;
        [SerializeField] private UI.PsychPanelPresenter psychPanel = null;
        [SerializeField] private UI.TaskPanelPresenter taskPanel = null;
        [SerializeField] private UI.NarrationPanelPresenter narrationPanel = null;
        [SerializeField] private UI.LevelCompletePresenter levelCompletePresenter = null;
        [SerializeField] private UI.MissionCompletePresenter missionCompletePanel = null;
        [SerializeField] private UI.MinimapPresenter minimapPanel = null;
        [SerializeField] private UI.TitlecardPresenter titlecardPanel = null;
        [SerializeField] private UI.TimerPanelPresenter timerPanel = null;
        [SerializeField] private UI.WorldMapPresenter worldMapPanel = null;

        [Header("State")]
        [SerializeField] private GameData gameData;


        #region Properties
        private IState currentState;

        public GameData Data { get { return gameData; } private set { gameData = value; } }
        public PolyNav.PolyNav2D PolyNav { get { return polyNav2D; } private set { polyNav2D = value; } }

        public PlayerController Player { get { return playerController;} private set { playerController = value; }}

        public static GameManager Instance { get; private set; }

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
                    currentState.Begin();
                }
            }
        }

        #endregion

        private GameManager() => Instance = this;

        private void Awake()
        {
            CheckBindings();
            Initialization();
            CurrentState = new MainMenuState(mainMenuPresenter);
        }

        private void Update() => CurrentState.Update();
        private void LateUpdate() => CurrentState.LateUpdate();



        public void LoadNewGame()
        {
            Logger.Log(this, "Loading new game");
            //SetLoading();
            Data = new GameData(false);
            entityManager.Initialize();
            sceneManager.Initialize();
            sceneManager.CurrentScene = new Story.PreludeScene(Story.SceneManager.Instance.NewGameLevelDefault, 0, 0, 0, 0, 0, 0);
        }

        public void LoadRandomGame(string templateName, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            Logger.Log(this, "Loading random game");
            //SetLoading();
            Data = new GameData(true);
            entityManager.Initialize();
            sceneManager.Initialize();

            sceneManager.CurrentScene = new Story.RandomScene(templateName, length, height, width, difficulty, enemies, traps);
            //SceneInitialize();
        }

        // public void LoadNewLevel(string templateName)
        // {
        //     Logger.Log(this, "Loading new level");
        //     Data = new GameData(false);
        //     Data.entityList.Clear();

        //     var levelTemplate = GetLevelGenTemplate(templateName);
        //     Data.CurrentLevel = Level.LevelGenerator.Generate(levelTemplate);
        //     Data.CurrentLevel.Print();
        //     entityManager.Initialize();
        //     actionManager.Initialize();
        //     SceneInitialize();
        // }

        public void EndGame()
        {
            Logger.Log(this, "End Game");
            CurrentState = new MainMenuState(mainMenuPresenter);
            levelRenderer.DestroyLevel();
            Data.entityList.Clear();
            playerController.Hide();
        }

        public void IncreaseAlert() => Data.CurrentLevel.alertLevel++;

        public void GameOver() => CurrentState = new GameOverState();
        public void SetStartMenu() => CurrentState = new StartMenuState(this.startMenuPanel);
        public void SetSelectMenu() => CurrentState = new SelectMenuState(this.selectMenuPanel, this.selectMenuManager);
        public void SetSelectMenu(SelectMenuSubState subState) => CurrentState = new SelectMenuState(this.selectMenuPanel, this.selectMenuManager, subState);
        public void SetLoading() => CurrentState = new LoadingState(this.titlecardPanel);
        //public void SetTitlecard() => CurrentState = new TitlecardState(this.titlecardPanel);
        public void SetPlaying() => CurrentState = new PlayingState(this.playerController, this.sceneManager, this.actionConsolePanel, this.actionPanel, this.timerPanel, this.minimapPanel);
        public void SetNarration() => CurrentState = new NarrationState(this.narrationPanel);
        public void SetMissionComplete() => CurrentState = new MissionCompleteState(this.missionCompletePanel);
        public void SetLevelComplete() => CurrentState = new LevelCompleteState(this.levelCompletePresenter);
        public void SetWorldMap() => CurrentState = new WorldMapState(this.worldMapPanel);

        public void SaveGame()
        {
            Logger.Log(this, "Saving state");
            string json = JsonUtility.ToJson(Data);
            string path = Path.Combine(Application.persistentDataPath, $"SaveFile{Data.GameSlot}.json");
            File.WriteAllText(path, json);
            Logger.Log(this, "Saved ", path);
        }

        public void SetLight(GlobalLightTypes light)
        {
            switch (light)
            {
                case GlobalLightTypes.Warn:
                    globalLight.color = lightsWarn;
                    break;
                case GlobalLightTypes.Alert:
                    globalLight.color = lightsAlert;
                    break;

                default:
                case GlobalLightTypes.Default:
                    globalLight.color = lightsDefault;
                    break;
            }
        }
        //Data.currentLight = light;

        private void CheckBindings()
        {
            this.playerController = this.playerController ?? FindObjectOfType<PlayerController>();
            this.entityManager = this.entityManager ?? FindObjectOfType<EntityManager>();
            this.sceneManager = this.sceneManager ?? FindObjectOfType<Story.SceneManager>();
            this.narrationManager = this.narrationManager ?? FindObjectOfType<Dialogue.NarrationManager>();
            this.worldMapManager = this.worldMapManager ?? FindObjectOfType<WorldMapManager>();
        }

        private void Initialization()
        {
            Logger.Log(this, "Initialization");
            narrationManager.Initialize();
            missionCompleteManager.Initialize(this.sceneManager);
            levelCompleteManager.Initialize(this.sceneManager);
            worldMapManager.Initialize();

            mobileInputUI.Initialize();
            actionConsolePanel.Initialize(this.sceneManager);
            infoPanel.Initialize(this.selectMenuManager);
            stashPanel.Initialize(this.selectMenuManager);
            psychPanel.Initialize(this.selectMenuManager);
            taskPanel.Initialize(this.selectMenuManager);
            startMenuPanel.Initialize(this.startMenuManager);
            selectMenuPanel.Initialize(this.selectMenuManager, infoPanel, taskPanel, stashPanel, psychPanel);
            narrationPanel.Initialize(this.narrationManager);
            actionPanel.Initialize(this.playerController);
            titlecardPanel.Initialize(this.sceneManager);
            missionCompletePanel.Initialize(this.missionCompleteManager);
            levelCompletePresenter.Initialize(this.levelCompleteManager);
            worldMapPanel.Initialize(this.worldMapManager, this.sceneManager);
            levelRenderer.Initialize(this.playerController, this.entityManager, this.levelParent, this.enemiesParent, this.polyNav2D);
            HideAllInGameUI();
            playerController.Hide();
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
            taskPanel.Hide();
            infoPanel.Hide();
        }

        /// <summary>
        /// Run this each time the scene is changed
        /// </summary>
        public void SceneInitialize()
        {
            Logger.Log(this, "Scene initialization");
            //SetLight(GlobalLightTypes.Default);
            RenderLevel();
            SetLight(Data.CurrentLevel.template.startingLight);
            playerController.Move(Data.CurrentLevel.ConvertLevelPosToWorld(Data.CurrentLevel.playerSpawn.levelLocation) + Data.CurrentLevel.playerSpawn.worldOffset);
            minimapPanel.Initialize(Data.CurrentLevel, playerController.transform);
            SceneTriggersInitialize();
            timerPanel.Initialize(Data.CurrentLevel.timer);
            playerController.Show();
            GameManager.Instance.SaveGame();
            //SetPlaying();
            //this.actionManager.Invoke(GameState.CurrentLevel.template.startingAction);
        }

        private void RenderLevel()
        {
            this.levelRenderer.Render(Data.CurrentLevel);
            this.levelRenderer.PopulateLevelDoors(Data.CurrentLevel, Data.doorList);
            this.levelRenderer.PopulateNPCSpawns(Data.CurrentLevel, Data.entityList);
            this.levelRenderer.PopulateEnemySpawns(Data.CurrentLevel, Data.entityList);
            this.levelRenderer.PopulateTrapSpawns(Data.CurrentLevel, Data.entityList);
        }

        private void SceneTriggersInitialize()
        {
            Data.triggerList.Clear();
            Logger.Log(this, "Initializing triggers");

            foreach (var triggerObject in GameObject.FindGameObjectsWithTag(Tags.TRIGGER))
            {
                if (!triggerObject.activeInHierarchy)
                {
                    continue;
                }

                BaseTrigger trigger = triggerObject.GetComponent<BaseTrigger>();
                if (trigger != null)
                {
                    Data.triggerList.Add(trigger);
                    trigger.Initialize();
                }
            }
        }
    }
}