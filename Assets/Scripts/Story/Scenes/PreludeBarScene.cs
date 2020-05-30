using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class PreludeBarScene : GlobalScene
    {


        public PreludeBarScene(string templateName, int length, int height, int width, int difficulty, int enemies, int traps) : base(templateName, length, height, width, difficulty, enemies, traps)
        {
            LoadLevel();
            GameManager.Instance.SceneInitialize();

        }

        public override void Begin()
        {
            GameManager.Instance.SetPlaying();
        }

        public override void Next()
        {

        }

        public override bool Invoke(string actionName)
        {
            switch (actionName)
            {
                case "PreludeBarExit":
                    PreludeBarExit();
                    return true;
                case "PreludeBarJoe":
                case "PreludeBarJoe1":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeBarJoe1");
                    GameManager.Instance.Data.Story.storyEvents.Add("PreludeBarJoe");
                    return true;
                case "PreludeBarJoe2":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeBarJoe2");
                    return true;
                case "MeetSnowOwl":
                case "MeetSnowOwl1":
                    Logger.Log("PreludeBarScene", "Meet Snow Owl");
                    GameManager.Instance.Data.CurrentLevel.completed = true;
                    GameManager.Instance.Data.Story.storyEvents.Add("MeetSnowOwl");
                    Dialogue.NarrationManager.Instance.ShowNarration("MeetSnowOwl1");
                    TaskRepository.Instance.CompleteTaskObjective("bootstrap", "Meet Snow Owl");
                    SceneManager.Instance.AddToKnownLocations("SaikaCorpHQ");
                    //InfoRepository.Instance.AddToKnownEntities("SaikaCorpHQ");
                    return true;
                case "MeetSnowOwl2":
                    Logger.Log("PreludeBarScene", "Meet Snow Owl 2");
                    Dialogue.NarrationManager.Instance.ShowNarration("MeetSnowOwl2");
                    return true;
                case "MeetSnowOwl3":
                    Logger.Log("PreludeBarScene", "Meet Snow Owl 3");
                    Dialogue.NarrationManager.Instance.ShowNarration("MeetSnowOwl3");
                    return true;
                case "MeetSnowOwl4":
                    Logger.Log("PreludeBarScene", "Meet Snow Owl 4");
                    Dialogue.NarrationManager.Instance.ShowNarration("MeetSnowOwl4");
                    return true;
            }
            return base.Invoke(actionName);
        }

        public void PreludeBarExit()
        {
            GameManager.Instance.SetWorldMap();
        }

        public override bool Complete()
        {
            return GameManager.Instance.Data.CurrentLevel.completed && GameManager.Instance.Data.Story.storyEvents.Contains("MeetSnowOwl");
        }
    }
}
