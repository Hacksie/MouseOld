using UnityEngine;

namespace HackedDesign.Story
{
    public interface IScene
    {
        void Begin();
        bool Invoke(string actionName);
        void Next();
        bool Complete();
    }
}