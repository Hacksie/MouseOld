using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        public class InfoRepository : MonoBehaviour
        {
            public static InfoRepository Instance { get; private set; }

            [Header("Config")]
            public List<InfoCategory> categories = new List<InfoCategory>();
            public List<Character> characters = new List<Character>();
            public List<Corp> corps = new List<Corp>();
            public List<Enemy> enemies = new List<Enemy>();
            public List<Location> locations = new List<Location>();
            public Dictionary<string, InfoEntity> entities = new Dictionary<string, InfoEntity>();

            public string categoryResource = @"Info/Categories/";
            public string corpsResource = @"Info/Corps/";
            public string charactersResource = @"Info/Characters/";
            public string enemiesResource = @"Info/Enemies/";
            public string locationsResource = @"Info/Locations/";

            [Header("State")]
            public Dictionary<string, InfoEntity> knownEntities = new Dictionary<string, InfoEntity>();
            public List<Character> knownCharacters = new List<Character>();
            public List<Corp> knownCorps = new List<Corp>();
            public List<Enemy> knownEnemies = new List<Enemy>();
            public List<Enemy> uniqueEnemies = new List<Enemy>();
            public List<Location> knownLocations = new List<Location>();

            public string SelectedInfoCategory { get; set; }
            public string SelectedInfoEntity { get; set; }

            public InfoRepository()
            {
                Instance = this;
            }

            private void Start()
            {

                //LoadCategories();
                LoadCorps();
                LoadCharacters();
                //LoadEnemies();
                //LoadLocations();

            }

            public void LoadCategories()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(categoryResource);

                foreach (var file in jsonTextFiles)
                {
                    var cat = JsonUtility.FromJson<InfoCategory>(file.text);
                    Debug.Log(this.name + " category added: " + cat.id);
                    categories.Add(cat);
                }
            }

            public void LoadCorps()
            {
                foreach (var c in corps)
                {
                    entities.Add(c.id, c);
                }
            }

            public void LoadCharacters()
            {
                foreach (var c in characters)
                {
                    entities.Add(c.id, c);
                }
            }

            public void LoadEnemies()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(enemiesResource);

                foreach (var file in jsonTextFiles)
                {
                    var character = JsonUtility.FromJson<Enemy>(file.text);
                    enemies.Add(character);
                    entities.Add(character.id, character);
                    Logger.Log(this, "Enemy added: ", character.id);
                }
            }

            public void LoadLocations()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(locationsResource);

                foreach (var file in jsonTextFiles)
                {
                    var location = JsonUtility.FromJson<Location>(file.text);
                    locations.Add(location);
                    entities.Add(location.id, location);
                    Logger.Log(this, "Location added: ", location.id);
                }

            }

            public List<InfoCategory> GetCategories()
            {
                return categories;
            }

            public InfoEntity GetEntity(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Character;
                }

                return null;

            }

            public Character GetCharacter(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Character;
                }

                return null;
            }

            public Enemy GetEnemy(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Enemy;
                }

                return null;
            }

            public Enemy GetUniqueEnemy(string uniqueId)
            {
                foreach (var v in uniqueEnemies)
                {
                    if (v.uniqueId == uniqueId)
                        return v;
                }
                return null;
            }

            public Corp GetCorp(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Corp;
                }

                return null;
            }

            public List<InfoEntity> GetKnownEntities(string category)
            {
                var infocategory = categories.FirstOrDefault(e => e.id == category);

                if (infocategory == null)
                {
                    return null;
                }

                return knownEntities.Where(kv => kv.Value.category == category).Select(kv => kv.Value).ToList();
            }

            public Enemy GenerateRandomEnemy(Enemy enemy)
            {
                if (enemy == null)
                {
                    Logger.LogError(name, "enemy can't be null");
                    return null;
                }

                Logger.Log(name, "generating unique enemy " + enemy.id);
                int uniqueId = uniqueEnemies.Count;

                var newEnemy = ScriptableObject.CreateInstance<Enemy>();

                newEnemy.id = enemy.id;
                newEnemy.uniqueId = enemy.id + uniqueId.ToString();
                newEnemy.name = enemy.name;
                newEnemy.read = enemy.read;
                newEnemy.parentInfoCategory = enemy.parentInfoCategory;
                newEnemy.description = enemy.description;
                newEnemy.handle = enemy.handle;
                newEnemy.corp = enemy.corp;
                newEnemy.serial = enemy.serial;
                newEnemy.category = enemy.category;

                uniqueEnemies.Add(newEnemy);
                return newEnemy;
            }

            public bool AddToKnownEntities(string id)
            {
                if (!knownEntities.ContainsKey(id))
                {
                    if (entities.ContainsKey(id))
                    {
                        var entity = entities[id];
                        Logger.Log(this, "Adding entity ", entity.id, " to known entities");
                        knownEntities.Add(id, entity);
                        ActionManager.instance.AddActionMessage("'" + entity.id + "' added to " + entity.category);
                    }
                    else
                    {
                        Logger.LogError(this, "Entity not found: ", id);
                        return false;
                    }
                }

                return false;
            }
        }
    }
}