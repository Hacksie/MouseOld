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
                    InfoRepository.Instance.AddToKnownEntities("BouncerJoe");
                    return true;              
                case "PreludeBarKat":
                    InfoRepository.Instance.AddToKnownEntities("Kat");
                    return true;
            }
            return false;
        }


        public void PreludeExit()
        {
            if (GameManager.Instance.state.taskList.Exists(t => t.title == "Milk Run"))
            {
                Debug.Log("PreludeActions: can exit");
                GameManager.Instance.LoadNewLevel("Arisana Bar");
            }
            else
            {
                Debug.Log("PreludeActions: can't exit, haven't received mission");
            }
        }
    }
}
