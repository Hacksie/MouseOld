using UnityEngine;
namespace HackedDesign
{
    [CreateAssetMenu(fileName = "EntitySpriteConfig", menuName = "Mouse/Content/Entity Sprite Config")]
    [System.Serializable]
    public class EntitySprites : ScriptableObject
    {
        [SerializeField] private Sprite chatSprite = null;
        [SerializeField] private Sprite lookSprite = null;
        [SerializeField] private Sprite passiveSprite = null;
        [SerializeField] private Sprite alertSprite = null;
        [SerializeField] private Sprite huntSprite = null;
        [SerializeField] private Sprite seekingSprite = null;
        [SerializeField] private Sprite doorSprite = null;
        [SerializeField] private Sprite lockSprite = null;
        [SerializeField] private Sprite unlockSprite = null;
        [SerializeField] private Sprite interactSprite = null;
        [SerializeField] private Sprite hackSprite = null;
        [SerializeField] private Sprite overloadSprite = null;
        [SerializeField] private Sprite hackOrOverloadSprite = null;
        [SerializeField] private Sprite taskSprite = null;
        [SerializeField] private Sprite exitSprite = null;

        public Sprite GetSprite(EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Chat:
                    return chatSprite;
                case EntityState.Look:
                    return lookSprite;
                case EntityState.Patrol:
                    return passiveSprite;
                case EntityState.Alert:
                    return alertSprite;
                case EntityState.Seek:
                    return seekingSprite;
                case EntityState.Enter:
                    return doorSprite;
                case EntityState.Interact:
                    return interactSprite;
                case EntityState.Locked:
                    return lockSprite;
                case EntityState.Unlocked:
                    return unlockSprite;
                case EntityState.Hack:
                    return hackSprite;
                case EntityState.Overload:
                    return overloadSprite;
                case EntityState.HackOrOverload:
                    return hackOrOverloadSprite;
                case EntityState.Exit:
                    return exitSprite;
                case EntityState.Task:
                    return taskSprite;
            }

            return null;
        }        
    }
}