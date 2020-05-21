using System.Collections.Generic;
using HackedDesign.Story;
using UnityEngine;

namespace HackedDesign
{
    [System.Serializable]
    public class GameData
    {
        [Header("Save Properties")]
        [SerializeField] private int gameVersion = 0;
        [SerializeField] private int gameSlot = 0;

        [Header("Game State")]
        [SerializeField] private PlayStateEnum playState = PlayStateEnum.Titlecard;
        [SerializeField] private PlayerData playerState = null;
        [SerializeField] private StoryState story = new StoryState();
        [SerializeField] private Level.Level currentLevel = null;
        [SerializeField] private bool isRandom = false;

        public bool IsRandom { get { return isRandom; } private set { isRandom = value; } }

        private List<Story.Task> taskList = new List<Story.Task>();

        public Story.Task selectedTask = null;

        public GameObject alertTrap = null; // move this to state

        public GlobalLightTypes currentLight;

        public List<BaseTrigger> triggerList = new List<BaseTrigger>();
        //public List<IEntity> enemyList = new List<IEntity>();
        public List<IEntity> entityList = new List<IEntity>();
        public List<Door> doorList = new List<Door>();


        public int GameVersion { get { return gameVersion; } set { gameVersion = value; } }
        public int GameSlot { get { return gameSlot; } set { gameSlot = value; } }
        public PlayStateEnum PlayState { get { return playState; } set { playState = value; } }
        public PlayerData Player { get { return playerState; } set { playerState = value; } }
        public StoryState Story { get { return story; } set { story = value; } }
        public Level.Level CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }

        public List<Task> TaskList { get => taskList; private set => taskList = value; }

        public GameData() : this(false)
        {

        }


        public GameData(bool isRandom) : this(isRandom, PlayStateEnum.Loading)
        {

        }

        public GameData(bool isRandom, PlayStateEnum startingState)
        {
            this.IsRandom = isRandom;
            this.PlayState = startingState;
            this.Player = new PlayerData();
        }

        public bool IsPlaying()
        {
            return PlayState == PlayStateEnum.Playing;
        }

        public void SetPlaying()
        {
            PlayState = PlayStateEnum.Playing;
        }

    }

    public class StoryState
    {
        public int act = 0;
        public bool prelude_cat_talk = false;
        public bool prelude_laptop = false;
    }

    public enum PlayStateEnum
    {
        Titlecard,
        Cutscene,
        Playing,
        Hacking,
        Loading,
        Narration,
        Dialogue,
        Worldmap,
        StartMenu,
        SelectMenu,
        GameOver,
        Captured,
        MissionComplete,
        LevelComplete
    }

    public enum GameStateEnum
    {
        MainMenu,
        InGame
    }

    public enum SelectMenuState
    {
        Info,
        Tasks,
        Stash,
        Psych,
        Map
    }
}