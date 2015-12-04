using System;
using UnityEngine;

namespace Assets.Scripts
{
    class ColorConverter
    {
        /// <summary>
        /// When you make a new color in Unity, it needs the r, g, b values passed into the constructor.
        /// Those r g b values aren't the normal rgb values you'd get off a color picker.
        /// Instead, they're your rgb values/255. 
        /// It's incredibly annoying to convert from the standard rgb valeus -> unity rgb values,
        /// So this is a convience method to convert it for me.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color GetUnityColor(float r, float g, float b)
        {
            if (r > 255)
                r = 255f;

            if (g > 255)
                g = 255f;

            if (b > 255)
                b = 255f;

            r /= 255f;
            g /= 255f;
            b /= 255f;

            return new Color(r, g, b);
        }
    }
}
