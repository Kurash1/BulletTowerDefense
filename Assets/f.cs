using UnityEngine;
namespace Extensions
{
    public static class f
    {
        public static bool InRange(this int i, int min, int max)
        {
            return i >= min && i <= max;
        }
        public static Color getColor(int r, int g, int b)
        {
            return new Color((float)r / 255, (float)g / 255, (float)b / 255);
        }
    }
}
