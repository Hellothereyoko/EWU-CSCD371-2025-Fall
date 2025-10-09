namespace CanHazFunny
{
    public interface IJokeService
    {
        string GetJoke();
    }

    class jokeService : IJokeService
    {
        public string GetJoke()
        {

            JokeService jokeService1 = new JokeService();
            return jokeService1.GetJoke();
        }
    }
}