using UnityEngine;
using System.IO;

namespace HackedDesign
{
    public class CoreGame : MonoBehaviour
    {
        public static CoreGame Instance { get; private set; }

        public GameState State;

        private Input.IInputController inputController;

        [Header("Test Flags")]
        [SerializeField]
        private RuntimePlatform testPlatform = RuntimePlatform.WindowsEditor;
        [SerializeField]
        private bool testPlatformFlag = false;

        [Header("Game")]
        [SerializeField]
        private Entity.EntityManager entityManager = null;

        [Header("Player")]
        [SerializeField]
        private GameObject player = null;
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
        private GameObject roomAlertPrefab = null;
        [SerializeField]
        private GameObject roomAlert = null;

        [Header("Lights")]
        //public UnityEngine.Experimental.Rendering.Light2D globalLight;
        //public UnityEngine.Experimental.Rendering.Light2D globalLight;
        //public UnityEngine.Rendering.Light2D globalLight;
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
        private WorldMapManager worldMapManager = null;
        [SerializeField]
        private WorldMapPanelPresenter worldMapPanel = null;
        [SerializeField]
        private StatsPanelPresenter statsPanel = null;        
        [SerializeField]
        private Story.InfoManager infoManager = null;
        [SerializeField]
        private Story.InfoPanelPresenter infoPanel = null;
        [SerializeField]
        private Story.TaskDefinitionManager taskManager = null;
        [SerializeField]
        private Story.TaskPanelPresenter taskPanel = null;
        [SerializeField]
        private Dialogue.NarrationManager narrationManager = null;
        [SerializeField]
        private Dialogue.NarrationPanelPresenter narrationPanel = null;
        [SerializeField]
        private Dialogue.DialogueManager dialogueManager = null;
        [SerializeField]
        private Dialogue.DialoguePanelPresenter dialoguePanel = null;
        [SerializeField]
        private MissionCompleteManager missionCompleteManager = null;
        [SerializeField]
        private MissionCompletePresenter missionCompletePanel = null;

        

        [SerializeField]
        private Level.LevelMapPanelPresenter levelMapPanel = null;

        [SerializeField]
        private TimerPanelPresenter timerPanel = null;
        // [SerializeField]
        // private Timer timer;

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
            State.state = GameStateEnum.MAINMENU;
            Debug.Log(this.name + ": Initialization");

            UI.SetActive(true);

            SetPlatformInput();

            mobileInputUI.Initialize(inputController);
            actionConsolePanel.Initialize(actionManager);
            narrationManager.Initialize(inputController);
            dialogueManager.Initialize(inputController);
            infoPanel.Initialize(infoManager, selectMenuManager);
            taskPanel.Initialize(selectMenuManager); // in the future, we might separate out the task definitions and actual task states
            startMenuPanel.Initialize(startMenuManager);
            selectMenuPanel.Initialize(selectMenuManager, infoPanel, taskPanel, levelMapPanel);
            narrationPanel.Initialize(narrationManager);
            dialoguePanel.Initialize(dialogueManager);
            worldMapPanel.Initialize(worldMapManager);
            statsPanel.Initialize();
            levelRenderer.Initialize(entityManager, levelParent, npcParent, polyNav2D);
            missionCompletePanel.Initialize(missionCompleteManager);

            RepaintAllUI();

            ShowPlayer(false);
        }

        private void SetPlatformInput()
        {
            switch (testPlatformFlag ? testPlatform : Application.platform)
            {
                case RuntimePlatform.Android:
                    Debug.Log(this.name + ": input platform Android");
                    inputController = new Input.AndroidInputController(mobileInputUI);
                    break;
                default:
                    Debug.Log(this.name + ": input platform Default");
                    //inputController = new Input.AndroidInputController (mobileInputUI);
                    inputController = new Input.DesktopInputController();
                    break;
            }
        }

        public void LoadNewGame()
        {
            Debug.Log(this.name + ": loading new game");
            State.state = GameStateEnum.LOADING;
            entityManager.Initialize(npcParent);
            actionManager.Initialize(entityManager, taskManager);
            State.currentLevel = levelGenerator.GenerateLevel("Victoria's Room", 1, 1, 1, 0, 0, 0);
            State.isRandom = false;
            State.player = new Character.PlayerState();
            CoreGame.Instance.SceneInitialize();
        }

        public void LoadRandomGame(string template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            Debug.Log(this.name + ": loading random game");
            State.state = GameStateEnum.LOADING;
            State.currentLevel = levelGenerator.GenerateLevel(template, length, height, width, difficulty, enemies, traps);
            State.isRandom = true;
            State.player = new Character.PlayerState();
            entityManager.Initialize(npcParent);
            actionManager.Initialize(entityManager, taskManager);
            CoreGame.Instance.SceneInitialize();
        }

        public void LoadNewLevel(string template)
        {
            Debug.Log(this.name + ": loading new level");
            State.state = GameStateEnum.LOADING;
            //levelRenderer.DestroyLevel();
            State.entityList.Clear();
            State.currentLevel = levelGenerator.GenerateLevel(template);

            //entityManager.Initialize(npcParent);
            //actionManager.Initialize(entityManager, taskManager);
            CoreGame.Instance.SceneInitialize();
        }

        public void EndGame()
        {
            State.state = GameStateEnum.MAINMENU;
            RepaintAllUI();
            levelRenderer.DestroyLevel();
            State.entityList.Clear();
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

            Debug.Log(this.name + ": scene initialization");
            ShowPlayer(true);
            SetLight(GlobalLightTypes.Default);

            this.State.entityList.Clear();
            levelRenderer.Render(this.State.currentLevel);

            this.State.entityList.AddRange(levelRenderer.PopulateNPCSpawns(this.State.currentLevel));
            this.State.entityList.AddRange(levelRenderer.PopulateEnemySpawns(this.State.currentLevel));
            this.State.entityList.AddRange(levelRenderer.PopulateTrapSpawns(this.State.currentLevel));

            levelMapPanel.Initialize(selectMenuManager, State.currentLevel);

            player.transform.position = State.currentLevel.ConvertLevelPosToWorld(State.currentLevel.playerSpawn.levelLocation) + State.currentLevel.playerSpawn.worldOffset;

            playerController = player.GetComponent<PlayerController>();

            SceneTriggersInitialize();
            CreateAlert();
            timerPanel.Initialize (State.currentLevel.timer);

            SetPlaying();

            if (!string.IsNullOrWhiteSpace(State.currentLevel.template.startingAction))
            {
                Story.ActionManager.instance.Invoke(State.currentLevel.template.startingAction);
            }

            RepaintAllUI();

        }

        void RepaintAllUI()
        {
            mainMenu.Repaint();
            actionConsolePanel.Repaint();
            dialoguePanel.Repaint();
            narrationPanel.Repaint();
            selectMenuPanel.Repaint();
            startMenuPanel.Repaint();
            missionCompletePanel.Repaint();

            levelMapPanel.Repaint();
            worldMapPanel.Repaint();
            statsPanel.Repaint();
            timerPanel.Repaint ();
            mobileInputUI.Repaint();
            cursorPresenter.Repaint();
        }

        void SceneTriggersInitialize()
        {

            State.triggerList.Clear();
            Debug.Log(this.name + ": initializing triggers, count " + GameObject.FindGameObjectsWithTag("Trigger").Length);


            foreach (GameObject triggerObject in GameObject.FindGameObjectsWithTag("Trigger"))
            {
                if(!triggerObject.activeInHierarchy)
                {
                    continue;
                }
                
                Debug.Log(this.name + ": initializing trigger " + triggerObject.name);
                Triggers.ITrigger trigger = triggerObject.GetComponent<Triggers.ITrigger>();
                if (trigger != null)
                {
                    State.triggerList.Add(trigger);
                    trigger.Initialize(inputController);
                }
            }
        }

        public GameObject GetPlayer()
        {
            return player;
        }

        public void GameOver()
        {
            Debug.Log(this.name + ": GameOver");
            State.state = GameStateEnum.GAMEOVER;
        }

        public void SetPlaying()
        {
            Debug.Log(this.name + ": state set to PLAYING");
            Time.timeScale = 1;
            State.state = GameStateEnum.PLAYING;
        }

        public void SetDialogue()
        {
            Debug.Log(this.name + ": state set to DIALOGUE");
            Time.timeScale = 0;
            State.state = GameStateEnum.DIALOGUE;
            //Cursor.visible = true;
        }

        public void SetNarration()
        {
            Debug.Log(this.name + ": state set to NARRATION");
            Time.timeScale = 0;
            State.state = GameStateEnum.NARRATION;
            RepaintAllUI();
        }

        public void SetMissionComplete()
        {
            Debug.Log(this.name + ": state set to MISSION COMPLETE");
            Time.timeScale = 0;
            State.state = GameStateEnum.MISSIONCOMPLETE;
            RepaintAllUI();
        }        

        public void SetWorldMap()
        {
            Debug.Log(this.name + ": state set to WORLDMAP");
            Time.timeScale = 0;
            State.state = GameStateEnum.WORLDMAP;
        }

        public void CreateAlert()
        {
            this.roomAlert = GameObject.Instantiate(roomAlertPrefab, Vector3.zero, Quaternion.identity, levelParent.transform);
            ClearAlert();

        }

        public void SetAlert(GameObject trap)
        {
            Debug.Log(this.name + ": level alert set");
            this.State.alertTrap = trap;
            this.roomAlert.transform.position = trap.transform.position;
            this.roomAlert.SetActive(true);

        }

        public void ClearAlert()
        {
            this.State.alertTrap = null;
            this.roomAlert.SetActive(false);
        }

        public void SaveGame()
        {
            string json = JsonUtility.ToJson(State);
            string path = Path.Combine(Application.persistentDataPath, "SaveFile" + State.gameSlot + ".json");
            Debug.Log(this.name + ": saving " + path);
            File.WriteAllText(path, json);

            //XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            //using (StringWriter sw =)

        }

        public void UpdateLights()
        {
            switch(State.currentLight)
            {
                case GlobalLightTypes.Default:
                    globalLight.color = lightsDefault;
                    break;
                case GlobalLightTypes.Warn:
                    globalLight.color = lightsWarn;
                    break;
                case GlobalLightTypes.Alert:
                    globalLight.color = lightsAlert;
                    break;                
                case GlobalLightTypes.Bar:
                    globalLight.color = lightsBar;
                    break;                
            }            
        }

        public void SetLight(GlobalLightTypes light)
        {
            State.currentLight = light;
        }

        void Update()
        {

            switch (State.state)
            {

                case GameStateEnum.PLAYING:
                    PlayingUpdate();
                    //timer.UpdateTimer ();

                    if (inputController.StartButtonUp())
                    {
                        Debug.Log(this.name + ": show start menu");
                        State.state = GameStateEnum.STARTMENU;
                    }

                    if (inputController.SelectButtonUp())
                    {
                        Debug.Log(this.name + ": show select menu");
                        State.state = GameStateEnum.SELECTMENU;
                    }

                    break;

                case GameStateEnum.STARTMENU:
                    if (inputController.StartButtonUp())
                    {
                        Debug.Log(this.name + ": hide start menu");
                        SetPlaying();
                    }
                    break;

                case GameStateEnum.SELECTMENU:
                    if (inputController.SelectButtonUp())
                    {
                        Debug.Log(this.name + ": hide select menu");
                        SetPlaying();
                    }
                    break;
                case GameStateEnum.MISSIONCOMPLETE:
                    break;

                case GameStateEnum.GAMEOVER:
                    EndGame();
                    break;

            }

            UpdateLights();
            RepaintAllUI();

        }

        void LateUpdate()
        {
            actionManager.UpdateBehaviour();
            switch (State.state)
            {
                case GameStateEnum.NARRATION:
                    //narrationPanel.Repaint ();
                    break;
            }
        }

        void FixedUpdate()
        {
            switch (State.state)
            {
                case GameStateEnum.PLAYING:
                    PlayingFixedUpdate();
                    break;
            }
        }

        void PlayingUpdate()
        {

            playerController.UpdateMovement(inputController);
            PlayingNPCUpdate();
            PlayingTriggerUpdate();
            State.currentLevel.timer.Update();
        }

        void PlayingTriggerUpdate()
        {
            foreach (Triggers.ITrigger trigger in State.triggerList)
            {

                trigger.UpdateTrigger();
            }
        }

        void PlayingNPCUpdate()
        {
            foreach (Entity.BaseEntity npc in State.entityList)
            {
                npc.UpdateBehaviour();
            }
        }

        void PlayingFixedUpdate()
        {
            //mapUI.SetPlayerLocation(player.transform.position);
            playerController.UpdateTransform();
        }
    }
}