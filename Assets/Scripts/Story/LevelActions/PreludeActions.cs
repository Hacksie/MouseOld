using UnityEngine;
using HackedDesign.Entity;

namespace HackedDesign.Story
{
    public class PreludeActions : ILevelActions
    {
        public bool Invoke(string actionName)
        {
            switch (actionName)
            {
                case "Prelude":
                    CoreGame.Instance.SetTitlecard();
                    return true;
                case "Prelude1":
                    Debug.Log("PreludeActions: invoke Prelude1");
                    InfoManager.instance.AddToKnownCorps("Arisana");
                    InfoManager.instance.AddToKnownCharacters("ManagerLyon");
                    InfoManager.instance.AddToKnownCharacters("Cat");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude1");
                    return true;
                case "Prelude2":
                    Debug.Log("PreludeActions: invoke Prelude2");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude2");
                    return true;
                case "Prelude3":
                    Debug.Log("PreludeActions: invoke Prelude3");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude3");
                    return true;
                case "Prelude4":
                    Debug.Log("PreludeActions: invoke Prelude4");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude4");
                    return true;
                case "Prelude5":
                    Debug.Log("PreludeActions: invoke Prelude5");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude5");
                    return true;
                case "Prelude6":
                    Debug.Log("PreludeActions: invoke Prelude6");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude6");
                    return true;
                case "Prelude7":
                    Debug.Log("PreludeActions: invoke Prelude7");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude7");
                    return true;
                case "Prelude8":
                    Debug.Log("PreludeActions: invoke Prelude8");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude8");
                    return true;
                case "PreludeLaptop":
                    PreludeLaptop();
                    return true;
                case "PreludeCat":
                case "PreludeCat1":
                    Debug.Log("PreludeActions: prelude Cat");
                    CoreGame.Instance.State.story.prelude_cat_talk = true;
                    Dialogue.NarrationManager.instance.ShowNarration("PreludeCat1");
                    return true;
                case "PreludeCat2":
                    Dialogue.NarrationManager.instance.ShowNarration("PreludeCat2");
                    return true;
                case "PreludeCat3":
                    Dialogue.NarrationManager.instance.ShowNarration("PreludeCat3");
                    return true;
                case "PreludeCat4":
                    Dialogue.NarrationManager.instance.ShowNarration("PreludeCat4");
                    return true;
                case "PreludeCat5":
                    Dialogue.NarrationManager.instance.ShowNarration("PreludeCat5");
                    return true;
                case "PreludeExit":
                    PreludeExit();
                    return true;
                case "PreludeBarKat":
                    Debug.Log("Snowowl");
                    return true;
            }
            return false;
        }

        public void PreludeCat3()
        {
            
        }
        public void PreludeCat4()
        {
            
        }


        public void PreludeLaptop()
        {
            Debug.Log("PreludeActions: prelude laptop");
            CoreGame.Instance.State.story.prelude_laptop = true;
            ActionManager.instance.AddActionMessage("Task 'Milk Run' added to Tasks");
            //taskManager.selectedTask = 
            InfoManager.instance.AddToKnownCorps("Saika");
            //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Kat"));


            if (!CoreGame.Instance.State.taskList.Exists(t => t.title == "Milk Run"))
            {
                var task = TaskDefinitionManager.instance.GetTaskInstance("Milk Run");
                CoreGame.Instance.State.taskList.Add(task);
                CoreGame.Instance.State.selectedTask = task;
            }

            SelectMenuManager.instance.MenuState = SelectMenuManager.SelectMenuState.TASKS;
            CoreGame.Instance.State.state = GameStateEnum.SELECTMENU;
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
