using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enhancer.Containers
{
    public struct Vector
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Vector operator *(Vector v, int scalar)
        {
            return new Vector { X = v.X * scalar, Y = v.Y * scalar };
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public override int GetHashCode()
        {
            return X * Y;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static readonly Vector NullVector = new Vector { X = 0, Y = 0 };
        public static readonly Vector IdentityVector = new Vector { X = 1, Y = 1 };

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
