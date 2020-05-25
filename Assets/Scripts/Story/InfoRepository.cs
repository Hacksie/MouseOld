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
            public List<Trap> traps = new List<Trap>();
            public List<Location> locations = new List<Location>();
            public List<Floor> floors = new List<Floor>();
            public Dictionary<string, InfoEntity> entities = new Dictionary<string, InfoEntity>();

            [Header("State")]
            public Dictionary<string, InfoEntity> knownEntities = new Dictionary<string, InfoEntity>();
            public List<Trap> uniqueTraps = new List<Trap>();
            public List<Enemy> uniqueEnemies = new List<Enemy>();

            public string SelectedInfoCategory { get; set; }
            public string SelectedInfoEntity { get; set; }

            public InfoRepository()
            {
                Instance = this;
            }

            private void Start()
            {
                LoadCorps();
                LoadCharacters();
                LoadLocations();
                LoadFloors();
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

            public void LoadLocations()
            {
                foreach (var l in locations)
                {
                    entities.Add(l.id, l);
                }
            }

            public void LoadFloors()
            {
                foreach (var f in floors)
                {
                    entities.Add(f.id, f);
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
                    return entities[id];
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

            public Trap GetTrap(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Trap;
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

            public Trap GetUniqueTrap(string uniqueId)
            {
                foreach (var trap in uniqueTraps)
                {
                    if (trap.uniqueId == uniqueId)
                        return trap;
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

            public Location GetLocation(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Location;
                }

                return null;
            }

            public Floor GetFloor(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Floor;
                }
                return null;
            }

            public List<Location> GetKnownLocations()
            {
                var infocategory = categories.FirstOrDefault(e => e.id == "Locations");

                if (infocategory == null)
                {
                    return null;
                }

                return knownEntities.Where(kv => kv.Value.category == "Locations").Select(kv => kv.Value as Location).ToList();
            }

            public List<Floor> GetKnownFloorsForLocation(string locationId)
            {
                return knownEntities.Where(kv => kv.Value.category == "Floors")
                                    .Where(f => (f.Value as Floor).locationId == locationId)
                                    .Select(kv => (kv.Value as Floor))
                                    .ToList();
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

            public Trap GenerateRandomTrap(Trap trap)
            {
                if (trap == null)
                {
                    Logger.LogError(name, "enemy can't be null");
                    return null;
                }

                Logger.Log(name, "generating unique enemy " + trap.id);
                int uniqueId = uniqueEnemies.Count;

                var newTrap = ScriptableObject.CreateInstance<Trap>();

                newTrap.id = trap.id;
                newTrap.uniqueId = trap.id + uniqueId.ToString();
                newTrap.name = trap.name;
                newTrap.read = trap.read;
                newTrap.parentInfoCategory = trap.parentInfoCategory;
                newTrap.description = trap.description;
                newTrap.handle = trap.handle;
                newTrap.corp = trap.corp;
                newTrap.serial = trap.serial;
                newTrap.category = trap.category;

                uniqueTraps.Add(newTrap);
                return newTrap;
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
                        ActionManager.Instance.AddActionMessage("'" + entity.id + "' added to " + entity.category);
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