
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

        [Header("UI")]
        [SerializeField] private UI.MobileInputUIPresenter mobileInputUI = null;
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


        [Header("State")]
        [SerializeField] private GameData gameState;
        public GameData GameState { get { return gameState; } private set { gameState = value; } }
        public PolyNav.PolyNav2D PolyNav { get { return polyNav2D; } private set { polyNav2D = value; } }

        public static GameManager Instance { get; private set; }


        public GameStateEnum state;

        private GameManager()
        {
            Instance = this;
        }


        private void Awake()
        {
            CheckBindings();
            Initialization();
        }

        private void Update()
        {
            switch (state)
            {
                case GameStateEnum.MainMenu:
                    break;
                case GameStateEnum.InGame:
                    UpdateInGame();
                    break;
            }
        }

        private void LateUpdate()
        {
            switch (state)
            {
                case GameStateEnum.MainMenu:
                    mainMenu.Repaint();
                    break;
                case GameStateEnum.InGame:
                    mainMenu.Repaint();
                    RepaintInGameUI();
                    actionManager.UpdateBehaviour();
                    switch (GameState.PlayState)
                    {
                        case PlayStateEnum.Captured:
                        case PlayStateEnum.Dialogue:
                        case PlayStateEnum.GameOver:
                        case PlayStateEnum.MissionComplete:
                        case PlayStateEnum.LevelComplete:
                        case PlayStateEnum.SelectMenu:
                        case PlayStateEnum.StartMenu:
                        case PlayStateEnum.Worldmap:
                        case PlayStateEnum.Narration:
                        case PlayStateEnum.Playing:
                            AnimateDoors();
                            playerController.Animate();
                            AnimateEntity();
                            break;
                    }
                    break;
            }
        }

        public void LoadNewGame()
        {
            Logger.Log(this, "Loading new game");
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
            state = GameStateEnum.MainMenu;
            RepaintInGameUI();
            levelRenderer.DestroyLevel();
            GameState.entityList.Clear();
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
            SetLight(GlobalLightTypes.Default);

            levelRenderer.Render(GameState.CurrentLevel);
            levelRenderer.PopulateLevelDoors(GameState.CurrentLevel, GameState.doorList);
            levelRenderer.PopulateNPCSpawns(GameState.CurrentLevel, GameState.entityList);
            levelRenderer.PopulateEnemySpawns(GameState.CurrentLevel, GameState.entityList);
            minimapPanel.Initialize(GameState.CurrentLevel);

            player.transform.position = GameState.CurrentLevel.ConvertLevelPosToWorld(GameState.CurrentLevel.playerSpawn.levelLocation) + GameState.CurrentLevel.playerSpawn.worldOffset;



            SceneTriggersInitialize();
            timerPanel.Initialize(GameState.CurrentLevel.timer);

            SetPlaying();

            if (!string.IsNullOrWhiteSpace(GameState.CurrentLevel.template.startingAction))
            {
                Story.ActionManager.instance.Invoke(GameState.CurrentLevel.template.startingAction);
            }

            RepaintInGameUI();
        }

        public GameObject GetPlayer()
        {
            return player;
        }

        public void GameOver()
        {
            Logger.Log(this, "Game Over");
            GameState.PlayState = PlayStateEnum.GameOver;
        }

        public void SetTitlecard()
        {
            Logger.Log(this, "State set to TITLECARD");
            Time.timeScale = 0;
            GameState.PlayState = PlayStateEnum.Titlecard;
        }

        public void SetPlaying()
        {
            Logger.Log(this, "State set to PLAYING");
            Time.timeScale = 1;
            GameState.SetPlaying();
        }

        public void SetDialogue()
        {
            Logger.Log(this, "State set to DIALOGUE");
            Time.timeScale = 0;
            GameState.PlayState = PlayStateEnum.Dialogue;
        }

        public void SetNarration()
        {
            Logger.Log(this, "State set to NARRATION");
            Time.timeScale = 0;
            GameState.PlayState = PlayStateEnum.Narration;
            RepaintInGameUI();
        }

        public void SetMissionComplete()
        {
            Logger.Log(this, "State set to MISSION COMPLETE");
            Time.timeScale = 0;
            GameState.PlayState = PlayStateEnum.MissionComplete;
            RepaintInGameUI();
        }

        public void SetLevelComplete()
        {
            Logger.Log(this, "State set to LEVEL COMPLETE");
            Time.timeScale = 0;
            GameState.PlayState = PlayStateEnum.LevelComplete;
            RepaintInGameUI();
        }

        public void SetWorldMap()
        {
            Logger.Log(this, "State set to WORLDMAP");
            Time.timeScale = 0;
            GameState.PlayState = PlayStateEnum.Worldmap;
        }

        public void ToggleStart()
        {
            if (GameState.PlayState == PlayStateEnum.Playing)
            {
                GameState.PlayState = PlayStateEnum.StartMenu;
            }
            else if (GameState.PlayState == PlayStateEnum.StartMenu)
            {
                GameState.PlayState = PlayStateEnum.Playing;
            }
        }

        public void ToggleSelect()
        {
            if (GameState.PlayState == PlayStateEnum.Playing)
            {
                GameState.PlayState = PlayStateEnum.SelectMenu;
            }
            else if (GameState.PlayState == PlayStateEnum.SelectMenu)
            {
                GameState.PlayState = PlayStateEnum.Playing;
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

        public void UpdateLights()
        {
            /*
            switch (state.currentLight)
            {
                case GlobalLightTypes.Warn:
                    globalLight.color = lightsWarn;
                    break;
                case GlobalLightTypes.Alert:
                    globalLight.color = lightsAlert;
                    break;
                case GlobalLightTypes.Bar:
                    globalLight.color = lightsBar;
                    break;
                case GlobalLightTypes.Default:
                default:
                    globalLight.color = lightsDefault;
                    break;
            }*/
        }

        public void SetLight(GlobalLightTypes light)
        {
            GameState.currentLight = light;
        }


        public bool IsInGame()
        {
            return state == GameStateEnum.InGame;
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
            state = GameStateEnum.MainMenu;

            Logger.Log(this, "Initialization");

            // GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
            // GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);

            playerController = player.GetComponent<PlayerController>();

            //UI.SetActive(true);
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

        private void UpdateInGame()
        {
            switch (GameState.PlayState)
            {
                case PlayStateEnum.Playing:
                    PlayingUpdate();
                    break;
                case PlayStateEnum.StartMenu:
                    break;
                case PlayStateEnum.MissionComplete:
                    break;
                case PlayStateEnum.GameOver:
                    EndGame();
                    break;
                default:
                    break;
            }

            UpdateLights();
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

        private void RepaintInGameUI()
        {
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

        private void AnimateDoors()
        {
            foreach (var door in GameState.doorList)
            {
                door.UpdateAnimation();
            }
        }

        private void PlayingUpdate()
        {
            playerController.UpdateTransform();
            PlayingEntityUpdate();
            PlayingTriggerUpdate();
            GameState.CurrentLevel.timer.Update();
        }

        private void PlayingTriggerUpdate()
        {
            foreach (var trigger in GameState.triggerList)
            {
                //trigger.UpdateTrigger(inputController);
            }
        }

        private void PlayingEntityUpdate()
        {
            foreach (var entity in GameState.entityList)
            {
                entity.UpdateBehaviour();
            }
        }

        private void AnimateEntity()
        {
            foreach (var entity in GameState.entityList)
            {
                entity.Animate();
            }            
        }
    }
}