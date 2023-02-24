using UnityEngine;
public class Point
{
    public int Y;
    public int X;
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Point(Point a)
    {
        X = a.X;
        Y = a.Y;
    }
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.X + b.X, a.Y + b.Y);
    }
    public static Point operator +(Point a, (int, int) b)
    {
        return a + new Point(b.Item1, b.Item2);
    }
    public override string ToString()
    {
        return $"Point({X},{Y})";
    }
    public static implicit operator Vector3(Point a)
    {
        return new Vector3(a.X, a.Y);
    }
    public static implicit operator Vector2(Point a)
    {
        return new Vector2(a.X, a.Y);
    }
    public static implicit operator Point(Vector3 a)
    {
        return new Point(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y));
    }
    public static implicit operator Point(Vector2 a)
    {
        return new Point(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y));
    }
    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, 0);
    }
    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }
    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }
    public bool Equals(Point point)
    {
        return point.X == X && point.Y == Y;
    }

    public override bool Equals(object o)
    {
        return this.Equals(o as Point);
    }
}