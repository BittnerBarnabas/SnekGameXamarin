using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnekGame.Model
{
    public enum Direction
    {
        UP,DOWN,LEFT,RIGHT

    }

    public static class Extension
    {
        public static Direction getInverse(this Direction dir)
        {
            switch (dir)
            {
                case Direction.UP:
                    return Direction.DOWN;
                case Direction.DOWN:
                    return Direction.UP;
                case Direction.LEFT:
                    return Direction.RIGHT;
                case Direction.RIGHT:
                    return Direction.LEFT;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }            
        }
    }
}
