using UnityEngine;

namespace TestTask.Editable
{
    // We're creating new functions for Color32 since Unity does not provide conversions to any primitive
    public static  class Color32Extensions
    {
        public static Color32 ToColor32(this int colorAsInt)
        {
            return new Color32(
                r: (byte)((colorAsInt >> 24) & 0xFF),
                g: (byte)((colorAsInt >> 16) & 0xFF),
                b: (byte)((colorAsInt >> 8) & 0xFF),
                a: (byte)((colorAsInt >> 0) & 0xFF)
            );
        }

        public static int ToInt(this Color32 color)
        {
            return (color.r << 24) | (color.g << 16) | (color.b << 8) | (color.a << 0);
        }
    }
}