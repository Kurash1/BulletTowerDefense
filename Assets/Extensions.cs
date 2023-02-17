using UnityEngine;
namespace Extensions
{
    public static class Extensions
    {
        public static bool InRange(this int i, int min, int max)
        {
            return i >= min && i <= max;
        }
    }
}
