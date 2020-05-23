
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
        [SerializeField] private Level.LevelGenTemplate[] levelGenTemplates = null;

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
        [SerializeField] private Entities.EntityManager entityManager = null;
        [SerializeField] private Story.ActionManager actionManager = null;
        [SerializeField] private StartMenuManager startMenuManager = null;
        [SerializeField] private SelectMenuManager selectMenuManager = null;
        [SerializeField] private Dialogue.NarrationManager narrationManager = null;
        [SerializeField] private MissionCompleteManager missionCompleteManager = null;
        [SerializeField] private LevelCompleteManager levelCompleteManager = null;

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
        [SerializeField] private GameData gameState;


        #region Properties
        private IState currentState;

        public GameData GameState { get { return gameState; } private set { gameState = value; } }
        public PolyNav.PolyNav2D PolyNav { get { return polyNav2D; } private set { polyNav2D = value; } }

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
                    currentState.Start();
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

        private Level.LevelGenTemplate GetLevelGenTemplate(string template) => levelGenTemplates.FirstOrDefault(t => t.name == template);

        public void LoadNewGame()
        {
            Logger.Log(this, "Loading new game");
            CurrentState = new LoadingState();
            GameState = new GameData(false);
            entityManager.Initialize();
            actionManager.Initialize(this.narrationManager);

            var levelTemplate = GetLevelGenTemplate(newGameLevel);
            GameState.CurrentLevel = Level.LevelGenerator.Generate(levelTemplate, 0,0,0);
            GameState.CurrentLevel.Print();
            SceneInitialize();
        }

        public void LoadRandomGame(string templateName, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            Logger.Log(this, "Loading random game");
            GameState = new GameData(true);

            var levelTemplate = GetLevelGenTemplate(templateName);
            GameState.CurrentLevel = Level.LevelGenerator.Generate(levelTemplate, length, height, width, difficulty, enemies, traps);
            GameState.CurrentLevel.Print();
            entityManager.Initialize();
            actionManager.Initialize(this.narrationManager);
            SceneInitialize();
        }

        public void LoadNewLevel(string templateName)
        {
            Logger.Log(this, "Loading new level");
            GameState = new GameData(false);
            GameState.entityList.Clear();

            var levelTemplate = GetLevelGenTemplate(templateName);
            GameState.CurrentLevel = Level.LevelGenerator.Generate(levelTemplate);
            GameState.CurrentLevel.Print();
            entityManager.Initialize();
            actionManager.Initialize(this.narrationManager);
            SceneInitialize();
        }

        public void EndGame()
        {
            Logger.Log(this, "End Game");
            CurrentState = new MainMenuState(mainMenuPresenter);
            levelRenderer.DestroyLevel();
            GameState.entityList.Clear();
            playerController.Hide();
        }

        public void IncreaseAlert() => GameState.CurrentLevel.alertLevel++;

        public void GameOver() => CurrentState = new GameOverState();
        public void SetStartMenu() => CurrentState = new StartMenuState(this.startMenuPanel);
        public void SetSelectMenu() => CurrentState = new SelectMenuState(this.selectMenuPanel);
        public void SetTitlecard() => CurrentState = new TitlecardState(this.titlecardPanel);
        public void SetPlaying() => CurrentState = new PlayingState(this.playerController, this.actionManager, this.actionConsolePanel, this.actionPanel, this.timerPanel, this.minimapPanel);
        public void SetNarration() => CurrentState = new NarrationState(this.narrationPanel);
        public void SetMissionComplete() => CurrentState = new MissionCompleteState(this.missionCompletePanel);
        public void SetLevelComplete() => CurrentState = new LevelCompleteState(this.levelCompletePresenter);
        public void SetWorldMap() => CurrentState = new WorldMapState(this.worldMapPanel);

        public void ToggleStart()
        {
            if (CurrentState is StartMenuState)
            {
                CurrentState = new PlayingState(this.playerController, this.actionManager, this.actionConsolePanel, this.actionPanel, this.timerPanel, this.minimapPanel);
            }
            else if (CurrentState is PlayingState)
            {
                CurrentState = new StartMenuState(this.startMenuPanel);
            }
        }

        public void ToggleSelect()
        {
            if (CurrentState is SelectMenuState)
            {
                CurrentState = new PlayingState(this.playerController, this.actionManager, this.actionConsolePanel, this.actionPanel, this.timerPanel, this.minimapPanel);
            }
            else if (CurrentState is PlayingState)
            {
                CurrentState = new SelectMenuState(this.selectMenuPanel);
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

        public void SetLight(GlobalLightTypes light) => GameState.currentLight = light;

        private void CheckBindings()
        {
            this.playerController = this.playerController ?? FindObjectOfType<PlayerController>();
            this.entityManager = this.entityManager ?? FindObjectOfType<Entities.EntityManager>();
            this.actionManager = this.actionManager ?? FindObjectOfType<Story.ActionManager>();
            this.startMenuManager = this.startMenuManager ?? FindObjectOfType<StartMenuManager>();
            this.selectMenuManager = this.selectMenuManager ?? FindObjectOfType<SelectMenuManager>();
            this.narrationManager = this.narrationManager ?? FindObjectOfType<Dialogue.NarrationManager>();
            this.missionCompleteManager = this.missionCompleteManager ?? FindObjectOfType<MissionCompleteManager>();
            this.levelCompleteManager = this.levelCompleteManager ?? FindObjectOfType<LevelCompleteManager>();
        }

        private void Initialization()
        {
            Logger.Log(this, "Initialization");
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
            levelRenderer.Initialize(playerController, entityManager, levelParent, enemiesParent, polyNav2D);
            missionCompletePanel.Initialize(missionCompleteManager);
            levelCompletePresenter.Initialize(levelCompleteManager);
            worldMapPanel.Initialize();
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
        }

        /// <summary>
        /// Run this each time the scene is changed
        /// </summary>
        private void SceneInitialize()
        {
            Logger.Log(this, "Scene initialization");
            SetLight(GlobalLightTypes.Default);
            RenderLevel();
            playerController.Move(GameState.CurrentLevel.ConvertLevelPosToWorld(GameState.CurrentLevel.playerSpawn.levelLocation) + GameState.CurrentLevel.playerSpawn.worldOffset);
            minimapPanel.Initialize(GameState.CurrentLevel, playerController.transform);
            SceneTriggersInitialize();
            timerPanel.Initialize(GameState.CurrentLevel.timer);
            SetPlaying();

            playerController.Show();
            this.actionManager.Invoke(GameState.CurrentLevel.template.startingAction);
        }

        private void RenderLevel()
        {
            this.levelRenderer.Render(GameState.CurrentLevel);
            this.levelRenderer.PopulateLevelDoors(GameState.CurrentLevel, GameState.doorList);
            this.levelRenderer.PopulateNPCSpawns(GameState.CurrentLevel, GameState.entityList);
            this.levelRenderer.PopulateEnemySpawns(GameState.CurrentLevel, GameState.entityList);
            this.levelRenderer.PopulateTrapSpawns(GameState.CurrentLevel, GameState.entityList);
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

                BaseTrigger trigger = triggerObject.GetComponent<BaseTrigger>();
                if (trigger != null)
                {
                    GameState.triggerList.Add(trigger);
                    trigger.Initialize();
                }
            }
        }
    }
}