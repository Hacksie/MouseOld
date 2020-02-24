using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class PreludeBarActions : ILevelActions
    {
        public bool Invoke(string actionName)
        {
            switch (actionName)
            {
                case "PreludeExit":
                    PreludeExit();
                    return true;
                case "PreludeBarJoe":
                    InfoManager.instance.AddToKnownCharacters("BouncerJoe");
                    return true;              
                case "PreludeBarKat":
                    InfoManager.instance.AddToKnownCharacters("Kat");
                    return true;
            }
            return false;
        }


        public void PreludeExit()
        {
            if (CoreGame.Instance.state.taskList.Exists(t => t.title == "Milk Run"))
            {
                Debug.Log("PreludeActions: can exit");
                CoreGame.Instance.LoadNewLevel("Arisana Bar");
            }
            else
            {
                Debug.Log("PreludeActions: can't exit, haven't received mission");
            }
        }
    }
}
