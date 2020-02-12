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
                case "Prelude1":
                    Debug.Log("PreludeActions: invoke Prelude1");
                    InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Arisana"));
                    InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Manager Lyon"));
                    InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Cat"));
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
                    //SelectMenuManager.instance.MenuState = SelectMenuManager.SelectMenuState.TASKS;
                    //CoreGame.Instance.State.state = GameStateEnum.SELECTMENU;
                    return true;
                case "PreludeLaptop":
                    PreludeLaptop();
                    return true;
                    //.SelectMenuState = SelectMenuManager.SelectMenuState.TASKS;
                case "PreludeCat":
                case "PreludeCat1":
                    PreludeCat1();
                    return true;
                case "PreludeCat2":
                    PreludeCat2();
                    return true;
                case "PreludeCat3":
                    PreludeCat3();
                    return true;
                case "PreludeCat4":
                    PreludeCat4();
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

        public void PreludeLaptop()
        {
            Debug.Log("PreludeActions: prelude laptop");
            CoreGame.Instance.State.story.prelude_laptop = true;
            ActionManager.instance.AddActionMessage("Task 'Milk Run' added to Tasks");
            //taskManager.selectedTask = 
            InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Saika"));
            //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Kat"));


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

        public void PreludeCat1()
        {
            Debug.Log("PreludeActions: prelude Cat");
            CoreGame.Instance.State.story.prelude_cat_talk = true;
            Dialogue.NarrationManager.instance.ShowNarration("PreludeCat1");

        }

        public void PreludeCat2()
        {
            Dialogue.NarrationManager.instance.ShowNarration("PreludeCat2");
            //var Cat = EntityManager.instance.GetPooledNPC("Cat");
            //var trigger = Cat.GetComponent<Triggers.BaseTrigger>();
            //trigger.enabled = false;
        }

        public void PreludeCat3()
        {
            Dialogue.NarrationManager.instance.ShowNarration("PreludeCat3");
        }        
        public void PreludeCat4()
        {
            Dialogue.NarrationManager.instance.ShowNarration("PreludeCat4");
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
