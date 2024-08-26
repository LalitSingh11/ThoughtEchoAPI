namespace ThoughtEcho.Utilities
{
    public class UtilityFunctions
    {
        public static int GenerateRandomNumber()
        {
            Random random = new();
            return random.Next(100000, 999999);
        }

        public static bool Compare<T>(T a, T b) 
        {
            return (a?.ToString() == b?.ToString());
        }
    }
}
