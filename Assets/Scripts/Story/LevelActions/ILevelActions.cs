using UnityEngine;

namespace HackedDesign.Story
{
    public interface ILevelActions
    {
        void Begin();
        bool Invoke(string actionName);
        void Next();
    }
}