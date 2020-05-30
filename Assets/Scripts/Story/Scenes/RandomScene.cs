using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class RandomScene : GlobalScene
    {
        public RandomScene(string templateName, int length, int height, int width, int difficulty, int enemies, int traps) : base(templateName, length, height, width, difficulty, enemies, traps)
        {
            LoadLevel();
            GameManager.Instance.SceneInitialize();
            //GameManager.Instance.SetTitlecard(); //FIXME: Make this async / loaderbar
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
            return false;
        }        
    }
}