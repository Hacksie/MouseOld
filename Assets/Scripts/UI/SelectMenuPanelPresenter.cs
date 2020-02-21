using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    public class SelectMenuPanelPresenter : MonoBehaviour
    {

        SelectMenuManager selectMenuManager;

        private Story.InfoPanelPresenter infoPanel = null;
        private Story.TaskPanelPresenter taskPanel = null;
        private Level.LevelMapPanelPresenter levelMapPanel = null;
        private StashPanelPresenter stashPanel = null;
        private PsychPanelPresenter psychPanel = null;

        public Button InfoButton = null;
        public Button TasksButton = null;
        public Button StashButton = null;
        public Button PsychButton = null;
        public Button MapButton = null;


        public void Initialize(SelectMenuManager selectMenuManager, Story.InfoPanelPresenter infoPanel, Story.TaskPanelPresenter taskPanel, StashPanelPresenter stashPanel, PsychPanelPresenter psychPanel, Level.LevelMapPanelPresenter levelMapPanel)
        {
            this.selectMenuManager = selectMenuManager;
            this.infoPanel = infoPanel;
            this.taskPanel = taskPanel;
            this.levelMapPanel = levelMapPanel;
            this.stashPanel = stashPanel;
            this.psychPanel = psychPanel;
        }

        public void Repaint()
        {
            if (CoreGame.Instance.state.state == State.GameStateEnum.SELECTMENU && !this.gameObject.activeInHierarchy)
            {
                Show(true);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                switch (selectMenuManager.MenuState)
                {
                    case SelectMenuManager.SelectMenuState.INFO:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(InfoButton.gameObject);
                        break;
                    case SelectMenuManager.SelectMenuState.TASKS:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(TasksButton.gameObject);
                        break;
                    case SelectMenuManager.SelectMenuState.STASH:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(StashButton.gameObject);
                        break;
                    case SelectMenuManager.SelectMenuState.PSYCH:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(PsychButton.gameObject);
                        break;
                    case SelectMenuManager.SelectMenuState.MAP:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(MapButton.gameObject);
                        break;
                }

            }
            else if (CoreGame.Instance.state.state != State.GameStateEnum.SELECTMENU && this.gameObject.activeInHierarchy)
            {
                Show(false);
            }

            infoPanel.Repaint();
            taskPanel.Repaint();
            stashPanel.Repaint();
            psychPanel.Repaint();
            levelMapPanel.Repaint();
        }

        private void Show(bool flag)
        {
            Debug.Log(this.name + ": repaint");
            this.gameObject.SetActive(flag);
              
        }

        public void ResumeClickEvent()
        {
            CoreGame.Instance.SetPlaying();
        }

        public void InfoClickEvent()
        {
            Debug.Log("Select Menu Info Clicked");
            selectMenuManager.MenuState = SelectMenuManager.SelectMenuState.INFO;
            Show(true);
        }

        public void TaskClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuManager.SelectMenuState.TASKS;
            Show(true);
        }

        public void StashClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuManager.SelectMenuState.STASH;
            Show(true);
        }

        public void PsychClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuManager.SelectMenuState.PSYCH;
            Show(true);
        }

        public void MapClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuManager.SelectMenuState.MAP;
            Show(true);
        }


    }
}