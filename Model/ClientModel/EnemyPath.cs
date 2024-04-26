using System.Collections.Generic;
using System.Linq;
using Godot;

public class EnemyPath
{
    private Curve2D _curve;
    private List<Vector2> _edges = new();
    private List<float> _edgeLengths = new();

    public float Length => _edgeLengths.Sum();

    public static Curve2D CreateWindingPathCurve()
    {
        var windingPath = new Curve2D();
        windingPath.AddPoint(new Vector2(880, 470));
        windingPath.AddPoint(new Vector2(880, 80));
        windingPath.AddPoint(new Vector2(688, 80));
        windingPath.AddPoint(new Vector2(688, 400));
        windingPath.AddPoint(new Vector2(496, 400));
        windingPath.AddPoint(new Vector2(496, 80));
        windingPath.AddPoint(new Vector2(304, 80));
        windingPath.AddPoint(new Vector2(304, 240));
        windingPath.AddPoint(new Vector2(144, 240));

        return windingPath;
    }

    public EnemyPath(Curve2D curve)
    {
        _curve = curve;
        for (int i = 0; i < curve.PointCount - 1; i++)
        {
            var p1 = curve.GetPointPosition(i);
            var p2 = curve.GetPointPosition(i + 1);
            _edges.Add(p2 - p1);
            _edgeLengths.Add(p2.DistanceTo(p1));
        }
    }

    public Vector2 GetPosition(float distanceOnPath)
    {
        int edgeIndex = 0;
        while (distanceOnPath > _edgeLengths[edgeIndex])
        {
            if (edgeIndex == _edges.Count - 1)
            {
                return _curve.GetPointPosition(edgeIndex + 1);
            }
            distanceOnPath -= _edgeLengths[edgeIndex];
            edgeIndex++;
        }

        float ratio = distanceOnPath / _edgeLengths[edgeIndex];
        var offset = _edges[edgeIndex] * ratio;
        return _curve.GetPointPosition(edgeIndex) + offset;
    }
}