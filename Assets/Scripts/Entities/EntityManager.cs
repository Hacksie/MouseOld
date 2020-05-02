using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Entities
    {
        public class EntityManager : MonoBehaviour
        {
            public static EntityManager instance;

            [Header("Prefabs")]
            public List<GameObject> enemies = null;
            public List<GameObject> traps = null;
            [SerializeField] private List<GameObject> npcPrefabList = null;

            public List<BaseEntity> npcPool = new List<BaseEntity>();

            [SerializeField] private GameObject npcPoolParent;


            EntityManager()
            {
                instance = this;
            }

            public void Initialize()
            {
                if (npcPoolParent == null)
                {
                    Logger.LogError(name, "npcParent is null");
                }

                foreach (var npc in npcPrefabList)
                {
                    if (npc == null)
                    {
                        continue;
                    }

                    var go = Instantiate(npc, Vector3.zero, Quaternion.identity, npcPoolParent.transform);
                    go.name = npc.name; // We don't want the (cloned) label
                    Logger.Log(name, "instantiating NPC ", npc.name, " ", go.name);
                    BaseEntity entity = go.GetComponent<BaseEntity>();

                    npcPool.Add(entity);
                    //go.SetActive(false);
                }
            }

            public BaseEntity GetPooledNPC(string name)
            {
                //return null;
                for(int i=0;i<npcPoolParent.transform.childCount;i++)
                {
                    if(npcPoolParent.transform.GetChild(i).name == name)
                    {
                        return npcPoolParent.transform.GetChild(i).gameObject.GetComponent<BaseEntity>();
                    }
                }
                return null;
                //return npcPool.FirstOrDefault(g => g.gameObject.name == name);

            }

        }
    }
}