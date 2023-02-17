using System.Collections.Generic;
using UnityEngine;
public class World : MonoBehaviour
{
    private static readonly Dictionary<Point, Spot> Map = new Dictionary<Point, Spot>();
#nullable enable
    public static Spot? MapGet(int x, int y)
    {
        return MapGet(new Point(x, y));
    }
    public static Spot? MapGet(Point a)
    {
        if (Map.ContainsKey(a))
            return Map[a];
        return null;
    }
    public static bool MapAdd(int x, int y)
    {
        return MapAdd(new Point(x, y));
    }
    public static bool MapAdd(Point a)
    {
        if (Map.ContainsKey(a))
            return false;
        Map.Add(a, SpotConstructor(a));
        return true;
    }
    public static Spot SpotConstructor(Point a)
    {
        GameObject GameObject = new GameObject();
        GameObject.transform.position = a.ToVector3();
        SpriteRenderer SpriteRenderer = GameObject.AddComponent<SpriteRenderer>();
        SpriteRenderer.sprite = main.Square;
        SpriteRenderer.color = Color.black;
        Spot Spot = GameObject.AddComponent<Spot>();
        Spot.spriteRenderer = SpriteRenderer;
        return Spot;
    }
#nullable disable
    public static World main;
    public Sprite Square;
    void Start()
    {
        if (main != null)
            Destroy(this);
        main = this;
        for(int x = -2; x <= 2; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (!MapAdd(x, y))
                    throw new System.IndexOutOfRangeException();
            }
        }
        for(int x = -1; x <= 1; x++)
        {
            const int y = 0;
            Spot current = MapGet(x, y);
            if (current == null)
                continue;
            current.spriteRenderer.color = Color.white;
        }
    }
}
public class Point
{
    public int Y;
    public int X;
    public Point(int x, int y)
    {
        X = x;
        Y = y;
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
public class Spot : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
}