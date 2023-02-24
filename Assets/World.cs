using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Extensions;
using System;
using System.Linq;

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
        Spot Spot = GameObject.AddComponent<Spot>();
        Spot.spriteRenderer = SpriteRenderer;
        Spot.SetColor(cBlock);
        return Spot;
    }
#nullable disable
    public Sprite Square;
    public Sprite Triangle;
    public Sprite Background;
    public static World main;
    public static Point start;
    public static Point end;
    public static List<Point> Path = new List<Point>();
    public static List<Point> OptimalPath = new List<Point>();
    public static Color cBlock = Color.black;
    public static Color cClear = Color.red;
    public static Color cStart = Color.green;
    public static Color cEnd = Color.cyan;
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
            Path.Add(here);
            switch (x)
            {
                case -1:
                    current.SetColor(cStart);
                    start = here;
                    break;
                case 0:
                    current.SetColor(cClear);
                    break;
                case 1:
                    current.SetColor(cEnd);
                    end = here;
                    break;
            }
        }
        OptimizePath();
    }
    private void OptimizePath()
    {
        OptimalPath = FindOptimalPath();
    }
    private static List<Point> FindOptimalPath()
    {
        Dictionary<Point, int> costSoFar = new Dictionary<Point, int>();
        Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
        SortedSet<Point> frontier = new SortedSet<Point>(Comparer<Point>.Create((a, b) => costSoFar[a] - costSoFar[b]));
        Point start = Path[0];
        Point goal = Path[Path.Count - 1];

        costSoFar[start] = 0;
        frontier.Add(start);

        while (frontier.Count > 0)
        {
            Point current = frontier.First();
            frontier.Remove(current);

            if (current.Equals(goal))
            {
                // Construct the path by tracing back from the goal to the start
                List<Point> path = new List<Point>();
                Point next = goal;
                while (!next.Equals(start))
                {
                    path.Add(next);
                    next = cameFrom[next];
                }
                path.Add(start);
                path.Reverse();
                return path;
            }

            foreach (Point neighbor in GetNeighbors(current))
            {
                int newCost = costSoFar[current] + GetDistance(current, neighbor);
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    cameFrom[neighbor] = current;
                    if (!frontier.Contains(neighbor))
                    {
                        frontier.Add(neighbor);
                    }
                }
            }
        }

        // If we reached here, there is no path from start to goal
        return null;
    }
    private static List<Point> GetNeighbors(Point current)
    {
        List<Point> neighbors = new List<Point>();

        // Check the four cardinal directions
        Point north = new Point(current.X, current.Y + 1);
        Point south = new Point(current.X, current.Y - 1);
        Point east = new Point(current.X + 1, current.Y);
        Point west = new Point(current.X - 1, current.Y);

        // Add any valid neighbors to the list
        if (Path.Contains(north))
        {
            neighbors.Add(north);
        }
        if (Path.Contains(south))
        {
            neighbors.Add(south);
        }
        if (Path.Contains(east))
        {
            neighbors.Add(east);
        }
        if (Path.Contains(west))
        {
            neighbors.Add(west);
        }

        return neighbors;
    }
    private static int GetDistance(Point a, Point b)
    {
        // Calculate the Manhattan distance between Points a and b
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);
        return dx + dy;
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
                MapGet(start).SetColor(cClear);
                MapGet(point).SetColor(cStart);
                start = point;
                Path.Insert(0, point);
                OptimizePath();
            }
            static bool valid(Point a)
            {
                if (a.X == start.X)
                {
                    if(a.Y.InRange(start.Y - 1, start.Y + 1))
                        if(GetNeighbors(a).Count == 1)
                            return MapGet(a).spriteRenderer.color != cClear;
                }
                else if (a.Y == start.Y)
                {
                    if(a.X.InRange(start.X - 1, start.X + 1))
                        if (GetNeighbors(a).Count == 1)
                            return MapGet(a).spriteRenderer.color != cClear;
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
        if (color == World.cClear)
            spriteRenderer.sprite = World.main.Background;
        else
            spriteRenderer.sprite = World.main.Square;

        if (color == World.cBlock)
            gameObject.AddComponent<BoxCollider2D>();
        else
        {
            //Debug.Log($"Destory {this.ToString()}");
            Destroy(gameObject.GetComponent<BoxCollider2D>());
        }

        spriteRenderer.color = color;
    }
    public void Start()
    {
        gameObject.name = ((Point)transform.position).ToString();
    }
    public override string ToString()
    {
        return $"This Spot {transform.position.ToString()}";
    }
}