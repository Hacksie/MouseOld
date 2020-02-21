using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace HackedDesign
{
    public class CoreGame : MonoBehaviour
    {
        public static CoreGame Instance { get; private set; }

        public State.GameState state;

        private Input.IInputController inputController;

        [Header("Test Flags")]
        [SerializeField]
        private RuntimePlatform testPlatform = RuntimePlatform.WindowsEditor;
        [SerializeField]
        private bool testPlatformFlag = false;

        [Header("Game")]
        [SerializeField]
        private Entity.EntityManager entityManager = null;
        [SerializeField]
        private CharacterSpriteManager characterSpriteManager = null;

        private List<CharacterSprite> characterSprites = new List<CharacterSprite>();

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
        private string newGameLevel = "Olivia's Room";

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
        //[SerializeField]
        //private StatsPanelPresenter statsPanel = null;        
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
        // [SerializeField]
        // private Dialogue.DialogueManager dialogueManager = null;
        // [SerializeField]
        // private Dialogue.DialoguePanelPresenter dialoguePanel = null;
        [SerializeField]
        private MissionCompleteManager missionCompleteManager = null;
        [SerializeField]
        private MissionCompletePresenter missionCompletePanel = null;
        [SerializeField]
        private Level.MinimapPresenter minimapPanel = null;
        [SerializeField]
        private StatsPresenter statsPanel = null;
        [SerializeField]
        private TitlecardPresenter titlecardPanel = null;


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
            state.state = State.GameStateEnum.MAINMENU;
            Debug.Log(this.name + ": Initialization");

            UI.SetActive(true);

            SetPlatformInput();

            narrationManager.Initialize(inputController);
            infoManager.Initialize();
            characterSpriteManager.Initialize(infoManager);
            mobileInputUI.Initialize(inputController);
            actionConsolePanel.Initialize(actionManager);            
            infoPanel.Initialize(infoManager, selectMenuManager);
            stashPanel.Initialize(selectMenuManager);
            psychPanel.Initialize(selectMenuManager);
            taskPanel.Initialize(selectMenuManager);
            startMenuPanel.Initialize(startMenuManager);
            selectMenuPanel.Initialize(selectMenuManager, infoPanel, taskPanel, stashPanel, psychPanel, levelMapPanel);
            narrationPanel.Initialize(narrationManager, infoManager, characterSpriteManager);
            worldMapPanel.Initialize(worldMapManager);
            statsPanel.Initialize();
            actionPanel.Initialize();
            titlecardPanel.Initialize(actionManager);
            levelRenderer.Initialize(entityManager, infoManager, characterSpriteManager, levelParent, npcParent, polyNav2D);
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
            state.state = State.GameStateEnum.LOADING;
            entityManager.Initialize(npcParent);
            actionManager.Initialize(entityManager, taskManager);
            state.currentLevel = levelGenerator.GenerateLevel(newGameLevel, 1, 1, 1, 0, 0, 0);
            state.isRandom = false;
            state.player = new State.PlayerState();
            CoreGame.Instance.SceneInitialize();
        }

        public void LoadRandomGame(string template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            Debug.Log(this.name + ": loading random game");
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
            Debug.Log(this.name + ": loading new level");
            state.state = State.GameStateEnum.LOADING;
            //levelRenderer.DestroyLevel();
            state.entityList.Clear();
            state.currentLevel = levelGenerator.GenerateLevel(template);

            entityManager.Initialize(npcParent);
            actionManager.Initialize(entityManager, taskManager);
            SceneInitialize();
        }

        public void EndGame()
        {
            Debug.Log(this.name + ": End Game");
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

            Debug.Log(this.name + ": scene initialization");
            ShowPlayer(true);
            SetLight(State.GlobalLightTypes.Default);

            //this.State.entityList.Clear();
            levelRenderer.Render(this.state.currentLevel);

            levelRenderer.PopulateNPCSpawns(this.state.currentLevel, this.state.entityList);
            this.state.entityList.AddRange(levelRenderer.PopulateEnemySpawns(this.state.currentLevel));
            this.state.entityList.AddRange(levelRenderer.PopulateTrapSpawns(this.state.currentLevel));



            levelMapPanel.Initialize(selectMenuManager, state.currentLevel);
            minimapPanel.Initialize(state.currentLevel);



            player.transform.position = state.currentLevel.ConvertLevelPosToWorld(state.currentLevel.playerSpawn.levelLocation) + state.currentLevel.playerSpawn.worldOffset;

            characterSprites.Clear();
            playerController = player.GetComponent<PlayerController>();
            var playerSprite = player.GetComponent<CharacterSprite>();
            playerSprite.Initialize(characterSpriteManager);
            characterSprites.Add(playerSprite);

            foreach (Entity.BaseEntity npc in state.entityList)
            {
                var charSprite = npc.gameObject.GetComponent<CharacterSprite>();
                if (charSprite != null)
                {
                    Debug.Log(this.name + ": character sprite added: " + charSprite.gameObject.name);
                    characterSprites.Add(charSprite);
                }
            }



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
            stashPanel.Repaint();
            psychPanel.Repaint();
            levelMapPanel.Repaint();
            worldMapPanel.Repaint();
            actionPanel.Repaint();
            timerPanel.Repaint();
            mobileInputUI.Repaint();
            minimapPanel.Repaint();
            cursorPresenter.Repaint();
            titlecardPanel.Repaint();
            statsPanel.Repaint();
            
        }

        // FIXME: There's probably a better way to do this these days
        void SceneTriggersInitialize()
        {

            state.triggerList.Clear();
            Debug.Log(this.name + ": initializing triggers, count " + GameObject.FindGameObjectsWithTag("Trigger").Length);


            foreach (GameObject triggerObject in GameObject.FindGameObjectsWithTag("Trigger"))
            {
                if (!triggerObject.activeInHierarchy)
                {
                    continue;
                }

                Debug.Log(this.name + ": initializing trigger " + triggerObject.name);
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
            Debug.Log(this.name + ": GameOver");
            state.state = State.GameStateEnum.GAMEOVER;
        }

        public void SetTitlecard()
        {
            Debug.Log(this.name + ": state set to TITLECARD");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.TITLECARD;            
        }

        public void SetPlaying()
        {
            Debug.Log(this.name + ": state set to PLAYING");
            Time.timeScale = 1;
            state.state = State.GameStateEnum.PLAYING;
        }

        public void SetDialogue()
        {
            Debug.Log(this.name + ": state set to DIALOGUE");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.DIALOGUE;
            //Cursor.visible = true;
        }

        public void SetNarration()
        {
            Debug.Log(this.name + ": state set to NARRATION");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.NARRATION;
            RepaintAllUI();
        }

        public void SetMissionComplete()
        {
            Debug.Log(this.name + ": state set to MISSION COMPLETE");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.MISSIONCOMPLETE;
            RepaintAllUI();
        }

        public void SetWorldMap()
        {
            Debug.Log(this.name + ": state set to WORLDMAP");
            Time.timeScale = 0;
            state.state = State.GameStateEnum.WORLDMAP;
        }

        public void CreateAlert()
        {
            this.roomAlert = GameObject.Instantiate(roomAlertPrefab, Vector3.zero, Quaternion.identity, levelParent.transform);
            ClearAlert();

        }

        public void SetAlert(GameObject trap)
        {
            Debug.Log(this.name + ": level alert set");
            this.state.alertTrap = trap;
            this.roomAlert.transform.position = trap.transform.position;
            this.roomAlert.SetActive(true);

        }

        public void ClearAlert()
        {
            this.state.alertTrap = null;
            this.roomAlert.SetActive(false);
        }

        public void SaveGame()
        {
            string json = JsonUtility.ToJson(state);
            string path = Path.Combine(Application.persistentDataPath, "SaveFile" + state.gameSlot + ".json");
            Debug.Log(this.name + ": saving " + path);
            File.WriteAllText(path, json);

            //XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            //using (StringWriter sw =)

        }

        public void UpdateLights()
        {
            switch (state.currentLight)
            {
                case State.GlobalLightTypes.Default:
                    globalLight.color = lightsDefault;
                    break;
                case State.GlobalLightTypes.Warn:
                    globalLight.color = lightsWarn;
                    break;
                case State.GlobalLightTypes.Alert:
                    globalLight.color = lightsAlert;
                    break;
                case State.GlobalLightTypes.Bar:
                    globalLight.color = lightsBar;
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
                    //timer.UpdateTimer ();

                    if (inputController.StartButtonUp())
                    {
                        Debug.Log(this.name + ": show start menu");
                        state.state = State.GameStateEnum.STARTMENU;
                    }

                    if (inputController.SelectButtonUp())
                    {
                        Debug.Log(this.name + ": show select menu");
                        state.state = State.GameStateEnum.SELECTMENU;
                    }
                    break;

                case State.GameStateEnum.STARTMENU:
                    if (inputController.StartButtonUp())
                    {
                        Debug.Log(this.name + ": hide start menu");
                        SetPlaying();
                    }
                    break;

                case State.GameStateEnum.SELECTMENU:
                    if (inputController.SelectButtonUp())
                    {
                        Debug.Log(this.name + ": hide select menu");
                        SetPlaying();
                    }
                    break;
                case State.GameStateEnum.MISSIONCOMPLETE:
                    break;

                case State.GameStateEnum.GAMEOVER:
                    EndGame();
                    break;
            }

            UpdateLights();
            RepaintAllUI();

        }

        public void UpdateCharacterSprites()
        {
            for (int i = 0; i < characterSprites.Count; i++)
            {
                characterSprites[i].UpdateSprites();
            }
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
                case State.GameStateEnum.SELECTMENU:
                case State.GameStateEnum.STARTMENU:
                case State.GameStateEnum.WORLDMAP:
                case State.GameStateEnum.NARRATION:
                case State.GameStateEnum.PLAYING:
                    UpdateCharacterSprites();
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

        void PlayingUpdate()
        {

            playerController.UpdateMovement(inputController);
            PlayingNPCUpdate();
            PlayingTriggerUpdate();
            state.currentLevel.timer.Update();
        }

        void PlayingTriggerUpdate()
        {
            foreach (Triggers.ITrigger trigger in state.triggerList)
            {
                trigger.UpdateTrigger(inputController);
            }
        }

        void PlayingNPCUpdate()
        {
            foreach (Entity.BaseEntity npc in state.entityList)
            {
                npc.UpdateBehaviour();
            }
        }

        void PlayingFixedUpdate()
        {
            playerController.UpdateTransform();
        }
    }
}