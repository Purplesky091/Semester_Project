using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Vector2Int
    {
        private static Vector2Int zero = new Vector2Int(0, 0);
        public static Vector2Int Zero { get { return zero; } }

        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
