using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class BootstrapScene : GlobalScene {
        

        public BootstrapScene(string templateName, int length, int height, int width, int difficulty, int enemies, int traps) : base(templateName, length, height, width, difficulty, enemies, traps)
        {
            
        }

        public override void Begin()
        {
            GameManager.Instance.SetPlaying();
        }

        public override void Next()
        {

        }        

        public override bool Complete()
        {
            return GameManager.Instance.Data.CurrentLevel.completed == true;
        }   

        public override bool Invoke(string actionName)
        {
            switch (actionName)             
            {
                case "EndComputer":
                GameManager.Instance.Data.CurrentLevel.completed = true;
                return true;
                
            }

            return false;
        }
    }
}
