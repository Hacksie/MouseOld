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
                case "PreludeExit":
                    PreludeExit();
                    return true;
                case "PreludeBarJoe":
                    //InfoRepository.Instance.AddToKnownEntities("BouncerJoe");
                    return true;
                case "PreludeBarKat":
                    InfoRepository.Instance.AddToKnownEntities("SnowOwl");
                    return true;
            }
            return false;
        }


        public void PreludeExit()
        {
            GameManager.Instance.SetWorldMap();

        }
    }
}
