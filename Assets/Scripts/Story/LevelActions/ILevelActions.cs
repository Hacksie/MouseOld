using UnityEngine;

namespace HackedDesign.Story
{
    public interface ILevelActions
    {
        bool Invoke(string actionName);
    }
}