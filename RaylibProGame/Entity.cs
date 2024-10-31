namespace RaylibProGame
{
    internal abstract class Entity
    {
        //is called once, when the game is starting
        public abstract void StartEntity();
        //is ran every game loop
        public abstract void EntityUpdate();
        //is ran every game loop
        public abstract void EntityDraw();
    }
}
