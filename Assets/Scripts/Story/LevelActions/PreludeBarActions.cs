using UnityEngine;
using HackedDesign.Entity;

namespace HackedDesign.Story
{
    public class PreludeBarActions : ILevelActions
    {
        public void Invoke(string actionName)
        {
            switch (actionName)
            {
                case "PreludeExit":
                    PreludeExit();
                    break;
                case "PreludeBarJoe":
                    InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Bouncer Joe"));
                    break;                    
                case "PreludeBarKat":
                    InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Kat"));
                    break;
            }

        }


        public void PreludeExit()
        {
            if (CoreGame.Instance.State.taskList.Exists(t => t.title == "Milk Run"))
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
