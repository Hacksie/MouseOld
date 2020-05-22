namespace HackedDesign
{
    public interface IState
    {
        void Start();
        void Update();
        void LateUpdate(); 
        void End();
    }   
}