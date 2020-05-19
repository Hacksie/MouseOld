using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class PreludeActions : ILevelActions
    {
        public bool Invoke(string actionName)
        {
            switch (actionName)
            {
                case "Bootstrap":
                    CoreGame.Instance.SetTitlecard();
                    break;
                case "Bootstrap1":
                    Debug.Log("PreludeActions: invoke Prelude1");
                    InfoRepository.Instance.AddToKnownEntities("Mouse");
                    InfoRepository.Instance.AddToKnownEntities("Arisana");
                    InfoRepository.Instance.AddToKnownEntities("ManagerLyon");
                    InfoRepository.Instance.AddToKnownEntities("Cat");
                    InfoRepository.Instance.AddToKnownEntities("Saika");
                    Dialogue.NarrationManager.instance.ShowNarration("Bootstrap1");
                    break;
                case "Prelude":
                    CoreGame.Instance.SetTitlecard();
                    return true;
                case "Prelude1":
                    Debug.Log("PreludeActions: invoke Prelude1");
                    InfoRepository.Instance.AddToKnownEntities("Mouse");
                    InfoRepository.Instance.AddToKnownEntities("Arisana");
                    InfoRepository.Instance.AddToKnownEntities("ManagerLyon");
                    InfoRepository.Instance.AddToKnownEntities("Cat");
                    InfoRepository.Instance.AddToKnownEntities("Saika");

                    ActionManager.instance.AddActionMessage("Task added to current tasks - Bootstrap");
                    if (!CoreGame.Instance.state.taskList.Exists(t => t.id == "bootstrap"))
                    {
                        var task = TaskDefinitionRepository.Instance.GetTaskInstance("bootstrap");
                        CoreGame.Instance.state.taskList.Add(task);
                        CoreGame.Instance.state.selectedTask = task;
                    }
                    CoreGame.Instance.state.currentLevel.completed = true;

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
                case "PreludeGun":
                    Dialogue.NarrationManager.instance.ShowNarration("PreludeGun");
                    return true;
                case "PreludeClothes":
                    Dialogue.NarrationManager.instance.ShowNarration("PreludeClothes");
                    return true;
                case "PreludeCat":
                case "PreludeCat1":
                    Debug.Log("PreludeActions: prelude Cat");
                    CoreGame.Instance.state.story.prelude_cat_talk = true;
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
                case "PreludeFridge":
                    Dialogue.NarrationManager.instance.ShowNarration("PreludeFridge");
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
            CoreGame.Instance.state.story.prelude_laptop = true;
            SelectMenuManager.instance.MenuState = SelectMenuManager.SelectMenuState.TASKS;
            CoreGame.Instance.state.state = GameState.GameStateEnum.SELECTMENU;

            
        }


        public void PreludeExit()
        {
            if (CoreGame.Instance.state.currentLevel.completed)
            {
                CoreGame.Instance.SetWorldMap();
                //CoreGame.Instance.LoadNewLevel("Bootstrap");
            }
            else
            {
                Debug.Log("PreludeActions: can't exit, haven't received mission");
            }
            /*
            if (CoreGame.Instance.state.taskList.Exists(t => t.title == "Milk Run"))
            {
                Debug.Log("PreludeActions: can exit");
                

            }
            else
            {
                Debug.Log("PreludeActions: can't exit, haven't received mission");
            }*/
        }
    }
}
