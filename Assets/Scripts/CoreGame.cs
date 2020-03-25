using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using System.Collections.Generic;

namespace HackedDesign
{
    public class CoreGame : MonoBehaviour
    {
        public static CoreGame Instance { get; private set; }

        public State.GameState state;

        //private Input.IInputController inputController;

        [Header("Test Flags")]
        [SerializeField] private RuntimePlatform testPlatform = RuntimePlatform.WindowsEditor;
        [SerializeField] private bool testPlatformFlag = false;

        [Header("Game")]
        [SerializeField] private Entities.EntityManager entityManager = null;
        [SerializeField] public AudioSource denied;

        [Header("Player")]
        [SerializeField] private GameObject player = null;
        private PlayerController playerController = null;

        [Header("Level")]
        [SerializeField]
        private Level.LevelGenerator levelGenerator = null;
        [SerializeField]
        private Level.LevelRenderer levelRenderer = null;
        [SerializeField]
        private GameObject levelParent = null;
        [SerializeField]
        private GameObject npcParent = null;
        [SerializeField]
        private PolyNav.PolyNav2D polyNav2D = null;
        [SerializeField]
        private string newGameLevel = "Olivia's Room";

        [SerializeField]
        private GameObject roomAlertPrefab = null;
        [SerializeField]
        private GameObject roomAlert = null;

        [Header("Lights")]
        public UnityEngine.Experimental.Rendering.Universal.Light2D globalLight;
        public Color lightsDefault;
        public Color lightsWarn;
        public Color lightsAlert;
        public Color lightsBar;

        [Header("Mobile UI")]
        [SerializeField]
        private Input.MobileInputUIPresenter mobileInputUI = null;

        [Header("UI")]
        [SerializeField]
        private GameObject UI = null;
        [SerializeField]
        private CursorPresenter cursorPresenter = null;
        [SerializeField]
        private MainMenuPresenter mainMenu = null;
        [SerializeField]
        private Story.ActionManager actionManager = null;
        [SerializeField]
        private ActionConsolePresenter actionConsolePanel = null;
        [SerializeField]
        private StartMenuManager startMenuManager = null;
        [SerializeField]
        private StartMenuPanelPresenter startMenuPanel = null;
        [SerializeField]
        private SelectMenuManager selectMenuManager = null;
        [SerializeField]
        private SelectMenuPanelPresenter selectMenuPanel = null;
        [SerializeField]
        private ActionPanelPresenter actionPanel = null;
        [SerializeField]
        private Story.InfoManager infoManager = null;
        [SerializeField]
        private Story.InfoPanelPresenter infoPanel = null;
        [SerializeField]
        private StashPanelPresenter stashPanel = null;
        [SerializeField]
        private PsychPanelPresenter psychPanel = null;
        [SerializeField]
        private Story.TaskDefinitionManager taskManager = null;
        [SerializeField]
        private Story.TaskPanelPresenter taskPanel = null;
        [SerializeField]
        private Dialogue.NarrationManager narrationManager = null;
        [SerializeField]
        private Dialogue.NarrationPanelPresenter narrationPanel = null;
        [SerializeField]
        private MissionCompleteManager missionCompleteManager = null;
        [SerializeField]
        private LevelCompleteManager levelCompleteManager = null;
        [SerializeField]
        private LevelCompletePresenter levelCompletePresenter = null;
        [SerializeField]
        private MissionCompletePresenter missionCompletePanel = null;
        [SerializeField]
        private Level.MinimapPresenter minimapPanel = null;
        [SerializeField]
        private TitlecardPresenter titlecardPanel = null;
        [SerializeField]
        private Level.LevelMapPanelPresenter levelMapPanel = null;
        [SerializeField]
        private WorldMapManager worldMapManager = null;
        [SerializeField]
        private WorldMapPanelPresenter worldMapPanelPresenter = null;

        [SerializeField]
        private TimerPanelPresenter timerPanel = null;

        CoreGame()
        {
            Instance = this;
        }

        /// <summary>
        /// Run in editor
        /// </summary>
        void Start()
        {
            CheckBindings();
            Initialization();
        }

        void CheckBindings()
        {
            if (entityManager == null)
            {
                Debug.LogError(this.name + ": entityManager not set");
            }
            if (cursorPresenter == null)
            {
                Debug.LogError("cursorPresenter not set");
            }

            if (levelMapPanel == null)
            {
                Debug.LogError("levelMapPanel not set");
            }
        }

        /// <summary>
        /// Run only once
        /// </summary>
        public void Initialization()
        {
            state.state = State.GameStateEnum.MAINMENU;
            Logger.Log(this.name, "Initialization");

            GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
            GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);

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
            actionPanel.Initialize();
            titlecardPanel.Initialize(actionManager);
            levelRenderer.Initialize(entityManager, infoManager, levelParent, npcParent, polyNav2D);
            missionCompletePanel.Initialize(missionCompleteManager);
            levelCompletePresenter.Initialize(levelCompleteManager);
            worldMapPanelPresenter.Initialize(worldMapManager, infoManager);

            RepaintAllUI();
            ShowPlayer(false);
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

        public void LoadNewGame()
        {
            Logger.Log(this.name, "loading new game");
            state.state = State.GameStateEnum.LOADING;
            entityManager.Initialize(npcParent);
            actionManager.Initialize(entityManager, taskManager);
            state.currentLevel = levelGenerator.GenerateLevel(newGameLevel, 0, 0, 0);
            state.isRandom = false;
            state.player = new State.PlayerState();
            CoreGame.Instance.SceneInitialize();
        }

        public void LoadRandomGame(string template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            Logger.Log(this.name, "loading random game");
            state.state = State.GameStateEnum.LOADING;
            state.currentLevel = levelGenerator.GenerateLevel(template, length, height, width, difficulty, enemies, traps);
            state.isRandom = true;
            state.player = new State.PlayerState();
            entityManager.Initialize(npcParent);
            actionManager.Initialize(entityManager, taskManager);
            SceneInitialize();
        }

        public void LoadNewLevel(string template)
        {
            Logger.Log(this.name, "loading new level");
            state.state = State.GameStateEnum.LOADING;
            state.entityList.Clear();
            state.currentLevel = levelGenerator.GenerateLevel(template);

            entityManager.Initialize(npcParent);
            actionManager.Initialize(entityManager, taskManager);
            SceneInitialize();
        }

        public void EndGame()
        {
            Logger.Log(this.name, "End Game");
            state.state = State.GameStateEnum.MAINMENU;
            RepaintAllUI();
            levelRenderer.DestroyLevel();
            state.entityList.Clear();
            ShowPlayer(false);

            Time.timeScale = 0;
        }

        private void ShowPlayer(bool flag)
        {
            if (player != null)
            {
                player.SetActive(flag);
            }
        }

        /// <summary>
        /// Run this each time the scene is changed
        /// </summary>
        public void SceneInitialize()
        {
            Logger.Log(this.name, "scene initialization");
            ShowPlayer(true);
            SetLight(State.GlobalLightTypes.Default);

            levelRenderer.Render(this.state.currentLevel);

            this.state.doorList.AddRange(levelRenderer.PopulateLevelDoors(this.state.currentLevel));

            
            levelRenderer.PopulateNPCSpawns(this.state.currentLevel, this.state.entityList); 
            levelRenderer.PopulateEnemySpawns(this.state.currentLevel, this.state.enemyList);
            //this.state.entityList.AddRange(levelRenderer.PopulateTrapSpawns(this.state.currentLevel));

            levelMapPanel.Initialize(selectMenuManager, state.currentLevel);
            minimapPanel.Initialize(state.currentLevel);

            player.transform.position = state.currentLevel.ConvertLevelPosToWorld(state.currentLevel.playerSpawn.levelLocation) + state.currentLevel.playerSpawn.worldOffset;

            playerController = player.GetComponent<PlayerController>();

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

        void RepaintAllUI()
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
            worldMapPanelPresenter.Repaint();
        }

        // FIXME: There's probably a better way to do this these days
        void SceneTriggersInitialize()
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

        public GameObject GetPlayer()
        {
            return player;
        }

        public void GameOver()
        {
            Logger.Log(name, "GameOver");
            state.state = State.GameStateEnum.GAMEOVER;
        }

        public void SetTitlecard()
        {
            Logger.Log(name, "state set to TITLECARD");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.TITLECARD;
        }

        public void SetPlaying()
        {
            Logger.Log(name, "state set to PLAYING");
            Time.timeScale = 1;
            state.state = State.GameStateEnum.PLAYING;
        }

        public void SetDialogue()
        {
            Logger.Log(name, "state set to DIALOGUE");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.DIALOGUE;
        }

        public void SetNarration()
        {
            Logger.Log(name, "state set to NARRATION");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.NARRATION;
            RepaintAllUI();
        }

        public void SetMissionComplete()
        {
            Logger.Log(name, "state set to MISSION COMPLETE");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.MISSIONCOMPLETE;
            RepaintAllUI();
        }

        public void SetLevelComplete()
        {
            Logger.Log(name, "state set to LEVEL COMPLETE");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.LEVELCOMPLETE;
            RepaintAllUI();
        }

        public void SetWorldMap()
        {
            Logger.Log(name, "state set to WORLDMAP");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.WORLDMAP;
        }

        public void ToggleStart()
        {
            if(state.state == State.GameStateEnum.PLAYING)
            {
                state.state = State.GameStateEnum.STARTMENU;
            }
            else if(state.state == State.GameStateEnum.STARTMENU)
            {
                state.state = State.GameStateEnum.PLAYING;
            }
        }

        public void ToggleSelect()
        {
            if (state.state == State.GameStateEnum.PLAYING)
            {
                state.state = State.GameStateEnum.SELECTMENU;
            }
            else if (state.state == State.GameStateEnum.SELECTMENU)
            {
                state.state = State.GameStateEnum.PLAYING;
            }
        }

        public void CreateAlert()
        {
            this.roomAlert = Instantiate(roomAlertPrefab, Vector3.zero, Quaternion.identity, levelParent.transform);
            ClearAlert();
        }

        public void SetAlert(GameObject trap)
        {
            Logger.Log(name, "level alert set");
            state.alertTrap = trap;
            roomAlert.transform.position = trap.transform.position;
            roomAlert.SetActive(true);
        }

        public void ClearAlert()
        {
            state.alertTrap = null;
            roomAlert.SetActive(false);
        }

        public void SaveGame()
        {
            string json = JsonUtility.ToJson(state);
            string path = Path.Combine(Application.persistentDataPath, "SaveFile" + state.gameSlot + ".json");
            Logger.Log(name, "saving ", path);
            File.WriteAllText(path, json);
        }

        public void UpdateLights()
        {
            switch (state.currentLight)
            {
                case State.GlobalLightTypes.Warn:
                    globalLight.color = lightsWarn;
                    break;
                case State.GlobalLightTypes.Alert:
                    globalLight.color = lightsAlert;
                    break;
                case State.GlobalLightTypes.Bar:
                    globalLight.color = lightsBar;
                    break;
                case State.GlobalLightTypes.Default:
                default:
                    globalLight.color = lightsDefault;
                    break;
            }
        }

        public void SetLight(State.GlobalLightTypes light)
        {
            state.currentLight = light;
        }

        void Update()
        {
            switch (state.state)
            {
                case State.GameStateEnum.PLAYING:
                    PlayingUpdate();
                    /*
                    if (inputController.StartButtonUp())
                    {
                        Logger.Log(name, "show start menu");
                        state.state = State.GameStateEnum.STARTMENU;
                    }

                    if (inputController.SelectButtonUp())
                    {
                        Logger.Log(name, "show select menu");
                        state.state = State.GameStateEnum.SELECTMENU;
                    }*/
                    break;

                case State.GameStateEnum.STARTMENU:
                    /*
                    if (inputController.StartButtonUp())
                    {
                        Logger.Log(name, "hide start menu");
                        SetPlaying();
                    }
                    break;

                case State.GameStateEnum.SELECTMENU:
                    if (inputController.SelectButtonUp())
                    {
                        Logger.Log(name, "hide select menu");
                        SetPlaying();
                    }*/
                    break;
                case State.GameStateEnum.MISSIONCOMPLETE:
                    break;

                case State.GameStateEnum.GAMEOVER:
                    EndGame();
                    break;
                default:
                    break;
            }

            UpdateLights();
            RepaintAllUI();

        }

        public bool IsPlaying()
        {
            return state.state == State.GameStateEnum.PLAYING;
        }

        public bool IsInGame()
        {
            return state.state == State.GameStateEnum.PLAYING || state.state == State.GameStateEnum.NARRATION || CoreGame.Instance.state.state == State.GameStateEnum.STARTMENU || CoreGame.Instance.state.state == State.GameStateEnum.SELECTMENU || CoreGame.Instance.state.state == State.GameStateEnum.LEVELCOMPLETE;
        }


        void LateUpdate()
        {
            actionManager.UpdateBehaviour();
            switch (state.state)
            {
                case State.GameStateEnum.CAPTURED:
                case State.GameStateEnum.DIALOGUE:
                case State.GameStateEnum.GAMEOVER:
                case State.GameStateEnum.MISSIONCOMPLETE:
                case State.GameStateEnum.LEVELCOMPLETE:
                case State.GameStateEnum.SELECTMENU:
                case State.GameStateEnum.STARTMENU:
                case State.GameStateEnum.WORLDMAP:
                case State.GameStateEnum.NARRATION:
                case State.GameStateEnum.PLAYING:
                    UpdateDoorAnimations();
                    break;
            }
        }

        void FixedUpdate()
        {
            switch (state.state)
            {
                case State.GameStateEnum.PLAYING:
                    PlayingFixedUpdate();
                    break;
            }
        }

        void UpdateDoorAnimations()
        {
            foreach(var door in state.doorList)
            {
                door.UpdateAnimation();
            }
        }

        void PlayingUpdate()
        {

            //playerController.UpdateMovement(inputController);
            //PlayingNPCUpdate();
            PlayingEnemyUpdate();
            PlayingTriggerUpdate();
            state.currentLevel.timer.Update();
        }

        void PlayingTriggerUpdate()
        {
            foreach (var trigger in state.triggerList)
            {
                //trigger.UpdateTrigger(inputController);
            }
        }

        void PlayingEnemyUpdate()
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

        void PlayingFixedUpdate()
        {
            //playerController.UpdateTransform();
        }
    }
}