public class Useful
{
    private static readonly System.Random random = new System.Random();
    private static readonly object syncLock = new object();
    public static int RandomHero(int length) {
        lock (syncLock) {
            return random.Next(length);
        }
    }
}
