namespace MyGame.Pool
{
    public interface IPrewarmablePool
    {
        void Prewarm(int count);
    }
}