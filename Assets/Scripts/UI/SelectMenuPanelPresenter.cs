﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    public class SelectMenuPanelPresenter : MonoBehaviour
    {

        SelectMenuManager selectMenuManager;

        // Inject these in
        private Story.InfoPanelPresenter infoPanel = null;
        private Story.TaskPanelPresenter taskPanel = null;
        private Level.LevelMapPanelPresenter levelMapPanel = null;
        private GameObject StashPanel = null;
        private GameObject PsychPanel = null;

        public Button InfoButton = null;
        public Button TasksButton = null;
        public Button StashButton = null;
        public Button PsychButton = null;
        public Button MapButton = null;


        public void Initialize(SelectMenuManager selectMenuManager, Story.InfoPanelPresenter infoPanel, Story.TaskPanelPresenter taskPanel, Level.LevelMapPanelPresenter levelMapPanel)
        {
            this.selectMenuManager = selectMenuManager;
            this.infoPanel = infoPanel;
            this.taskPanel = taskPanel;
            this.levelMapPanel = levelMapPanel;
        }

        public void Repaint()
        {
            if (CoreGame.Instance.State.state == GameStateEnum.SELECTMENU && !this.gameObject.activeInHierarchy)
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
            else if (CoreGame.Instance.State.state != GameStateEnum.SELECTMENU && this.gameObject.activeInHierarchy)
            {
                Show(false);
            }

            //if(selectMenuManager.)
            infoPanel.Repaint();
            taskPanel.Repaint();
            levelMapPanel.Repaint();



        }

        private void Show(bool flag)
        {
            Debug.Log(this.name + ": repaint");
            HideAll();
            this.gameObject.SetActive(flag);
            //Cursor.visible = flag;

            if (!flag)
            {
                return;
            }

            // switch(selectMenuManager.GetMenuState())
            // {
            // 	case SelectMenuManager.SelectMenuState.INFO:
            // 	//infoPanel.Show(true);
            // 	break;

            // 	case SelectMenuManager.SelectMenuState.TASKS:
            // 	//taskPanel.Show(true);
            // 	break;

            // 	case SelectMenuManager.SelectMenuState.STASH:
            // 	break;

            // 	case SelectMenuManager.SelectMenuState.PSYCH:
            // 	break;
            // }
        }

        public void HideAll()
        {
            if (infoPanel != null)
            {
                //infoPanel.Show(false);
            }

            if (taskPanel != null)
            {
                //taskPanel.Show(false);
            }

            if (StashPanel != null)
            {
                StashPanel.SetActive(false);
            }

            if (PsychPanel != null)
            {
                PsychPanel.SetActive(false);
            }
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

        public void MapClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuManager.SelectMenuState.MAP;
            Show(true);
        }


    }
}