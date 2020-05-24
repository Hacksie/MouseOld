namespace HackedDesign
{
    public interface IState
    {
        void Begin();
        void Update();
        void LateUpdate(); 
        void End();
        void Interact();
        void Hack();
        void Dash();
        void Overload();
        void Start();
        void Select();

        //bool PlayerActionAllowed { get; }
    }   
}