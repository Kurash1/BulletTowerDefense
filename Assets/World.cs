using System.Collections.Generic;
using UnityEngine;
using Extensions;
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
    public static Spot? MapAdd(int x, int y)
    {
        return MapAdd(new Point(x, y));
    }
    public static Spot? MapAdd(Point a)
    {
        if (Map.ContainsKey(a))
            return null;
        Map.Add(a, SpotConstructor(a));
        return Map[a];
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
    public Sprite Square;
    public Sprite Triangle;
    public static World main;
    public static Point start;
    public static Point end;
    public static List<Point> path = new List<Point>();
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
            Point here = new Point(x, y);
            Spot current = MapGet(x, y);
            if (current == null)
                continue;
            path.Add(here);
            switch (x)
            {
                case -1:
                    current.SetColor(Color.red);
                    start = here;
                    break;
                case 0:
                    current.SetColor(Color.grey);
                    break;
                case 1:
                    current.SetColor(Color.cyan);
                    end = here;
                    break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject @object = new GameObject();
                @object.AddComponent<PathFollower>();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Point point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (valid(point))
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;
                        MapAdd(point + (x, y));
                    }
                }
                MapGet(start).SetColor(Color.grey);
                MapGet(point).SetColor(Color.red);
                start = point;
                path.Insert(0, point);
            }
            static bool valid(Point a)
            {
                if (a.X == start.X)
                {
                    if(a.Y.InRange(start.Y - 1, start.Y + 1))
                        return MapGet(a).spriteRenderer.color != Color.grey;
                }
                else if (a.Y == start.Y)
                {
                    if(a.X.InRange(start.X - 1, start.X + 1))
                        return MapGet(a).spriteRenderer.color != Color.grey;
                }
                return false;
            }
        }
    }
}
public class Spot : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public void SetColor(Color color)
    {
        if (color == Color.black)
            gameObject.AddComponent<PolygonCollider2D>();
        else
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
        spriteRenderer.color = color;
    }
    public void Start()
    {
        gameObject.name = ((Point)transform.position).ToString();
        if (spriteRenderer.color == Color.black)
            gameObject.AddComponent<PolygonCollider2D>();
    }
    public override string ToString()
    {
        return $"This Spot {transform.position.ToString()}";
    }
}