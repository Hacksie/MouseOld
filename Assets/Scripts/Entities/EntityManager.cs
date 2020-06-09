using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class EntityManager : MonoBehaviour
    {
        public static EntityManager Instance { get; private set; }

        [Header("Prefabs")]
        public List<GameObject> enemies = null;
        public List<GameObject> traps = null;
        [SerializeField] private List<GameObject> npcPrefabList = null;
        public List<IEntity> npcPool = new List<IEntity>();
        [SerializeField] private GameObject npcPoolParent = null;

        public EntityManager() => Instance = this;

        public void Initialize()
        {
            if (npcPoolParent == null)
            {
                Logger.LogError(name, "npcParent is null");
                return;
            }

            foreach (var npc in npcPrefabList.Where(n => n != null))
            {

                var go = Instantiate(npc, Vector3.zero, Quaternion.identity, npcPoolParent.transform);
                go.name = npc.name; // We don't want the (cloned) label
                Logger.Log(name, "instantiating NPC ", npc.name, " ", go.name);
                IEntity entity = go.GetComponent<IEntity>();
                entity.Deactivate();
                npcPool.Add(entity);
            }
        }

        public bool EnemyIsValid(string name) => enemies.Exists(e => e.name == name);
        public bool TrapIsValid(string name) => traps.Exists(e => e.name == name);
        public bool NPCIsValid(string name) => npcPrefabList.Exists(e => e.name == name);

        public GameObject GetEnemyPrefabByName(string name)
        {
            if (enemies.Count == 0) return null;
            return enemies.FirstOrDefault(prefab => prefab != null && prefab.name == name);
        }

        public GameObject GetTrapPrefabByName(string name)
        {
            if (traps.Count == 0) return null;
            return traps.FirstOrDefault(prefab => prefab != null && prefab.name == name);
        }        

        public IEntity GetPooledNPC(string name)
        {
            for (int i = 0; i < npcPoolParent.transform.childCount; i++)
            {
                if (npcPoolParent.transform.GetChild(i).name == name)
                {
                    return npcPoolParent.transform.GetChild(i).gameObject.GetComponent<IEntity>();
                }
            }
            return null;
        }
    }
}