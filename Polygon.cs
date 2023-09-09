using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Drawing;

public abstract class Polygon
{
    public List<Vector3> Points { get; protected set; }
    public double AverageDistance { get; protected set; }
    public Color Color { get; set; }
    public List<Point> PointsInScreen { get; set; } = new List<Point>();
    public virtual void AddPoint(Vector3 point)
        => Points.Add(point);
    public virtual bool CheckIfInFov()
    {
        PointsInScreen.Clear();

        bool inFov = false;
        var distance = 0.0;
        var Fov = Engine.Current.FieldOfView;

        foreach (var point in Points)
        {
            var pointInScreen = Fov.GetPointInScreen(point);
            PointsInScreen.Add(pointInScreen);

            if (Fov.CheckIfPointInFov(point))
                inFov = true;

            distance += Vector3.Distance(Engine.Current.Camera.CameraPosition, point);
        }

        AverageDistance = distance / PointsInScreen.Count;

        return inFov;
    }
}

public class Triangle : Polygon
{
    public Triangle(Vector3[] points, Color color)
    {
        Color = color;

        if (points.Length != 3)
            throw new ArgumentException("A triangle needs 3 points to be instantiated.");

        Points = points.ToList();
    }
}

public class Hexagon : Polygon
{
    public Hexagon(Vector3[] points, Color color)
    {
        Color = color;

        if (points.Length != 6)
            throw new ArgumentException("A hexagon needs 6 points to be instantiated.");

        Points = points.ToList();
    }
}

public class Pentagon : Polygon
{
    public Pentagon(Vector3[] points, Color color)
    {
        Color = color;

        if (points.Length != 5)
            throw new ArgumentException("A pentagon needs 5 points to be instantiated.");

        Points = points.ToList();
    }
}