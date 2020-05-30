using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Dialogue
    {
        public partial class NarrationManager : MonoBehaviour
        {
            [SerializeField] private List<Narration> narrationList = new List<Narration>();
            [SerializeField] private string narrationResource = @"Narration/";

            public static NarrationManager Instance { get; private set;}
            public Narration CurrentNarration { get; private set;}			

            NarrationManager() => Instance = this;

            public void Initialize() => LoadNarration();

            private void LoadNarration()
            {
                foreach (var file in Resources.LoadAll<TextAsset>(narrationResource))
                {
                    var narrations = JsonUtility.FromJson<NarrationHolder>(file.text);
                    narrationList.AddRange(narrations.narrations);
                    Logger.Log(this, "Narrations added from: ", file.name);
                }
            }

            public void ShowNarration(Narration narration)
            {
                if (narration != null)
                {
                    Logger.Log(this, "Show narration ", narration.id);
                    CurrentNarration = narration;
                    GameManager.Instance.SetNarration();
                }
                else
                {
                    Logger.LogError(this, "No narration to show");
                }
            }

            public void ShowNarration(string id) => ShowNarration(narrationList.FirstOrDefault(e => e != null && e.id == id));

            public void NarrationButtonEvent()
            {
                Logger.Log(this, "Narration button event");

                string nextAction = CurrentNarration.action;

                CurrentNarration = null;
                GameManager.Instance.SetPlaying();

                Story.SceneManager.Instance.Invoke(nextAction);
            }

            public Narration GetCurrentNarration()
            {
                return CurrentNarration;
            }
        }
    }
}