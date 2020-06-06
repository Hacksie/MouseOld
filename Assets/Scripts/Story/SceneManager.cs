using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace HackedDesign.Story
{
    public class SceneManager : MonoBehaviour
    {
        [Header("Level")]
        [SerializeField] private Level.LevelGenTemplate[] levelGenTemplates = null;
        [SerializeField] private string newGameLevel = "Olivia's Apartment";
        [SerializeField] private float timeOut = 10.0f;

        public static SceneManager Instance { get; private set; }

        public List<ActionMessage> console = new List<ActionMessage>();

        public string NewGameLevelDefault { get { return newGameLevel; } private set { newGameLevel = value; } }

        public IScene CurrentScene { get; set; }

        SceneManager() => Instance = this;

        public Level.LevelGenTemplate GetLevelGenTemplate(string template) => levelGenTemplates.FirstOrDefault(t => t.name == template);

        public void Initialize()
        {

        }

        public void AddActionMessage(string message) => console.Add(new ActionMessage()
        {
            time = Time.time,
            message = message
        });

        public void UpdateBehaviour()
        {
            if (console.Count > 0 && (Time.time - console[0].time) > timeOut)
            {
                console.RemoveAt(0);
            }
        }

        public void Invoke(string actionName)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                return;
            }

            var handled = CurrentScene.Invoke(actionName);

            if (!handled)
            {
                Logger.LogWarning(this, "Cannot invoke action: ", actionName, " in current scene");
            }
        }

        public void AddToKnownLocations(string locationId)
        {
            GameManager.Instance.Data.Story.KnownLocations.Add(locationId);
        }

        public List<string> GetKnownLocations()
        {
            return GameManager.Instance.Data.Story.KnownLocations;
        }

        public IEnumerable<Level.LevelGenTemplate> GetFloorsForLocation(string location)
        {
            return levelGenTemplates.Where(f => f.location == location);
        }
    }

    public class ActionMessage
    {
        public float time;
        public string message;
    }

}
