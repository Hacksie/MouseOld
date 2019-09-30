using UnityEngine;
using HackedDesign.Entity;

namespace HackedDesign.Story
{
    public class PreludeActions : ILevelActions
    {
        public void Invoke(string actionName)
        {
            switch (actionName)
            {
                case "Prelude1":
                    Debug.Log("PreludeActions: invoke Prelude1");
                    InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Arisana"));
                    InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Manager Lyon"));
                    InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Kari"));
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude1");
                    break;
                case "Prelude2":
                    Debug.Log("PreludeActions: invoke Prelude2");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude2");
                    break;
                case "Prelude3":
                    Debug.Log("PreludeActions: invoke Prelude3");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude3");
                    break;
                case "Prelude4":
                    Debug.Log("PreludeActions: invoke Prelude4");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude4");
                    break;
                case "Prelude5":
                    Debug.Log("PreludeActions: invoke Prelude5");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude5");
                    break;
                case "Prelude6":
                    Debug.Log("PreludeActions: invoke Prelude6");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude6");
                    break;
                case "Prelude7":
                    Debug.Log("PreludeActions: invoke Prelude7");
                    Dialogue.NarrationManager.instance.ShowNarration("Prelude7");
                    SelectMenuManager.instance.MenuState = SelectMenuManager.SelectMenuState.TASKS;
                    CoreGame.Instance.State.state = GameStateEnum.SELECTMENU;
                    break;
                case "PreludeLaptop":
                    PreludeLaptop();
                    break;
                    //.SelectMenuState = SelectMenuManager.SelectMenuState.TASKS;
                case "PreludeKari":
                case "PreludeKari1":
                    PreludeKari1();
                    break;
                case "PreludeKari2":
                    PreludeKari2();
                    break;

                case "PreludeExit":
                    PreludeExit();
                    break;
                case "PreludeBarSnowOwl":
                    Debug.Log("Snowowl");
                    break;
            }

        }

        public void PreludeLaptop()
        {
            Debug.Log("PreludeActions: prelude laptop");
            CoreGame.Instance.State.story.prelude_laptop = true;
            ActionManager.instance.AddActionMessage("Task 'Milk Run' added to Tasks");
            //taskManager.selectedTask = 
            InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Saika"));
            InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Snow Owl"));


            if (!CoreGame.Instance.State.taskList.Exists(t => t.title == "Milk Run"))
            {
                var task = TaskDefinitionManager.instance.GetTaskInstance("Milk Run");
                CoreGame.Instance.State.taskList.Add(task);
                CoreGame.Instance.State.selectedTask = task;
            }

                    SelectMenuManager.instance.MenuState = SelectMenuManager.SelectMenuState.TASKS;
                    CoreGame.Instance.State.state = GameStateEnum.SELECTMENU;

            //Invoke("Prelude")
            //Invoke("Prelude5");
        }

        public void PreludeKari1()
        {
            Debug.Log("PreludeActions: prelude kari");
            CoreGame.Instance.State.story.prelude_kari_talk = true;
            Dialogue.NarrationManager.instance.ShowNarration("PreludeKari1");

        }

        public void PreludeKari2()
        {
            Dialogue.NarrationManager.instance.ShowNarration("PreludeKari2");
            var kari = EntityManager.instance.GetPooledNPC("Kari");
            //var trigger = kari.GetComponent<Triggers.BaseTrigger>();
            //trigger.enabled = false;
        }


        public void PreludeExit()
        {
            if (CoreGame.Instance.State.taskList.Exists(t => t.title == "Milk Run"))
            {
                CoreGame.Instance.LoadNewLevel("Arisana Bar");
                Debug.Log("PreludeActions: can exit");
            }
            else
            {
                Debug.Log("PreludeActions: can't exit, haven't received mission");
            }
        }
    }
}
