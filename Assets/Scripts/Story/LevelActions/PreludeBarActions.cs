using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class PreludeBarActions : GlobalActions
    {
        public override void Begin() 
        {
            var levelTemplate = GameManager.Instance.GetLevelGenTemplate("Aisana Bar");
            GameManager.Instance.GameState.CurrentLevel = Level.LevelGenerator.Generate(levelTemplate, 0, 0, 0);
            GameManager.Instance.GameState.CurrentLevel.Print();
            GameManager.Instance.SceneInitialize();     
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
