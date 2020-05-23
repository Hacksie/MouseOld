using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HackedDesign.Story;

namespace HackedDesign.UI
{
    public class SelectMenuPanelPresenter : AbstractPresenter
    {

        SelectMenuManager selectMenuManager;

        private InfoPanelPresenter infoPanel = null;
        private TaskPanelPresenter taskPanel = null;
        private StashPanelPresenter stashPanel = null;
        private PsychPanelPresenter psychPanel = null;

        [SerializeField] private Button InfoButton = null;
        [SerializeField] private Button TasksButton = null;
        [SerializeField] private Button StashButton = null;
        [SerializeField] private Button PsychButton = null;
        [SerializeField] private Button MapButton = null;


        public void Initialize(SelectMenuManager selectMenuManager, InfoPanelPresenter infoPanel, TaskPanelPresenter taskPanel, StashPanelPresenter stashPanel, PsychPanelPresenter psychPanel)
        {
            this.selectMenuManager = selectMenuManager;
            this.infoPanel = infoPanel;
            this.taskPanel = taskPanel;
            this.stashPanel = stashPanel;
            this.psychPanel = psychPanel;
        }

        public override void Repaint()
        {

            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

            switch (selectMenuManager.MenuState)
            {
                case SelectMenuSubState.Info:
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(InfoButton.gameObject);
                    break;
                case SelectMenuSubState.Tasks:
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(TasksButton.gameObject);
                    break;
                case SelectMenuSubState.Stash:
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(StashButton.gameObject);
                    break;
                case SelectMenuSubState.Psych:
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(PsychButton.gameObject);
                    break;
                case SelectMenuSubState.Map:
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(MapButton.gameObject);
                    break;
            }
            infoPanel.Repaint();
            taskPanel.Repaint();
            stashPanel.Repaint();
            psychPanel.Repaint();
        }

        private void Show(bool flag)
        {
            gameObject.SetActive(flag);

        }

        public void ResumeClickEvent()
        {
            GameManager.Instance.SetPlaying();
        }

        public void InfoClickEvent()
        {
            Debug.Log("Select Menu Info Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Info;
            Show(true);
        }

        public void TaskClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Tasks;
            Show(true);
        }

        public void StashClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Stash;
            Show(true);
        }

        public void PsychClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Psych;
            Show(true);
        }

        public void MapClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Map;
            Show(true);
        }


    }
}